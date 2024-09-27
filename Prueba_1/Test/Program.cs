using OfficeOpenXml;
using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.IO;

class Program
{
    static string filePath = @"C:\Users\eduar\Documents\prueba\test_dev_back_jr\testdevbackjr\Prueba_1\DatosPracticaSQL.xlsx"; //especificar donde se encuentra el xlsx
    static string connectionString = "Server=localhost;Database=prueba_tecnica;Uid=root;Pwd=admin;";

    static void Main(string[] args)
    {
        InsertarUsuarios();
        InsertarEmpleados();
    }

    static void InsertarUsuarios()
    {
        FileInfo fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists)
        {
            Console.WriteLine($"Archivo {filePath} no existe.");
            return;
        }

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            if (package.Workbook.Worksheets.Count == 0)
            {
                Console.WriteLine("El archivo no contiene hojas.");
                return;
            }

            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            if (worksheet.Dimension == null)
            {
                Console.WriteLine("La hoja está vacía.");
                return;
            }

            int rowCount = worksheet.Dimension.Rows;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                for (int row = 2; row <= rowCount; row++)
                {
                    string login = worksheet.Cells[row, 1].Value?.ToString();
                    string nombres = worksheet.Cells[row, 2].Value?.ToString();
                    string paterno = worksheet.Cells[row, 3].Value?.ToString();
                    string materno = worksheet.Cells[row, 4].Value?.ToString();

                    if (login == null || nombres == null || paterno == null || materno == null)
                    {
                        Console.WriteLine($"Datos incompletos en la fila {row}.");
                        continue;
                    }

                    string query = "INSERT INTO usuarios (Login, Nombre, Paterno, Materno) VALUES (@Login, @Nombre, @Paterno, @Materno)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login", login);
                        cmd.Parameters.AddWithValue("@Nombre", nombres);
                        cmd.Parameters.AddWithValue("@Paterno", paterno);
                        cmd.Parameters.AddWithValue("@Materno", materno);
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
        }
        Console.WriteLine("datos de usuarios insertados.");
    }

    static void InsertarEmpleados()
    {
        FileInfo fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists)
        {
            Console.WriteLine($"Archivo {filePath} no existe.");
            return;
        }

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            if (package.Workbook.Worksheets.Count < 2)
            {
                Console.WriteLine("El archivo no contiene suficientes hojas.");
                return;
            }

            ExcelWorksheet employeeWorksheet = package.Workbook.Worksheets[1];
            if (employeeWorksheet.Dimension == null)
            {
                Console.WriteLine("La hoja de empleados está vacía.");
                return;
            }

            int employeeRowCount = employeeWorksheet.Dimension.Rows;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                for (int row = 2; row <= employeeRowCount; row++)
                {
                    string login = employeeWorksheet.Cells[row, 1].Value?.ToString();
                    string sueldoStr = employeeWorksheet.Cells[row, 2].Value?.ToString();
                    var fechaIngresoValue = employeeWorksheet.Cells[row, 3].Value;

                    if (login == null || sueldoStr == null || fechaIngresoValue == null)
                    {
                        Console.WriteLine($"Datos incompletos en la fila {row}.");
                        continue;
                    }

                    sueldoStr = sueldoStr.Replace("$", "").Replace(",", "").Trim();
                    double sueldo;
                    if (!double.TryParse(sueldoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out sueldo))
                    {
                        Console.WriteLine($"Sueldo inválido en la fila {row}: {sueldoStr}");
                        continue;
                    }

                    DateTime fechaIngreso;
                    if (fechaIngresoValue is double fechaNum)
                    {
                        fechaIngreso = DateTime.FromOADate(fechaNum);
                    }
                    else if (!DateTime.TryParse(fechaIngresoValue.ToString(), out fechaIngreso))
                    {
                        Console.WriteLine($"Fecha de ingreso inválida en la fila {row}: {fechaIngresoValue}");
                        continue;
                    }

                    int userId;
                    string userIdQuery = "SELECT userId FROM usuarios WHERE Login = @Login";
                    using (MySqlCommand cmd = new MySqlCommand(userIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login", login);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            Console.WriteLine($"Login no encontrado en la fila {row}: {login}");
                            continue;
                        }
                        userId = Convert.ToInt32(result);
                    }

                    string insertQuery = "INSERT INTO empleados (userId, Sueldo, FechaIngreso) VALUES (@userId, @Sueldo, @FechaIngreso)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@Sueldo", sueldo);
                        cmd.Parameters.AddWithValue("@FechaIngreso", fechaIngreso);
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
        }
        Console.WriteLine("Datos de empleados insertados");
    }
}
