using System;
using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.AppWeb.Models
{
    public class CreatePlaylistVM
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; } = null!;

        public bool EsPublica { get; set; } = false;

        // Será rellenado por el servidor con el Id del usuario
        [Required]
        public Guid UsuarioId { get; set; }
    }
}
