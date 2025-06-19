using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Artista
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string NombreArtistico { get; set; } = null!;

        public string? Biografia { get; set; }
        public string? UrlFotoPerfil { get; set; }
        public string? UrlFotoPortada { get; set; }

        // Canciones del artista 1:N
        public ICollection<Cancion> Canciones { get; set; }
            = new List<Cancion>();

        // Álbumes del artista 1:N
        public ICollection<Album> Albumes { get; set; }
            = new List<Album>();
    }

}
