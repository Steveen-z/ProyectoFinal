using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Clases
{
    public static class Encriptador
    {
        public static string EncriptarClave(string clave)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(clave));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
