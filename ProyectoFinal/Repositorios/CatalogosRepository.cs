using ProyectoFinal.Clases;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data; 

namespace ProyectoFinal.Repositorios
{
    public class CatalogosRepository
    {
        private readonly string _connectionString;

        public CatalogosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CONSULTA DE CATÁLOGOS

        public List<NivelesEducativos> ObtenerNiveles()
        {
            List<NivelesEducativos> lista = new List<NivelesEducativos>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdNivel, TipoBachillerato, Anio FROM NivelesEducativos ORDER BY TipoBachillerato DESC, Anio ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new NivelesEducativos
                    {
                        IdNivel = Convert.ToInt32(reader["IdNivel"]),
                        TipoBachillerato = reader["TipoBachillerato"].ToString(),
                        Anio = Convert.ToInt32(reader["Anio"])
                    });
                }
            }
            return lista;
        }

        public List<Especializaciones> ObtenerEspecializaciones()
        {
            List<Especializaciones> lista = new List<Especializaciones>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdEspecializacion, NombreEspecializacion FROM Especializaciones ORDER BY NombreEspecializacion ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Especializaciones
                    {
                        IdEspecializacion = Convert.ToInt32(reader["IdEspecializacion"]),
                        NombreEspecializacion = reader["NombreEspecializacion"].ToString()
                    });
                }
            }
            return lista;
        }

        public List<Asignaturas> ObtenerAsignaturasPorNivel(int idNivel, int? idEspecializacion)
        {
            List<Asignaturas> lista = new List<Asignaturas>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdAsignatura, NombreAsignatura, IdNivel, IdEspecializacion FROM Asignaturas WHERE IdNivel = @IdNivel";

                if (idEspecializacion.HasValue)
                {
                    query += " AND IdEspecializacion = @IdEspecializacion";
                }
                else
                {
                    query += " AND IdEspecializacion IS NULL";
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
                    lista.Add(new Asignaturas
                    {
                        IdAsignatura = Convert.ToInt32(reader["IdAsignatura"]),
                        NombreAsignatura = reader["NombreAsignatura"].ToString(),
                        IdNivel = Convert.ToInt32(reader["IdNivel"]),
                        IdEspecializacion = reader.IsDBNull(reader.GetOrdinal("IdEspecializacion")) ? (int?)null : Convert.ToInt32(reader["IdEspecializacion"])
                    });
                }
            }
            return lista;
        }

        // INICIALIZACIÓN DE DATOS 

        private int? ObtenerIdNivel(string tipoBachillerato, int anio)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdNivel FROM NivelesEducativos WHERE TipoBachillerato = @Tipo AND Anio = @Anio";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Tipo", tipoBachillerato);
                cmd.Parameters.AddWithValue("@Anio", anio);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : (int?)null;
            }
        }

        private int? ObtenerIdEspecializacion(string nombreEspecializacion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdEspecializacion FROM Especializaciones WHERE NombreEspecializacion = @Nombre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombreEspecializacion);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : (int?)null;
            }
        }

        public int InicializarNiveles()
        {
            var nivelesAInsertar = new List<(string tipo, int anio)>
            {
                ("General", 1),
                ("General", 2),
                ("Técnico", 1),
                ("Técnico", 2),
                ("Técnico", 3)
            };

            int idNivelBase = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                foreach (var nivel in nivelesAInsertar)
                {
                    string checkQuery = "SELECT IdNivel FROM NivelesEducativos WHERE TipoBachillerato = @Tipo AND Anio = @Anio";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Tipo", nivel.tipo);
                    checkCmd.Parameters.AddWithValue("@Anio", nivel.anio);

                    object existingId = checkCmd.ExecuteScalar();

                    if (existingId != null)
                    {
                        int currentId = Convert.ToInt32(existingId);
                        if (nivel.tipo == "General" && nivel.anio == 1)
                        {
                            idNivelBase = currentId;
                        }
                        continue;
                    }

                    string insertQuery = "INSERT INTO NivelesEducativos (TipoBachillerato, Anio) VALUES (@Tipo, @Anio); SELECT SCOPE_IDENTITY();";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Tipo", nivel.tipo);
                    insertCmd.Parameters.AddWithValue("@Anio", nivel.anio);

                    int newId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    if (nivel.tipo == "General" && nivel.anio == 1)
                    {
                        idNivelBase = newId;
                    }
                }
            }
            return idNivelBase;
        }

        public void InicializarEspecializaciones()
        {
            var especializaciones = new List<string> {
                "Desarrollo de Software",
                "Contaduría y Finanzas",
                "Enfermería"
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                foreach (var nombreEspecializacion in especializaciones)
                {
                    string checkQuery = "SELECT COUNT(1) FROM Especializaciones WHERE NombreEspecializacion = @Nombre";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Nombre", nombreEspecializacion);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        string insertQuery = "INSERT INTO Especializaciones (NombreEspecializacion) VALUES (@Nombre)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@Nombre", nombreEspecializacion);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }



        public void InicializarAsignaturas()
        {
            int? idGen1 = ObtenerIdNivel("General", 1);
            int? idGen2 = ObtenerIdNivel("General", 2);
            int? idTec1 = ObtenerIdNivel("Técnico", 1);
            int? idTec2 = ObtenerIdNivel("Técnico", 2);
            int? idTec3 = ObtenerIdNivel("Técnico", 3);

            int? idSW = ObtenerIdEspecializacion("Desarrollo de Software");
            int? idCF = ObtenerIdEspecializacion("Contaduría y Finanzas");
            int? idEnf = ObtenerIdEspecializacion("Enfermería");

            var asignaturasAInsertar = new List<(string nombre, int? idNivel, int? idEspecializacion)>
            {
                // Asignaturas Generales
                ("Lenguaje y Lit. I", idGen1, null), ("Matemática I", idGen1, null),
                ("Ciencias Nat. I", idGen1, null), ("Estudios Sociales I", idGen1, null),
                ("Lenguaje y Lit. II", idGen2, null), ("Matemática II", idGen2, null),
                ("Ciencias Nat. II", idGen2, null), ("Estudios Sociales II", idGen2, null),

                // Asignaturas Desarrollo de Software
                ("Fundamentos de TI", idTec1, idSW), ("Lógica de Programación", idTec2, idSW),
                ("Base de Datos Avanzada", idTec3, idSW),
                
                // Asignaturas Contaduría y Finanzas
                ("Contabilidad Básica", idTec1, idCF), ("Auditoría Financiera", idTec2, idCF),
                ("Leyes Tributarias", idTec3, idCF),

                // Asignaturas Enfermería
                ("Introducción a la Salud", idTec1, idEnf), ("Primeros Auxilios", idTec2, idEnf),
                ("Farmacología Básica", idTec3, idEnf),
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                foreach (var asignatura in asignaturasAInsertar)
                {
                    if (asignatura.idNivel.HasValue)
                    {
                        string checkQuery = "SELECT COUNT(1) FROM Asignaturas WHERE NombreAsignatura = @Nombre AND IdNivel = @IdNivel AND IdEspecializacion " +
                                            (asignatura.idEspecializacion.HasValue ? "= @IdEspecializacion" : "IS NULL");

                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@Nombre", asignatura.nombre);
                        checkCmd.Parameters.AddWithValue("@IdNivel", asignatura.idNivel.Value);

                        if (asignatura.idEspecializacion.HasValue)
                        {
                            checkCmd.Parameters.AddWithValue("@IdEspecializacion", asignatura.idEspecializacion.Value);
                        }

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertQuery = "INSERT INTO Asignaturas (NombreAsignatura, IdNivel, IdEspecializacion) VALUES (@Nombre, @IdNivel, @IdEspecializacion)";
                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@Nombre", asignatura.nombre);
                            insertCmd.Parameters.AddWithValue("@IdNivel", asignatura.idNivel.Value);
                            insertCmd.Parameters.AddWithValue("@IdEspecializacion", asignatura.idEspecializacion.HasValue ? (object)asignatura.idEspecializacion.Value : DBNull.Value);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        // Modificar Especialización
        public bool ModificarEspecializacion(int id, string nuevoNombre)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Especializaciones SET NombreEspecializacion = @NuevoNombre WHERE IdEspecializacion = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NuevoNombre", nuevoNombre);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        
        // Eliminar Especialización
        public bool EliminarEspecializacion(int id)
        {

            // Detalle a completaaaaaaaaaaaaaaaaaaaaaar
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Especializaciones WHERE IdEspecializacion = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //Crear Especialización
        public bool AgregarEspecializacion(string nombre)
        {
            // Verifica si la especialización ya existe para evitar duplicados
            if (ObtenerIdEspecializacion(nombre) != null)
            {
                return false; 
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // La consulta de insercin
                string query = "INSERT INTO Especializaciones (NombreEspecializacion) VALUES (@Nombre)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // Método para cargar Niveles filtrados para el ComboBox 


        public DataTable ObtenerNivelesPorEspecializacion(int? idEspecializacion)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                IdNivel, 
                TipoBachillerato + ' ' + CAST(Anio AS VARCHAR) + '° Año' AS NombreCompleto
            FROM NivelesEducativos 
            ORDER BY TipoBachillerato DESC, Anio ASC"; 

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


        // Método para cargar Asignaturas filtradas
        public DataTable ObtenerAsignaturas(int? idNivel, int? idEspecializacion)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        A.IdAsignatura, 
                        A.NombreAsignatura, 
                        N.TipoBachillerato + ' ' + CAST(N.Anio AS VARCHAR) AS Nivel,
                        ISNULL(E.NombreEspecializacion, 'General') AS Especializacion
                    FROM Asignaturas A
                    INNER JOIN NivelesEducativos N ON A.IdNivel = N.IdNivel
                    LEFT JOIN Especializaciones E ON A.IdEspecializacion = E.IdEspecializacion
                    WHERE 
                        (@IdNivel IS NULL OR A.IdNivel = @IdNivel) AND
                        (@IdEspecializacion IS NULL OR A.IdEspecializacion = @IdEspecializacion)
                    ORDER BY N.Anio, A.NombreAsignatura";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdNivel", (object)idNivel ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)idEspecializacion ?? DBNull.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        // Método para obtener una Asignatura por Id
        public Asignaturas ObtenerAsignaturaPorId(int idAsignatura)
        {
            Asignaturas asignatura = null;
            string query = "SELECT IdAsignatura, NombreAsignatura, IdNivel, IdEspecializacion FROM Asignaturas WHERE IdAsignatura = @IdAsignatura";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdAsignatura", idAsignatura);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    asignatura = new Asignaturas
                    {
                        IdAsignatura = Convert.ToInt32(reader["IdAsignatura"]),
                        NombreAsignatura = reader["NombreAsignatura"].ToString(),
                        IdNivel = Convert.ToInt32(reader["IdNivel"]),
                        IdEspecializacion = reader.IsDBNull(reader.GetOrdinal("IdEspecializacion")) ? (int?)null : Convert.ToInt32(reader["IdEspecializacion"])
                    };
                }
            }
            return asignatura;
        }


        public bool AsignaturaEstaEnUso(int idAsignatura)
        {
            bool estaEnUso = false;

            string nombreTablaNotas = "NombreCorrectoDeTuTabla";

            string query = $@"
        SELECT 
            COUNT(IdRegistro) 
        FROM {nombreTablaNotas} 
        WHERE IdAsignatura = @IdAsignatura";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IdAsignatura", idAsignatura);
                    conn.Open();

                    // 2. Ejecuta la consulta y asigna el resultado a la variable.
                    int count = (int)cmd.ExecuteScalar();
                    estaEnUso = count > 0;
                }
            }
            catch (Exception ex)
            {
                
            }

            return estaEnUso;
        }

        // Mtodo para Agregar una nueva Asignatura 
        public bool AgregarAsignatura(string nombre, int idNivel, int? idEspecializacion)
        {
            string query = "INSERT INTO Asignaturas (NombreAsignatura, IdNivel, IdEspecializacion) VALUES (@Nombre, @IdNivel, @IdEspecializacion)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@IdNivel", idNivel);
                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)idEspecializacion ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Metodo para Modificar una Asignatura
        public bool ModificarAsignatura(int idAsignatura, string nombre, int idNivel, int? idEspecializacion)
        {
            string query = "UPDATE Asignaturas SET NombreAsignatura = @Nombre, IdNivel = @IdNivel, IdEspecializacion = @IdEspecializacion WHERE IdAsignatura = @IdAsignatura";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@IdNivel", idNivel);
                cmd.Parameters.AddWithValue("@IdAsignatura", idAsignatura);
                cmd.Parameters.AddWithValue("@IdEspecializacion", (object)idEspecializacion ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Método para Eliminar una Asignatura 
        public bool EliminarAsignatura(int idAsignatura)
        {
            string query = "DELETE FROM Asignaturas WHERE IdAsignatura = @IdAsignatura";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdAsignatura", idAsignatura);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public DataTable ObtenerEspecializacionesDataTable()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdEspecializacion, NombreEspecializacion FROM Especializaciones ORDER BY NombreEspecializacion ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

       
        public DataTable ObtenerNivelesDataTable()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                
                string query = "SELECT IdNivel, TipoBachillerato + ' ' + CAST(Anio AS VARCHAR) + '° Año' AS NombreCompleto FROM NivelesEducativos ORDER BY TipoBachillerato DESC, Anio ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}