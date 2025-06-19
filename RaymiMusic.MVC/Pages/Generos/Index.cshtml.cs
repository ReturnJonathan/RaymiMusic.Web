using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Generos
{
    public class IndexModel : PageModel
    {
        private readonly IGenerosApiService _svc;
        public IEnumerable<Genero> Generos { get; private set; } = new List<Genero>();

        public IndexModel(IGenerosApiService svc)
        {
            _svc = svc;
        }

        public async Task OnGetAsync()
        {
            Generos = await _svc.ObtenerTodosAsync();
        }
    }
}
