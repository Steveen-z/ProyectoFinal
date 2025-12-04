using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProyectoFinal.Clases; 

namespace ProyectoFinal.Repositorios
{
    public class EstudiantesRepository
    {
        private readonly string _connectionString;

        public EstudiantesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AgregarEstudiante(Estudiantes estudiante)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Estudiantes (Nombre, Apellido, IdNivel, IdEspecializacion, IdUsuario) 
                    VALUES (@Nombre, @Apellido, @IdNivel, @IdEspecializacion, @IdUsuario)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                cmd.Parameters.AddWithValue("@IdNivel", estudiante.IdNivel);

                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)estudiante.IdEspecializacion ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@IdUsuario", estudiante.IdUsuario);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //  Obtener lista de Estudiantes filtrados por Nivel para el Docente

        public List<Estudiantes> ObtenerEstudiantesPorNivel(int idNivel, int? idEspecializacion)
        {
            
            List<Estudiantes> lista = new List<Estudiantes>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        E.IdEstudiante, E.Nombre, E.Apellido, E.IdNivel, E.IdEspecializacion, E.IdUsuario,
                        U.CodigoAcceso
                    FROM Estudiantes E
                    INNER JOIN Usuarios U ON E.IdUsuario = U.IdUsuario
                    WHERE E.IdNivel = @IdNivel";

                // logic condicional para el filtro de especialización
                if (idEspecializacion.HasValue)
                {
                    query += " AND E.IdEspecializacion = @IdEspecializacion";
                }
                else
                {
                    query += " AND E.IdEspecializacion IS NULL";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdNivel", idNivel);

                if (idEspecializacion.HasValue)
                {
                    cmd.Parameters.AddWithValue("@IdEspecializacion", idEspecializacion.Value);
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Estudiantes
                    {
                        IdEstudiante = Convert.ToInt32(reader["IdEstudiante"]),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        IdNivel = Convert.ToInt32(reader["IdNivel"]),

                        IdEspecializacion = reader.IsDBNull(reader.GetOrdinal("IdEspecializacion")) ? (int?)null : Convert.ToInt32(reader["IdEspecializacion"]),

                        IdUsuario = Convert.ToInt32(reader["IdUsuario"]),

                        
                    });
                }
            }
            return lista;
        }
    }
}