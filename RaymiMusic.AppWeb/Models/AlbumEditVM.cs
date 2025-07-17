using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.AppWeb.Models
{
    public class AlbumEditVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Titulo { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? FechaLanzamiento { get; set; }

        [Required]
        public Guid ArtistaId { get; set; }

        // Sólo para Admin
        public SelectList? Artistas { get; set; }
    }
}
