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
            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            if (rol == "Admin")
            {
                Canciones = await _svc.ObtenerTodosAsync();
            }
            else if (rol == "Artista")
            {
                var todas = await _svc.ObtenerTodosAsync();
                Canciones = todas.Where(c => c.Artista.NombreArtistico == correo);
            }
            else
            {
                // Usuario o sin sesión
                Canciones = await _svc.ObtenerTodosAsync();
            }
        }

    }
}
