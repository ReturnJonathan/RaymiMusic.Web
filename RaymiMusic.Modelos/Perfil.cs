using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Perfil
    {
        [Key, ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }

        public string? NombreCompleto { get; set; }
        public string? UrlFoto { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }

}
