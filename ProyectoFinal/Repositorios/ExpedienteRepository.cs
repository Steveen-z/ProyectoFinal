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

        // Dentro de ExpedienteRepository.cs

        public DataTable ObtenerNotasPorEstudiante(int idUsuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            -- 1. Obtener el IdEstudiante
            DECLARE @IdEstudiante INT;
            SELECT @IdEstudiante = IdEstudiante  
            FROM Estudiantes  
            WHERE IdUsuario = @IdUsuario;

            -- 2. Definir la base de notas solo para el estudiante actual
            WITH NotasBase AS (
                SELECT 
                    N.IdAsignatura,
                    N.Periodo,
                    N.Calificacion
                FROM Notas N
                WHERE N.IdEstudiante = @IdEstudiante
            )
            
            -- 3. Consulta Principal con PIVOT y cálculo del promedio
            SELECT
                A.NombreAsignatura,
                -- **CORRECCIÓN:** Usar el alias 'P' para acceder a las columnas pivotadas
                ISNULL(P.P1, 0.0) AS 'P1',  
                ISNULL(P.P2, 0.0) AS 'P2',  
                ISNULL(P.P3, 0.0) AS 'P3',  
                ISNULL(P.P4, 0.0) AS 'P4',

                -- Calcula el Promedio Final
                (ISNULL(P.P1, 0.0) + ISNULL(P.P2, 0.0) + ISNULL(P.P3, 0.0) + ISNULL(P.P4, 0.0)) / 4.0 AS NotaFinal
            
            FROM Asignaturas A

            -- 4. Unir Asignaturas con el Nivel/Especialización del Estudiante
            INNER JOIN Estudiantes E 
                ON A.IdNivel = E.IdNivel 
                AND (A.IdEspecializacion = E.IdEspecializacion OR (A.IdEspecializacion IS NULL AND E.IdEspecializacion IS NULL))
                AND E.IdEstudiante = @IdEstudiante -- Asegurar que solo traiga materias del estudiante

            -- 5. LEFT JOIN con la tabla pivotada
            LEFT JOIN (
                SELECT IdAsignatura, P1, P2, P3, P4
                FROM NotasBase
                PIVOT (
                    MAX(Calificacion)
                    FOR Periodo IN ([P1], [P2], [P3], [P4])
                ) AS PVT
            ) P ON A.IdAsignatura = P.IdAsignatura
            
            ORDER BY A.NombreAsignatura;"; // <- El cierre de las comillas dobles y el punto y coma final de SQL

                SqlCommand cmd = new SqlCommand(query, conn);
                // El parámetro @IdUsuario se usa en el DECLARE SELECT inicial.
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}