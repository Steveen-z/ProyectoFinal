using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ProyectoFinal.Clases;

namespace ProyectoFinal.Repositorios
{
    public class EstudiantesGestionRepository
    {
        private readonly string _connectionString;

        public EstudiantesGestionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private bool CodigoExiste(SqlConnection conn, SqlTransaction transaction, string codigo)
        {
            string query = "SELECT COUNT(*) FROM Usuarios WHERE CodigoAcceso = @Codigo";
            SqlCommand cmd = new SqlCommand(query, conn, transaction);
            cmd.Parameters.AddWithValue("@Codigo", codigo);

            return (int)cmd.ExecuteScalar() > 0;
        }

        private string GenerarCodigoUnico(SqlConnection conn, SqlTransaction transaction)
        {
            string codigoBase = "E" + DateTime.Now.Year.ToString();
            string codigo;
            Random rnd = new Random();

            do
            {
                string randomDigits = rnd.Next(1000, 9999).ToString("D4");
                codigo = codigoBase + randomDigits;
            } while (CodigoExiste(conn, transaction, codigo));

            return codigo;
        }

        public string AgregarEstudianteCompleto(Estudiantes estudiante, string claveTemporal)
        {
            string codigoAccesoGenerado = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    codigoAccesoGenerado = GenerarCodigoUnico(conn, transaction);
                    string rol = "Estudiante";

                    string passwordHash = Encriptador.EncriptarClave(claveTemporal);

                    string usuarioQuery = @"
                        INSERT INTO Usuarios (CodigoAcceso, PasswordHash, Rol)
                        VALUES (@CodigoAcceso, @PasswordHash, @Rol);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand usuarioCmd = new SqlCommand(usuarioQuery, conn, transaction);
                    usuarioCmd.Parameters.AddWithValue("@CodigoAcceso", codigoAccesoGenerado);
                    usuarioCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    usuarioCmd.Parameters.AddWithValue("@Rol", rol);

                    int idUsuario = Convert.ToInt32(usuarioCmd.ExecuteScalar());

                    string estudianteQuery = @"
                        INSERT INTO Estudiantes (Nombre, Apellido, IdNivel, IdEspecializacion, IdUsuario) 
                        VALUES (@Nombre, @Apellido, @IdNivel, @IdEspecializacion, @IdUsuario)";

                    SqlCommand estudianteCmd = new SqlCommand(estudianteQuery, conn, transaction);
                    estudianteCmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                    estudianteCmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                    estudianteCmd.Parameters.AddWithValue("@IdNivel", estudiante.IdNivel);
                    estudianteCmd.Parameters.AddWithValue("@IdEspecializacion", (object)estudiante.IdEspecializacion ?? DBNull.Value);
                    estudianteCmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    estudianteCmd.ExecuteNonQuery();

                    transaction.Commit();
                    return codigoAccesoGenerado;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error al agregar estudiante: " + ex.Message);
                    return null;
                }
            }
        }

        public string ObtenerPasswordHash(int idUsuario)
        {
            string hash = null;
            string query = "SELECT PasswordHash FROM Usuarios WHERE IdUsuario = @IdUsuario";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null) hash = result.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener hash: {ex.Message}");
                }
            }
            return hash;
        }

        public bool ActualizarClave(int idUsuario, string nuevoPasswordHash)
        {
            string query = "UPDATE Usuarios SET PasswordHash = @PasswordHash WHERE IdUsuario = @IdUsuario";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@PasswordHash", nuevoPasswordHash);
                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar clave: {ex.Message}");
                    return false;
                }
            }
        }

        public DataTable ObtenerEstudiantesFiltrados(int? idNivel, int? idEspecializacion)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    E.IdEstudiante, E.Nombre, E.Apellido, 
                    E.IdNivel, E.IdEspecializacion,
                    CONCAT(N.Anio, '° ', N.TipoBachillerato) AS Nivel,
                    ES.NombreEspecializacion AS Especializacion,  
                    U.CodigoAcceso 
                FROM Estudiantes E
                INNER JOIN Usuarios U ON E.IdUsuario = U.IdUsuario
                INNER JOIN NivelesEducativos N ON E.IdNivel = N.IdNivel
                LEFT JOIN Especializaciones ES ON E.IdEspecializacion = ES.IdEspecializacion
                WHERE 
                    (@IdNivel IS NULL OR E.IdNivel = @IdNivel) AND
                    (@IdEspecializacion IS NULL OR E.IdEspecializacion = @IdEspecializacion)
                ORDER BY E.Apellido, E.Nombre";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdNivel", (object)idNivel ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)idEspecializacion ?? DBNull.Value);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener estudiantes filtrados: " + ex.Message);
                }
            }
            return dt;
        }

        public Dictionary<int, string> ObtenerNiveles()
        {
            Dictionary<int, string> niveles = new Dictionary<int, string>();
            string query = "SELECT IdNivel, TipoBachillerato, Anio FROM NivelesEducativos WHERE IdNivel > 0 ORDER BY Anio, TipoBachillerato";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("IdNivel")) ||
                            reader.IsDBNull(reader.GetOrdinal("TipoBachillerato")) ||
                            reader.IsDBNull(reader.GetOrdinal("Anio")))
                        {
                            continue;
                        }

                        int id = reader.GetInt32(reader.GetOrdinal("IdNivel"));
                        string tipo = reader.GetString(reader.GetOrdinal("TipoBachillerato"));
                        int anio = reader.GetInt32(reader.GetOrdinal("Anio"));

                        if (id > 0 && !string.IsNullOrWhiteSpace(tipo))
                        {
                            niveles.Add(id, $"{anio}° {tipo}");
                        }
                    }
                }
            }
            return niveles;
        }

        public Dictionary<int, string> ObtenerEspecializaciones()
        {
            Dictionary<int, string> especializaciones = new Dictionary<int, string>();
            string query = "SELECT IdEspecializacion, NombreEspecializacion FROM Especializaciones WHERE IdEspecializacion > 0 ORDER BY NombreEspecializacion";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("IdEspecializacion")) ||
                            reader.IsDBNull(reader.GetOrdinal("NombreEspecializacion")))
                        {
                            continue;
                        }

                        int id = reader.GetInt32(reader.GetOrdinal("IdEspecializacion"));
                        string nombre = reader.GetString(reader.GetOrdinal("NombreEspecializacion"));

                        if (id > 0 && !string.IsNullOrWhiteSpace(nombre))
                        {
                            especializaciones.Add(id, nombre);
                        }
                    }
                }
            }
            return especializaciones;
        }

        public List<NivelesEducativos> ObtenerNivelesClase()
        {
            List<NivelesEducativos> niveles = new List<NivelesEducativos>();
            string query = "SELECT IdNivel, TipoBachillerato, Anio FROM NivelesEducativos WHERE IdNivel > 0 ORDER BY Anio, TipoBachillerato";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("IdNivel")) ||
                            reader.IsDBNull(reader.GetOrdinal("TipoBachillerato")) ||
                            reader.IsDBNull(reader.GetOrdinal("Anio")))
                        {
                            continue;
                        }

                        niveles.Add(new NivelesEducativos(
                            idNivel: reader.GetInt32(reader.GetOrdinal("IdNivel")),
                            tipoBachillerato: reader.GetString(reader.GetOrdinal("TipoBachillerato")),
                            anio: reader.GetInt32(reader.GetOrdinal("Anio"))
                        ));
                    }
                }
            }
            return niveles;
        }

        public List<Especializaciones> ObtenerEspecializacionesClase()
        {
            List<Especializaciones> especializaciones = new List<Especializaciones>();
            string query = "SELECT IdEspecializacion, NombreEspecializacion FROM Especializaciones WHERE IdEspecializacion > 0 ORDER BY NombreEspecializacion";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("IdEspecializacion")) ||
                            reader.IsDBNull(reader.GetOrdinal("NombreEspecializacion")))
                        {
                            continue;
                        }

                        especializaciones.Add(new Especializaciones(
                            idEspecializacion: reader.GetInt32(reader.GetOrdinal("IdEspecializacion")),
                            nombreEspecializacion: reader.GetString(reader.GetOrdinal("NombreEspecializacion"))
                        ));
                    }
                }
            }
            return especializaciones;
        }

        public Estudiantes ObtenerEstudiantePorId(int idEstudiante)
        {
            string query = @"
        SELECT 
            E.IdEstudiante, E.Nombre, E.Apellido, E.IdNivel, E.IdEspecializacion, U.CodigoAcceso, E.IdUsuario
        FROM 
            Estudiantes E
        INNER JOIN Usuarios U ON E.IdUsuario = U.IdUsuario
        WHERE 
            E.IdEstudiante = @IdEstudiante";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Estudiantes
                        {
                            IdEstudiante = reader.GetInt32(reader.GetOrdinal("IdEstudiante")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            IdNivel = reader.GetInt32(reader.GetOrdinal("IdNivel")),

                            IdEspecializacion = reader.IsDBNull(reader.GetOrdinal("IdEspecializacion"))
                                                     ? (int?)null
                                                     : reader.GetInt32(reader.GetOrdinal("IdEspecializacion")),

                            CodigoAcceso = reader.GetString(reader.GetOrdinal("CodigoAcceso")),
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario"))
                        };
                    }
                }
            }
            return null;
        }

        public DataTable ObtenerEstudiantesFiltrados(int? idNivel, int? idEspecializacion, string nombre)
        {
            DataTable dt = new DataTable();
            string query = @"
        SELECT 
            E.IdEstudiante, E.Nombre, E.Apellido, 
            E.IdNivel, E.IdEspecializacion,
            CONCAT(N.Anio, '° ', N.TipoBachillerato) AS Nivel,
            ES.NombreEspecializacion AS Especializacion,
            U.CodigoAcceso 
        FROM Estudiantes E
        INNER JOIN Usuarios U ON E.IdUsuario = U.IdUsuario
        INNER JOIN NivelesEducativos N ON E.IdNivel = N.IdNivel
        LEFT JOIN Especializaciones ES ON E.IdEspecializacion = ES.IdEspecializacion
        WHERE 
            (@IdNivel IS NULL OR E.IdNivel = @IdNivel) AND
            (@IdEspecializacion IS NULL OR E.IdEspecializacion = @IdEspecializacion) AND
            (@NombreFiltro IS NULL OR E.Nombre LIKE '%' + @NombreFiltro + '%' OR E.Apellido LIKE '%' + @NombreFiltro + '%')
        ORDER BY E.Apellido, E.Nombre";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdNivel", (object)idNivel ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)idEspecializacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NombreFiltro", (object)nombre ?? DBNull.Value);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener estudiantes filtrados: " + ex.Message);
                }
            }
            return dt;
        }

        public int ObtenerIdUsuarioPorCodigoAcceso(string codigoAcceso)
        {
            string query = "SELECT IdUsuario FROM Usuarios WHERE CodigoAcceso = @CodigoAcceso";
            int idUsuario = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CodigoAcceso", codigoAcceso);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idUsuario = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al buscar usuario por código: {ex.Message}");
                }
            }
            return idUsuario;
        }
        public bool ActualizarEstudiante(int idEstudiante, string nombre, string apellido, int nuevoIdNivel, int? nuevoIdEspecializacion)
        {
            string query = @"
                UPDATE Estudiantes 
                SET 
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    IdNivel = @IdNivel, 
                    IdEspecializacion = @IdEspecializacion
                WHERE 
                    IdEstudiante = @IdEstudiante";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@IdNivel", nuevoIdNivel);
                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)nuevoIdEspecializacion ?? DBNull.Value);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar el estudiante: {ex.Message}");
                    return false;
                }
            }
        }
    }
}