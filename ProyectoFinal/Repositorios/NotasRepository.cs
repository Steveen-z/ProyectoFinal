using System.Data;
using System.Data.SqlClient;
using System;

namespace ProyectoFinal.Repositorios
{
    public class NotasRepository
    {
        private readonly string _connectionString;

        public NotasRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public DataTable ObtenerAlumnosYNotasPorAsignatura(int idAsignatura)
        {
            DataTable dt = new DataTable();

            string periodo1 = "Periodo 1";
            string periodo2 = "Periodo 2";
            string periodo3 = "Periodo 3";
            string periodo4 = "Periodo 4"; 

            string query = $@"
        WITH Criterios AS (
            SELECT IdNivel, IdEspecializacion
            FROM Asignaturas 
            WHERE IdAsignatura = @IdAsignatura
        )
        SELECT
            E.IdEstudiante AS IdAlumno, 
            E.Nombre + ' ' + E.Apellido AS NombreCompleto,
            E.IdNivel,
            E.IdEspecializacion,
            ISNULL(MAX(CASE WHEN N.Periodo = @P1 THEN N.Calificacion END), 0.0) AS NotaP1, 
            ISNULL(MAX(CASE WHEN N.Periodo = @P2 THEN N.Calificacion END), 0.0) AS NotaP2,
            ISNULL(MAX(CASE WHEN N.Periodo = @P3 THEN N.Calificacion END), 0.0) AS NotaP3,
            ISNULL(MAX(CASE WHEN N.Periodo = @P4 THEN N.Calificacion END), 0.0) AS NotaP4
        FROM 
            Estudiantes E
        INNER JOIN 
            Criterios C ON C.IdNivel = E.IdNivel 
            AND (C.IdEspecializacion = E.IdEspecializacion 
                OR (C.IdEspecializacion IS NULL AND E.IdEspecializacion IS NULL))
        LEFT JOIN 
            Notas N ON E.IdEstudiante = N.IdEstudiante AND N.IdAsignatura = @IdAsignatura
        GROUP BY
            E.IdEstudiante, E.Nombre, E.Apellido, E.IdNivel, E.IdEspecializacion
        ORDER BY 
            NombreCompleto;
    ";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@IdAsignatura", idAsignatura);
                cmd.Parameters.AddWithValue("@P1", periodo1);
                cmd.Parameters.AddWithValue("@P2", periodo2);
                cmd.Parameters.AddWithValue("@P3", periodo3);
                cmd.Parameters.AddWithValue("@P4", periodo4);

                da.Fill(dt);
            }
            return dt;
        }

        public bool GuardarNota(int idEstudiante, int idAsignatura, string periodoNombre, decimal nota)
        {
            string query = @"
        IF EXISTS (
            SELECT 1 
            FROM Notas 
            WHERE IdEstudiante = @IdEstudiante AND IdAsignatura = @IdAsignatura AND Periodo = @Periodo
        )
        BEGIN
            UPDATE Notas 
            SET Calificacion = @Nota, 
                FechaRegistro = GETDATE()
            WHERE IdEstudiante = @IdEstudiante AND IdAsignatura = @IdAsignatura AND Periodo = @Periodo
            SELECT 1 
        END
        ELSE
        BEGIN
            INSERT INTO Notas (IdEstudiante, IdAsignatura, Periodo, Calificacion)
            VALUES (@IdEstudiante, @IdAsignatura, @Periodo, @Nota)
            SELECT 1 
        END";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Parámetros
                cmd.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                cmd.Parameters.AddWithValue("@IdAsignatura", idAsignatura);
                cmd.Parameters.AddWithValue("@Periodo", periodoNombre);
                cmd.Parameters.AddWithValue("@Nota", nota);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected >= 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                
            }
        }
    }
}