using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Canciones
{
    public class IndexModel : PageModel
    {
        private readonly ICancionesApiService _svc;
        public IEnumerable<Cancion> Canciones { get; private set; } = new List<Cancion>();

        public IndexModel(ICancionesApiService svc)
        {
            _svc = svc;
        }

        public async Task OnGetAsync()
        {
            Canciones = await _svc.ObtenerTodosAsync();
        }
    }
}
