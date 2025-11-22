using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Clases
{
    public class Docentes
    {
        public int IdDocente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

       
        public int IdUsuario { get; set; }

        public Docentes() { }

        public Docentes(int idDocente, string nombre, string apellido, int idUsuario)
        {
            IdDocente = idDocente;
            Nombre = nombre;
            Apellido = apellido;
            IdUsuario = idUsuario;
        }
    }
}
