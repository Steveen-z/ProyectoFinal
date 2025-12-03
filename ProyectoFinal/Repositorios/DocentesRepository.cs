using ProyectoFinal.Clases; 
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace ProyectoFinal.Repositorios
{
    public class DocentesRepository
    {
        private readonly string _connectionString;

        public DocentesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AgregarDocente(Docentes docente, string clave, string rol)
        {
            string passwordHash = Encriptador.EncriptarClave(clave);

            string query = @"
        BEGIN TRANSACTION;
        
        INSERT INTO Usuarios (CodigoAcceso, PasswordHash, Rol)
        VALUES (@CodigoAcceso, @PasswordHash, @Rol);
        
        DECLARE @NuevoIdUsuario INT = SCOPE_IDENTITY();
        
        INSERT INTO Docentes (IdUsuario, Nombre, Apellido, DUI, Telefono, Email)
        VALUES (@NuevoIdUsuario, @Nombre, @Apellido, @DUI, @Telefono, @Email);

        COMMIT TRANSACTION;"; 

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CodigoAcceso", docente.CodigoAcceso);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@Rol", rol);

                cmd.Parameters.AddWithValue("@Nombre", docente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", docente.Apellido);
                cmd.Parameters.AddWithValue("@DUI", docente.DUI);
                cmd.Parameters.AddWithValue("@Telefono", docente.Telefono);
                cmd.Parameters.AddWithValue("@Email", docente.Email);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); 
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al agregar docente: " + ex.Message);
                    return false;
                }
            }
        }

