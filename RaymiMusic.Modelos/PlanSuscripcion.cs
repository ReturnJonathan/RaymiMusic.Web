using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class PlanSuscripcion
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;   // Free, Premium

        [Column(TypeName = "decimal(8,2)")]     
        public decimal Precio { get; set; }

        public int DescargasMaximas { get; set; }

        // Navegación inversa: todos los usuarios con este plan
        public ICollection<Usuario> Usuarios { get; set; }
            = new List<Usuario>();
    }


}
