using System;
using System.Data.SqlClient;
using ProyectoFinal.Clases;

namespace ProyectoFinal.Repositorios
{
    public class DocentesRepository
    {
        private readonly string _connectionString;

        public DocentesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AgregarDocente(Docentes docente)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Docentes (Nombre, Apellido, IdUsuario) 
                    VALUES (@Nombre, @Apellido, @IdUsuario)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", docente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", docente.Apellido);
                cmd.Parameters.AddWithValue("@IdUsuario", docente.IdUsuario);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Docentes ObtenerDocentePorIdUsuario(int idUsuario)
        {
            return null;
        }
    }
}