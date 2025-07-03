using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos.DTOs.Cuenta
{
    public class LoginResponse
    {
        public Guid UsuarioId { get; set; }
        public string Correo { get; set; } = null!;
        public string Rol { get; set; } = null!; // Free, Premium, Artista, Admin
       
    }
}
