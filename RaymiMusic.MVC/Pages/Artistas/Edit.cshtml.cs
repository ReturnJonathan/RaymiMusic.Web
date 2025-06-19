using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Artistas
{
    public class EditModel : PageModel
    {
        private readonly IArtistasApiService _svc;

        [BindProperty]
        public Artista Artista { get; set; } = null!;

        public EditModel(IArtistasApiService svc)
        {
            _svc = svc;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var a = await _svc.ObtenerPorIdAsync(id);
            if (a == null) return NotFound();
            Artista = a;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _svc.ActualizarAsync(Artista.Id, Artista);
            return RedirectToPage("Index");
        }
    }
}
