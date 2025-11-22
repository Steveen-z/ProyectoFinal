using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProyectoFinal.Clases;

namespace ProyectoFinal.Repositorios
{
    public class NotasRepository
    {
        private readonly string _connectionString;

        public NotasRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void RegistrarNota(Notas nota)
        {

        }


        public List<Notas> ObtenerNotasPorEstudiante(int idEstudiante)
        {

            return new List<Notas>();
        }
    }
}