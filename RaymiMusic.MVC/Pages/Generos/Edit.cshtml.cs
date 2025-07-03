using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Generos
{
    public class EditModel : PageModel
    {
        private readonly IGenerosApiService _svc;

        [BindProperty]
        public Genero Genero { get; set; } = null!;

        public EditModel(IGenerosApiService svc)
        {
            _svc = svc;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var rol = HttpContext.Session.GetString("Rol");

            if (rol != "Admin")
            {
                return RedirectToPage("/Cuenta/Login");
            }

            var g = await _svc.ObtenerPorIdAsync(id);
            if (g == null) return NotFound();

            Genero = g;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var rol = HttpContext.Session.GetString("Rol");

            if (rol != "Admin")
            {
                return RedirectToPage("/Cuenta/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _svc.ActualizarAsync(Genero.Id, Genero);
            return RedirectToPage("Index");
        }
    }
}
