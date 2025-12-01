using System.Data.SqlClient;
using System.Data;
using ProyectoFinal.Clases;
using System;

namespace ProyectoFinal.Repositorios
{
    
    public class DetalleEstudiante
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string TipoBachillerato { get; set; }
        public int Anio { get; set; }
        public string Especializacion { get; set; }
    }

    public class ExpedienteRepository
    {
        private readonly string _connectionString;

        public ExpedienteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DetalleEstudiante ObtenerDetalleEstudiante(int idUsuario)
        {
            DetalleEstudiante detalle = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        E.Nombre, E.Apellido, 
                        N.TipoBachillerato, N.Anio,
                        ES.NombreEspecializacion
                    FROM Estudiantes E
                    INNER JOIN NivelesEducativos N ON E.IdNivel = N.IdNivel
                    LEFT JOIN Especializaciones ES ON E.IdEspecializacion = ES.IdEspecializacion
                    WHERE E.IdUsuario = @IdUsuario";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    detalle = new DetalleEstudiante
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        TipoBachillerato = reader["TipoBachillerato"].ToString(),
                        Anio = Convert.ToInt32(reader["Anio"]),
                        Especializacion = reader.IsDBNull(reader.GetOrdinal("NombreEspecializacion")) ? null : reader["NombreEspecializacion"].ToString()
                    };
                }
            }
            return detalle;
        }


       // Dentro de ExpedienteRepository.cs

public DataTable ObtenerNotasPorEstudiante(int idUsuario)
{
    DataTable dt = new DataTable();
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        string query = @"
            DECLARE @IdEstudiante INT;
            SELECT @IdEstudiante = IdEstudiante FROM Estudiantes WHERE IdUsuario = @IdUsuario;

            WITH NotasBase AS (
                SELECT  
                    N.IdAsignatura,
                    N.Periodo,
                    N.Calificacion
                FROM Notas N
                WHERE N.IdEstudiante = @IdEstudiante
            )
            
            SELECT
                A.NombreAsignatura,
           
                ISNULL(P.[Periodo 1], 0.0) AS P1,  
                ISNULL(P.[Periodo 2], 0.0) AS P2,  
                ISNULL(P.[Periodo 3], 0.0) AS P3,  
                ISNULL(P.[Periodo 4], 0.0) AS P4,

              
                (ISNULL(P.[Periodo 1], 0.0) + ISNULL(P.[Periodo 2], 0.0) + ISNULL(P.[Periodo 3], 0.0) + ISNULL(P.[Periodo 4], 0.0)) / 4.0 AS NotaFinal
            
            FROM Asignaturas A

            
            INNER JOIN Estudiantes E  
                ON A.IdNivel = E.IdNivel  
                AND E.IdEstudiante = @IdEstudiante 
                AND (A.IdEspecializacion = E.IdEspecializacion OR (A.IdEspecializacion IS NULL AND E.IdEspecializacion IS NULL))

            LEFT JOIN (
                SELECT 
                    IdAsignatura, 
                    [Periodo 1], [Periodo 2], [Periodo 3], [Periodo 4]
                FROM NotasBase
                PIVOT (
                    MAX(Calificacion)
                    FOR Periodo IN ([Periodo 1], [Periodo 2], [Periodo 3], [Periodo 4]) 
                ) AS PVT
            ) P ON A.IdAsignatura = P.IdAsignatura
            
            ORDER BY A.NombreAsignatura;"; 

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
    }
    return dt;
}
    }
}