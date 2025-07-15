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

        public Cancion Cancion { get; private set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var c = await _svc.ObtenerPorIdAsync(id);
            if (c is null) return NotFound();

            Cancion = c;
            return Page();
        }
    }
}
