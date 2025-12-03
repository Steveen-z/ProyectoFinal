using System;

namespace ProyectoFinal.Clases
{
    public class Docentes
    {
        public int IdDocente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

       //Actualizacion de la DB
        public string DUI { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public int IdUsuario { get; set; }
        public string CodigoAcceso { get; set; } 

        public Docentes() { }

        public Docentes(int idDocente, string nombre, string apellido,
                        string dui, string email, string telefono,
                        int idUsuario, string codigoAcceso)
        {
            IdDocente = idDocente;
            Nombre = nombre;
            Apellido = apellido;
            DUI = dui;
            Email = email;
            Telefono = telefono;
            IdUsuario = idUsuario;
            CodigoAcceso = codigoAcceso;
        }
    }
}
