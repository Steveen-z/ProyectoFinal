using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Clases
{
    public class Asignaturas
    {
        public int IdAsignatura { get; set; }
        public string NombreAsignatura { get; set; }

        public int IdNivel { get; set; }
        public int? IdEspecializacion { get; set; } 

        public Asignaturas() { }

        public Asignaturas(int idAsignatura, string nombreAsignatura, int idNivel, int? idEspecializacion)
        {
            IdAsignatura = idAsignatura;
            NombreAsignatura = nombreAsignatura;
            IdNivel = idNivel;
            IdEspecializacion = idEspecializacion;
        }
    }
}
