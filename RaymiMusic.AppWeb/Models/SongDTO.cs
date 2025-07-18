using System;

namespace RaymiMusic.AppWeb.Models
{
    public class SongDTO
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string AlbumNombre { get; set; } = "—";
        public string ArtistaNombre { get; set; } = null!;
        public TimeSpan Duracion { get; set; }
        public string RutaArchivo { get; set; } = null!;
    }
}
