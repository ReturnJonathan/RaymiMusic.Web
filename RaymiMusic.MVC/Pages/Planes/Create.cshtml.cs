using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Planes
{
    public class CreateModel : PageModel
    {
        private readonly IPlanesApiService _svc;

        [BindProperty]
        public PlanSuscripcion Plan { get; set; } = new();

        public CreateModel(IPlanesApiService svc)
        {
            _svc = svc;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _svc.CrearAsync(Plan);   // necesitarás exponer CrearAsync en IPlanesApiService
            return RedirectToPage("Index");
        }
    }
}
