using System;

namespace RaymiMusic.AppWeb.Models
{
    public class PlaylistDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = "";
        public int CancionesCount { get; set; }
    }
}
