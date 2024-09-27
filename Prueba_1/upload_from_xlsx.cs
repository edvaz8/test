using OfficeOpenXml;
using MySql.Data.MySqlClient;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string fi
            
            lePath = @"C:\ruta\al\archivo.xlsx";
        string connectionString = "Server=localhost;Database=prueba_tecnica;Uid=root;Pwd=1264vazquez;"
        
#
        FileInfo fileInfo = new FileInfo(filePath);

        using (ExcelPackage package = new ExcelPackage(fileInfo)) //excel 
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                for (int row = 2; row <= rowCount; row++) //initial row 2 bc 1 is header
                {
                    string login = worksheet.Cells[row, 1].Value.ToString();
                    string nombres = worksheet.Cells[row, 2].Value.ToString();
                    string paterno = worksheet.Cells[row, 3].Value.ToString();
                    string materno = worksheet.Cells[row, 4].Value.ToString();

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

        Console.WriteLine("Data inserted.");
    }
}
