using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    public class Usuario
    {
        public string Login { get; set; }
        public string NombreCompleto { get; set; }
        public double Sueldo { get; set; }
        public DateTime FechaIngreso { get; set; }
    }

    public interface IUsuarioRepository
    {
        List<Usuario> ObtenerTopUsuarios(int top);
        List<Usuario> ObtenerTodosLosUsuarios();
        void ActualizarSueldo(string login, double nuevoSueldo);
        void AgregarUsuario(Usuario usuario);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string connectionString;

        public UsuarioRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Usuario> ObtenerTopUsuarios(int top)
        {
            var usuarios = new List<Usuario>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT u.Login, CONCAT(u.Nombre, ' ', u.Paterno, ' ', u.Materno) AS NombreCompleto, e.Sueldo, e.FechaIngreso 
                FROM empleados e
                INNER JOIN usuarios u ON e.userId = u.userId
                ORDER BY e.Sueldo DESC 
                LIMIT @Top";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Top", top);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                Login = reader["Login"].ToString(),
                                NombreCompleto = reader["NombreCompleto"].ToString(),
                                Sueldo = Convert.ToDouble(reader["Sueldo"]),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"])
                            });
                        }
                    }
                }
            }
            return usuarios;
        }

        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            var usuarios = new List<Usuario>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT u.Login, CONCAT(u.Nombre, ' ', u.Paterno, ' ', u.Materno) AS NombreCompleto, e.Sueldo, e.FechaIngreso 
                FROM empleados e
                INNER JOIN usuarios u ON e.userId = u.userId
                ORDER BY e.Sueldo DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                Login = reader["Login"].ToString(),
                                NombreCompleto = reader["NombreCompleto"].ToString(),
                                Sueldo = Convert.ToDouble(reader["Sueldo"]),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"])
                            });
                        }
                    }
                }
            }
            return usuarios;
        }

        public void ActualizarSueldo(string login, double nuevoSueldo)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            UPDATE empleados e
            INNER JOIN usuarios u ON e.userId = u.userId
            SET e.Sueldo = @Sueldo
            WHERE u.Login = @Login";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Sueldo", nuevoSueldo);
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AgregarUsuario(Usuario usuario)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string queryInsertUsuario = @"
            INSERT INTO usuarios (Login, Nombre, Paterno, Materno) 
            VALUES (@Login, @Nombre, @Paterno, @Materno)";

                using (var cmd = new MySqlCommand(queryInsertUsuario, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", usuario.Login);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.NombreCompleto.Split(' ')[0]);
                    cmd.Parameters.AddWithValue("@Paterno", usuario.NombreCompleto.Split(' ')[1]);
                    cmd.Parameters.AddWithValue("@Materno", usuario.NombreCompleto.Split(' ')[2]);
                    cmd.ExecuteNonQuery();
                }

                string queryInsertEmpleado = @"
            INSERT INTO empleados (userId, Sueldo, FechaIngreso) 
            SELECT userId, @Sueldo, @FechaIngreso 
            FROM usuarios WHERE Login = @Login";

                using (var cmd = new MySqlCommand(queryInsertEmpleado, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", usuario.Login);
                    cmd.Parameters.AddWithValue("@Sueldo", usuario.Sueldo);
                    cmd.Parameters.AddWithValue("@FechaIngreso", usuario.FechaIngreso);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal class Program
        {
            private static string connectionString = "Server=localhost;Database=prueba_tecnica;Uid=root;Pwd=1264vazquez;";
            private static string path_csv = @"C:\Users\eduar\Documents\Usuarios.csv";

            static void Main(string[] args)
            {
                IUsuarioRepository usuarioRepo = new UsuarioRepository(connectionString);
                var top_usuarios = usuarioRepo.ObtenerTopUsuarios(10);
                Console.WriteLine("Top 10 Usuarios:");
                foreach (var usuario in top_usuarios)
                {
                    Console.WriteLine($"{usuario.Login}, {usuario.NombreCompleto}, {usuario.Sueldo}, {usuario.FechaIngreso}");
                }

                var todos_usuarios = usuarioRepo.ObtenerTodosLosUsuarios();
                Console.WriteLine("Listado de todos los usuarios:");
                foreach (var usuario in todos_usuarios)
                {
                    Console.WriteLine($"{usuario.Login}, {usuario.NombreCompleto}, {usuario.Sueldo}, {usuario.FechaIngreso}");
                }

                GenerarArchivoCsv(todos_usuarios, path_csv);

                try
                {
                    Console.Write("Ingrese el login del usuario para actualizar el salario (userXX): ");
                    string login = Console.ReadLine();
                    Console.Write("Ingrese el nuevo salario: ");

                    if (double.TryParse(Console.ReadLine(), out double nuevoSueldo))
                    {
                        usuarioRepo.ActualizarSueldo(login, nuevoSueldo);
                        Console.WriteLine("Salario actualizado con éxito.");
                    }
                    else
                    {
                        Console.WriteLine("Error: Ingrese un salario válido.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar el salario: {ex.Message}");
                }

                try
                {
                    Console.WriteLine("Agregar un nuevo usuario:");
                    Console.Write("Ingrese el login del nuevo usuario: ");
                    string nuevoLogin = Console.ReadLine();
                    Console.Write("Ingrese el nombre completo (nombre paterno materno): ");
                    string nuevoNombreCompleto = Console.ReadLine();
                    Console.Write("Ingrese el sueldo del nuevo usuario: ");

                    if (double.TryParse(Console.ReadLine(), out double nuevoSueldoEmpleado))
                    {
                        DateTime fechaIngreso = DateTime.Now;
                        Usuario nuevoUsuario = new Usuario
                        {
                            Login = nuevoLogin,
                            NombreCompleto = nuevoNombreCompleto,
                            Sueldo = nuevoSueldoEmpleado,
                            FechaIngreso = fechaIngreso
                        };
                        usuarioRepo.AgregarUsuario(nuevoUsuario);
                        Console.WriteLine("Nuevo usuario agregado con éxito.");
                    }
                    else
                    {
                        Console.WriteLine("Error: Ingrese un sueldo válido.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar el nuevo usuario: {ex.Message}");
                }
            }

            static void GenerarArchivoCsv(List<Usuario> usuarios,string path)
            {
                string csvFilePath = path;

                using (var writer = new StreamWriter(csvFilePath))
                {
                    writer.WriteLine("Login,Nombre Completo,Sueldo,Fecha Ingreso");
                    foreach (var usuario in usuarios)
                    {
                        writer.WriteLine($"{usuario.Login},{usuario.NombreCompleto},{usuario.Sueldo},{usuario.FechaIngreso.ToString("yyyy-MM-dd")}");
                    }
                }

                Console.WriteLine($"Archivo CSV generado en: {csvFilePath}");
            }
        }
    }
}
