using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class ListaPublica
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        // Creada por Admin (puede ser correo o Id)
        [Required]
        public string CreadaPor { get; set; } = null!;

        public ICollection<CancionLista> CancionesEnListas { get; set; }
            = new List<CancionLista>();
    }

}
