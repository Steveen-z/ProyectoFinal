using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Clases
{
    public class NivelesEducativos
    {
        public int IdNivel { get; set; }
        public string TipoBachillerato { get; set; } 
        public int Anio { get; set; } 
        public string NombreCompleto => $"{TipoBachillerato} - {Anio}er Año";

        public NivelesEducativos() { }

        public NivelesEducativos(int idNivel, string tipoBachillerato, int anio)
        {
            IdNivel = idNivel;
            TipoBachillerato = tipoBachillerato;
            Anio = anio;
        }
    }
}
