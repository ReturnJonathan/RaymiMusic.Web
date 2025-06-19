using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Canciones
{
    public class EditModel : PageModel
    {
        private readonly ICancionesApiService _svc;
        private readonly IArtistasApiService _artSvc;
        private readonly IGenerosApiService _genSvc;

        public EditModel(
            ICancionesApiService svc,
            IArtistasApiService artSvc,
            IGenerosApiService genSvc)
        {
            _svc = svc;
            _artSvc = artSvc;
            _genSvc = genSvc;
        }

        [BindProperty]
        public Cancion Cancion { get; set; } = null!;

        public IEnumerable<Artista> Artistas { get; set; } = Array.Empty<Artista>();
        public IEnumerable<Genero> Generos { get; set; } = Array.Empty<Genero>();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var c = await _svc.ObtenerPorIdAsync(id);
            if (c == null) return NotFound();
            Cancion = c;
            Artistas = await _artSvc.ObtenerTodosAsync();
            Generos = await _genSvc.ObtenerTodosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Artistas = await _artSvc.ObtenerTodosAsync();
                Generos = await _genSvc.ObtenerTodosAsync();
                return Page();
            }

            await _svc.ActualizarAsync(Cancion.Id, Cancion);
            return RedirectToPage("Index");
        }
    }
}
