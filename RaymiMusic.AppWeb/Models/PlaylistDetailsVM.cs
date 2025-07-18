using System;
using System.Collections.Generic;

namespace RaymiMusic.AppWeb.Models
{
    public class PlaylistDetailsVM
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool EsPublica { get; set; }
        public IEnumerable<SongDTO> Canciones { get; set; }
            = new List<SongDTO>();
    }
}
