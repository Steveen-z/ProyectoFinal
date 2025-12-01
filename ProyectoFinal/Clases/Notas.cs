using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Clases
{
    public class Notas
    {
        public int IdNota { get; set; }

        public int IdEstudiante { get; set; }
        public int IdAsignatura { get; set; }

        public string Periodo { get; set; } 
        public decimal Calificacion { get; set; }
        public DateTime FechaRegistro { get; set; }

        
        public string NombreAsignatura { get; set; }

        public Notas() { }

        public Notas(int idNota, int idEstudiante, int idAsignatura, string periodo, decimal calificacion, DateTime fechaRegistro)
        {
            IdNota = idNota;
            IdEstudiante = idEstudiante;
            IdAsignatura = idAsignatura;
            Periodo = periodo;
            Calificacion = calificacion;
            FechaRegistro = fechaRegistro;
        }
    }
}
