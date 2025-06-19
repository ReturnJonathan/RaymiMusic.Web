using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Artistas
{
    public class IndexModel : PageModel
    {
        private readonly IArtistasApiService _svc;
        public IEnumerable<Artista> Artistas { get; private set; } = new List<Artista>();

        public IndexModel(IArtistasApiService svc)
        {
            _svc = svc;
        }

        public async Task OnGetAsync()
        {
            Artistas = await _svc.ObtenerTodosAsync();
        }
    }
}
