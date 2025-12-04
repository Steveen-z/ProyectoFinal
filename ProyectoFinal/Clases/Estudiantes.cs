using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Clases
{
    public class Estudiantes
    {
        
        public int IdEstudiante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        
        public int IdNivel { get; set; }
        
        public int? IdEspecializacion { get; set; }
        public int IdUsuario { get; set; }

        public string CodigoAcceso { get; set; } // borrar si se quiebra el codigo


        public Estudiantes() { }

        
        public Estudiantes(int idEstudiante, string nombre, string apellido, int idNivel, int? idEspecializacion, int idUsuario)
        {
            IdEstudiante = idEstudiante;
            Nombre = nombre;
            Apellido = apellido;
            IdNivel = idNivel;
            IdEspecializacion = idEspecializacion;
            IdUsuario = idUsuario;
        }
    }
}
