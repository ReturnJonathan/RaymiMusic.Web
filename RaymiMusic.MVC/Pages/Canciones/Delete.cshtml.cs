using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Canciones
{
    public class DeleteModel : PageModel
    {
        private readonly ICancionesApiService _svc;

        public DeleteModel(ICancionesApiService svc)
        {
            _svc = svc;
        }

        [BindProperty]
        public Cancion Cancion { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var c = await _svc.ObtenerPorIdAsync(id);
            if (c == null) return NotFound();

            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            bool esAdmin = rol == "Admin";
            bool esDueño = rol == "Artista" && c.Artista.NombreArtistico == correo;

            if (!esAdmin && !esDueño)
                return RedirectToPage("/Cuenta/Login");

            Cancion = c;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var c = await _svc.ObtenerPorIdAsync(id);
            if (c == null) return NotFound();

            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            bool esAdmin = rol == "Admin";
            bool esDueño = rol == "Artista" && c.Artista.NombreArtistico == correo;

            if (!esAdmin && !esDueño)
                return RedirectToPage("/Cuenta/Login");

            await _svc.EliminarAsync(id);
            return RedirectToPage("Index");
        }

    }
}
