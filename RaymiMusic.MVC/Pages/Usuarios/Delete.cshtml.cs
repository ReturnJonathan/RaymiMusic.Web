using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Usuarios
{
    public class DeleteModel : PageModel
    {
        private readonly IUsuarioApiService _usuarioSvc;

        public DeleteModel(IUsuarioApiService usuarioSvc)
        {
            _usuarioSvc = usuarioSvc;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var u = await _usuarioSvc.ObtenerPorIdAsync(id);
            if (u == null) return NotFound();
            Usuario = u;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _usuarioSvc.EliminarAsync(id);
            return RedirectToPage("Index");
        }
    }
}
