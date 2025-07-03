using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Recuperaciones
    {
        [Key, ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }
        public string Codigo { get; set; }
        public DateTime Expiracion { get; set; }

        public Usuario? Usuario { get; set; }

    }
}
