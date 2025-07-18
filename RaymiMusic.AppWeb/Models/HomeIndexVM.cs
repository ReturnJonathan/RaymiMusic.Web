using System.Collections.Generic;
using RaymiMusic.Modelos;  // o el DTO que uses

namespace RaymiMusic.AppWeb.Models
{
    public class HomeIndexVM
    {
        public string Query { get; set; }
        public IEnumerable<SongDTO> Songs { get; set; }
    }
}
