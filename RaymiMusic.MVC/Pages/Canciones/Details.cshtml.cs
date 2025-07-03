using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Canciones
{
    public class DetailsModel : PageModel
    {
        private readonly ICancionesApiService _svc;

        public DetailsModel(ICancionesApiService svc)
        {
            _svc = svc;
        }

        [BindProperty]
        public Cancion Cancion { get; set; } = null!;

        public string UrlStreaming { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var c = await _svc.ObtenerPorIdAsync(id);
            if (c == null) return NotFound();

            Cancion = c;
            UrlStreaming = _svc.ObtenerUrlStreaming(id);
            return Page();
        }
    }
}
