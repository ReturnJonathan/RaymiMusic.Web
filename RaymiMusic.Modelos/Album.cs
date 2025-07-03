using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Titulo { get; set; } = null!;

        public DateTime? FechaLanzamiento { get; set; }

        // Relaciones
        [Required]
        public Guid ArtistaId { get; set; }
        public Artista Artista { get; set; } = null!;

        public ICollection<Cancion> Canciones { get; set; }
            = new List<Cancion>();
    }

}
