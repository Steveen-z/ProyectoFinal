using System;
using System.Collections.Generic; 
using System.Data.SqlClient;
using ProyectoFinal.Clases; 

namespace ProyectoFinal.Repositorios
{
    public class UsuariosRepository
    {
        private readonly string _connectionString;

        public UsuariosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        // 1. GENERADOR DE CÓDIGOS 

        private string GenerarCodigoUnico(string prefijo)
        {
            string codigoGenerado;
            bool existe;
            Random random = new Random();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                do
                {
                    // Genera un número de 8 dígitos asegurando que no empiece con 0
                    string numeros = random.Next(10000000, 99999999).ToString();

                    // La concatenación es limpia: Prefijo + 8 dígitos.
                    codigoGenerado = prefijo + numeros;

                    // Consulta SQL para verificar la existencia del código
                    string checkQuery = "SELECT COUNT(1) FROM Usuarios WHERE CodigoAcceso = @Codigo";
                    SqlCommand cmd = new SqlCommand(checkQuery, conn);
                    cmd.Parameters.AddWithValue("@Codigo", codigoGenerado);

                    int count = (int)cmd.ExecuteScalar();
                    existe = count > 0;

                } while (existe);
            }

            return codigoGenerado;
        }

        // 2. REGISTRO DE USUARIO 

        public int AgregarUsuario(string contraseña, string rol)
        {
            string codigoAcceso;

            // Determinar y validar el prefijo
            if (rol == "Estudiante")
            {
                codigoAcceso = GenerarCodigoUnico("E");
            }
            else if (rol == "Docente")
            {
                codigoAcceso = GenerarCodigoUnico("D");
            }
            else
            {
                throw new InvalidOperationException($"No se permite la generación automática de códigos para el rol: {rol}.");
            }

            // Encriptar la contraseña y realizar la inserción
            string hash = Encriptador.EncriptarClave(contraseña);
            int newIdUsuario = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Usuarios (CodigoAcceso, PasswordHash, Rol) VALUES (@CodigoAcceso, @PasswordHash, @Rol); SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodigoAcceso", codigoAcceso); 
                cmd.Parameters.AddWithValue("@PasswordHash", hash);
                cmd.Parameters.AddWithValue("@Rol", rol);

                conn.Open();
                newIdUsuario = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return newIdUsuario;
        }



        // consulta de usaurio
        public Usuarios ObtenerUsuarioPorCodigo(string codigoAcceso)
        {
            Usuarios usuario = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdUsuario, CodigoAcceso, PasswordHash, Rol FROM Usuarios WHERE CodigoAcceso = @Codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Codigo", codigoAcceso);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new Usuarios
                    {
                        IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                        CodigoAcceso = reader["CodigoAcceso"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Rol = reader["Rol"].ToString()
                    };
                }
            }
            return usuario;
        }
        //generador de usuarios
        public int AgregarUsuarioFijo(string codigoAcceso, string contraseña, string rol)
        {
            
            string hash = Encriptador.EncriptarClave(contraseña);
            int newIdUsuario = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                
                string query = "INSERT INTO Usuarios (CodigoAcceso, PasswordHash, Rol) VALUES (@CodigoAcceso, @PasswordHash, @Rol); SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);

                
                cmd.Parameters.AddWithValue("@CodigoAcceso", codigoAcceso);
                cmd.Parameters.AddWithValue("@PasswordHash", hash);
                cmd.Parameters.AddWithValue("@Rol", rol);

                conn.Open();

                // 3. Ejecución y retorno del nuevo ID
                newIdUsuario = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return newIdUsuario;
        }

       
        public void AgregarDocente(int idUsuario, string nombre, string apellido)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Docentes (Nombre, Apellido, IdUsuario) VALUES (@Nombre, @Apellido, @IdUsuario)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AgregarEstudiante(int idUsuario, string nombre, string apellido, int idNivel, int? idEspecializacion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Estudiantes (Nombre, Apellido, IdNivel, IdEspecializacion, IdUsuario) 
                    VALUES (@Nombre, @Apellido, @IdNivel, @IdEspecializacion, @IdUsuario)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@IdNivel", idNivel);
                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)idEspecializacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}