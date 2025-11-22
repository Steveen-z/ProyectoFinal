using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProyectoFinal.Clases
{
    public class Usuarios
    {
        public int IdUsuario { get; set; }
        public string CodigoAcceso { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }

        public Usuarios() { }

       
        public Usuarios(int IdUsuario_, string CodigoAcceso_, string PasswordHash_, string Rol_)
        {
            this.IdUsuario = IdUsuario_;
            this.CodigoAcceso = CodigoAcceso_;
            this.PasswordHash = PasswordHash_;
            this.Rol = Rol_;
        }
    }
}