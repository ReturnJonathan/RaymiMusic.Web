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
            var cancion = await _svc.ObtenerPorIdAsync(id);
            if (cancion == null) return NotFound();

            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            if (rol == "Artista" && cancion.Artista.NombreArtistico != correo)
            {
                return RedirectToPage("/Cuenta/Login");
            }

            Cancion = cancion;
            Artistas = await _artSvc.ObtenerTodosAsync();
            Generos = await _genSvc.ObtenerTodosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            if (rol == "Artista")
            {
                var original = await _svc.ObtenerPorIdAsync(Cancion.Id);
                if (original == null || original.Artista.NombreArtistico != correo)
                {
                    return RedirectToPage("/Cuenta/Login");
                }

                Cancion.ArtistaId = original.ArtistaId; // proteger campo ArtistaId
            }

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
