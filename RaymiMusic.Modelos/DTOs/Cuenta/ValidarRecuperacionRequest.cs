using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos.DTOs.Cuenta
{
    public class ValidarRecuperacionRequest
    {
        public string NuevaContraseña { get; set; }
        public string Codigo { get; set; }
    }
}
