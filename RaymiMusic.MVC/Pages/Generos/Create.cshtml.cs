using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Generos
{
    public class CreateModel : PageModel
    {
        private readonly IGenerosApiService _svc;

        [BindProperty]
        public Genero Genero { get; set; } = new();

        public CreateModel(IGenerosApiService svc)
        {
            _svc = svc;
        }

        public IActionResult OnGet()
        {
            var rol = HttpContext.Session.GetString("Rol");

            if (rol != "Admin")
            {
                return RedirectToPage("/Cuenta/Login");
            }

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

            await _svc.CrearAsync(Genero);
            return RedirectToPage("Index");
        }
    }
}
