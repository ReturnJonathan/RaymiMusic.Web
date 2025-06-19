using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.Modelos
{
    public class Cancion
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Titulo { get; set; } = null!;

        [Required]
        public string RutaArchivo { get; set; } = null!;  // ruta o URL

        [Required]
        public TimeSpan Duracion { get; set; }

        // Relaciones por clave foránea
        [Required]
        public Guid GeneroId { get; set; }
        public Genero? Genero { get; set; }

        [Required]
        public Guid ArtistaId { get; set; }
        public Artista? Artista { get; set; }

        public Guid? AlbumId { get; set; }
        public Album? Album { get; set; }

        public ICollection<CancionLista> CancionesEnListas { get; set; }
            = new List<CancionLista>();
    }
}
