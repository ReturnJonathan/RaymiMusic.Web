using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.Modelos
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; } = null!;

        [Required]
        public string HashContrasena { get; set; } = null!;

        [Required]
        public string Rol { get; set; } = "Free";  // Free, Premium, Artista, Admin

        // FK al plan de suscripción
        [Required]
        public Guid PlanSuscripcionId { get; set; }

        // Navegación al plan
        public PlanSuscripcion PlanSuscripcion { get; set; } = null!;

        // Perfil 1:1
        public Perfil? Perfil { get; set; }

        // Listas de reproducción del usuario 1:N
        public ICollection<ListaReproduccion> ListasReproduccion { get; set; }
            = new List<ListaReproduccion>();
    }
}
