using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class ListaReproduccion
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        public bool EsPublica { get; set; } = false;

        // Dueño de la lista 1:1 con Usuario
        [Required]
        public Guid? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } = null!;

        public ICollection<CancionLista>? CancionesEnListas { get; set; }
            = new List<CancionLista>();
    }
}
