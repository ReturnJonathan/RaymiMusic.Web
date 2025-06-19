using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class CancionLista
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ListaReproduccionId { get; set; }
        public ListaReproduccion ListaReproduccion { get; set; } = null!;

        [Required]
        public Guid CancionId { get; set; }
        public Cancion Cancion { get; set; } = null!;
    }
}
