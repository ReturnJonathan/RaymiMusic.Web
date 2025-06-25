using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RaymiMusic.MVC.Pages.Canciones
{
    public class CreateModel : PageModel
    {
        private readonly ICancionesApiService _svc;
        private readonly IArtistasApiService _artSvc;
        private readonly IGenerosApiService _genSvc;

        public CreateModel(
            ICancionesApiService svc,
            IArtistasApiService artSvc,
            IGenerosApiService genSvc)
        {
            _svc = svc;
            _artSvc = artSvc;
            _genSvc = genSvc;
        }

        [BindProperty]
        public Cancion Cancion { get; set; } = new();

        public IEnumerable<Artista> Artistas { get; set; } = Array.Empty<Artista>();
        public IEnumerable<Genero> Generos { get; set; } = Array.Empty<Genero>();

        // Colección donde volcaremos los mensajes de error
        public List<string> Errores { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            Generos = await _genSvc.ObtenerTodosAsync();
            Artistas = await _artSvc.ObtenerTodosAsync(); // por si es admin

            if (!ModelState.IsValid)
            {
                Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Page();
            }

            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            if (rol == "Artista")
            {
                var artista = await _artSvc.ObtenerPorCorreoAsync(correo!);
                if (artista == null) return Unauthorized();

                Cancion.ArtistaId = artista.Id;
            }

            Cancion.Id = Guid.NewGuid();
            await _svc.CrearAsync(Cancion);
            return RedirectToPage("Index");
        }

    }
}
