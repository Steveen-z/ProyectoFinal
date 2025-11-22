using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Clases
{
    public class Especializaciones
    {
        public int IdEspecializacion { get; set; }
        public string NombreEspecializacion { get; set; }

        public Especializaciones() { }

        public Especializaciones(int idEspecializacion, string nombreEspecializacion)
        {
            IdEspecializacion = idEspecializacion;
            NombreEspecializacion = nombreEspecializacion;
        }
    }
}
