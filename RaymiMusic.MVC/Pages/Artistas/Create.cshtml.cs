using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Artistas
{
    public class CreateModel : PageModel
    {
        private readonly IArtistasApiService _svc;

        [BindProperty]
        public Artista Artista { get; set; } = new();

        public CreateModel(IArtistasApiService svc)
        {
            _svc = svc;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _svc.CrearAsync(Artista);
            return RedirectToPage("Index");
        }
    }
}
