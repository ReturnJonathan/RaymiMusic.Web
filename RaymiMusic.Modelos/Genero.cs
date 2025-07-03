using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaymiMusic.Modelos
{
    public class Genero
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        public ICollection<Cancion> Canciones { get; set; }
            = new List<Cancion>();
    }

}
