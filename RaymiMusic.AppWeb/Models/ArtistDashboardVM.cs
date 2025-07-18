using System.Collections.Generic;

namespace RaymiMusic.AppWeb.Models
{
    public class ArtistDashboardVM
    {
        public string NombreArtista { get; set; } = null!;
        public int TotalCanciones { get; set; }
        public int TotalAlbumes { get; set; }
        public int TotalPlaylists { get; set; }
        public IEnumerable<SongDTO> UltimasCanciones { get; set; }
    }
}