public string GenerarCodigoDocenteUnico()
    {
        string prefijo = "D" + DateTime.Now.Year.ToString();
        string codigoGenerado;

        do
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                int aleatorio = BitConverter.ToInt32(bytes, 0) % 10000;
                if (aleatorio < 0) aleatorio *= -1;

                codigoGenerado = prefijo + aleatorio.ToString("D4"); 
            }
        }
        while (ObtenerDocentePorCodigoAcceso(codigoGenerado) != null);

        return codigoGenerado;
    }

    public DataTable ObtenerTodosLosDocentes(string filtroBusqueda = "")
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    D.IdDocente, 
                    D.Nombre, 
                    D.Apellido,
                    D.Nombre + ' ' + D.Apellido AS NombreCompleto,
                    D.DUI,
                    D.Email,
                    D.Telefono,
                    U.CodigoAcceso 
                FROM Docentes D
                INNER JOIN Usuarios U ON D.IdUsuario = U.IdUsuario
                WHERE D.Nombre + ' ' + D.Apellido LIKE '%' + @Filtro + '%'
                OR D.DUI LIKE '%' + @Filtro + '%'
                OR U.CodigoAcceso LIKE '%' + @Filtro + '%'
                ORDER BY D.Apellido, D.Nombre";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@Filtro", filtroBusqueda);
                try
                {
                    conn.Open();
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener docentes: " + ex.Message);
                }
            }
            return dt;
        }

        public bool EditarDocente(Docentes docente)
        {
            string query = @"
                UPDATE Docentes 
                SET Nombre = @Nombre, 
                    Apellido = @Apellido, 
                    DUI = @DUI, 
                    Email = @Email, 
                    Telefono = @Telefono
                WHERE IdDocente = @IdDocente;
            ";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdDocente", docente.IdDocente);
                cmd.Parameters.AddWithValue("@Nombre", docente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", docente.Apellido);
                cmd.Parameters.AddWithValue("@DUI", docente.DUI);
                cmd.Parameters.AddWithValue("@Email", docente.Email);
                cmd.Parameters.AddWithValue("@Telefono", docente.Telefono);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al editar docente: " + ex.Message);
                    return false;
                }
            }
        }
        public Docentes ObtenerDocentePorId(int idDocente)
        {
            Docentes docente = null;
            string query = @"
        SELECT 
            D.IdDocente, D.Nombre, D.Apellido, D.DUI, D.Telefono, D.Email, U.CodigoAcceso
        FROM Docentes D
        INNER JOIN Usuarios U ON D.IdUsuario = U.IdUsuario
        WHERE D.IdDocente = @IdDocente";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdDocente", idDocente);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            docente = new Docentes
                            {
                                IdDocente = (int)reader["IdDocente"],
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                DUI = reader["DUI"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Email = reader["Email"].ToString(),
                                CodigoAcceso = reader["CodigoAcceso"].ToString()
                                
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener docente por ID: " + ex.Message);
                }
            }
            return docente;
        }
        public bool EliminarDocente(int idDocente)
        {
            string query = @"
                BEGIN TRANSACTION;
                DECLARE @IdUsuarioAEliminar INT;
                SELECT @IdUsuarioAEliminar = IdUsuario FROM Docentes WHERE IdDocente = @IdDocente;
                DELETE FROM Docentes WHERE IdDocente = @IdDocente;
                DELETE FROM Usuarios WHERE IdUsuario = @IdUsuarioAEliminar;
                COMMIT TRANSACTION;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdDocente", idDocente);
                try { conn.Open(); cmd.ExecuteNonQuery(); return true; }
                catch (Exception ex) { Console.WriteLine("Error al eliminar docente: " + ex.Message); return false; }
            }
        }
        public bool ActualizarDocenteBase(Docentes docente)
        {
            string query = @"
        DECLARE @IdUsuarioExistente INT;
        SELECT @IdUsuarioExistente = IdUsuario FROM Usuarios WHERE CodigoAcceso = @CodigoAcceso;

        UPDATE Docentes 
        SET Nombre = @Nombre, 
            Apellido = @Apellido, 
            DUI = @DUI, 
            Email = @Email, 
            Telefono = @Telefono
        WHERE IdUsuario = @IdUsuarioExistente;

        -- UPDATE Usuarios SET Clave = @ClaveNueva WHERE IdUsuario = @IdUsuarioExistente;

        SELECT @@ROWCOUNT;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CodigoAcceso", docente.CodigoAcceso);
                cmd.Parameters.AddWithValue("@Nombre", docente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", docente.Apellido);
                cmd.Parameters.AddWithValue("@DUI", docente.DUI);
                cmd.Parameters.AddWithValue("@Email", docente.Email);
                cmd.Parameters.AddWithValue("@Telefono", docente.Telefono);

                try
                {
                    conn.Open();
                    int rowsAffected = (int)cmd.ExecuteScalar();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar docente base: " + ex.Message);
                    return false;
                }
            }
        }

        public bool ActualizarDocente(int idDocente, string nuevoEmail, string nuevoTelefono)
        {
            string query = @"
        UPDATE Docentes 
        SET Email = @Email, 
            Telefono = @Telefono
        WHERE IdDocente = @IdDocente";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdDocente", idDocente);
                cmd.Parameters.AddWithValue("@Email", nuevoEmail);
                cmd.Parameters.AddWithValue("@Telefono", nuevoTelefono);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar contacto del docente: " + ex.Message);
                    return false;
                }
            }
        }
        public bool VerificarClave(int idDocente, string claveActual)
        {
            string hashIngresado = Encriptador.EncriptarClave(claveActual);

            string query = @"
        SELECT U.PasswordHash 
        FROM Usuarios U
        INNER JOIN Docentes D ON U.IdUsuario = D.IdUsuario
        WHERE D.IdDocente = @IdDocente";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdDocente", idDocente);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar(); 

                    if (result != null)
                    {
                        string hashAlmacenado = result.ToString();
                        return hashIngresado.Equals(hashAlmacenado);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al verificar clave del docente: " + ex.Message);
                    return false;
                }
            }
        }

        public bool ActualizarClaveDocente(int idDocente, string nuevaClave)
        {
            string nuevoPasswordHash = Encriptador.EncriptarClave(nuevaClave);

            string query = @"
        DECLARE @IdUsuario INT;
        SELECT @IdUsuario = IdUsuario FROM Docentes WHERE IdDocente = @IdDocente;

        UPDATE Usuarios 
        SET PasswordHash = @NuevoPasswordHash
        WHERE IdUsuario = @IdUsuario";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdDocente", idDocente);
                cmd.Parameters.AddWithValue("@NuevoPasswordHash", nuevoPasswordHash);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar clave del docente: " + ex.Message);
                    return false;
                }
            }
        }
        public bool ActualizarClavePorCodigoAcceso(string codigoAcceso, string nuevaClave)
        {
            string nuevoPasswordHash = Encriptador.EncriptarClave(nuevaClave);

            string query = @"
        UPDATE Usuarios 
        SET PasswordHash = @NuevoPasswordHash
        WHERE CodigoAcceso = @CodigoAcceso";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CodigoAcceso", codigoAcceso);
                cmd.Parameters.AddWithValue("@NuevoPasswordHash", nuevoPasswordHash);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar clave por CodigoAcceso: " + ex.Message);
                    return false;
                }
            }
        }
        public bool ActualizarContactoDocente(Docentes docente)
        {
            string query = @"
        UPDATE Docentes 
        SET Email = @Email, 
            Telefono = @Telefono
        WHERE IdDocente = @IdDocente";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdDocente", docente.IdDocente);
                cmd.Parameters.AddWithValue("@Email", docente.Email);
                cmd.Parameters.AddWithValue("@Telefono", docente.Telefono);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar contacto del docente: " + ex.Message);
                    return false;
                }
            }
        }

        public DataTable ObtenerDocentePorCodigoAcceso(string codigoAcceso)
        {
            DataTable dt = new DataTable();

            string query = @"
        SELECT 
            D.IdDocente 
        FROM Docentes D
        INNER JOIN Usuarios U ON D.IdUsuario = U.IdUsuario
        WHERE U.CodigoAcceso = @CodigoAcceso";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@CodigoAcceso", codigoAcceso);

                try
                {
                    conn.Open();
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener docente por código de acceso: " + ex.Message);
                }
            }

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
    }
}