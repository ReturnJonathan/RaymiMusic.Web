using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Usuarios
{
    public class EditModel : PageModel
    {
        private readonly IUsuarioApiService _usuarioSvc;
        private readonly IPlanesApiService _planesSvc;

        public EditModel(IUsuarioApiService usuarioSvc,
                         IPlanesApiService planesSvc)
        {
            _usuarioSvc = usuarioSvc;
            _planesSvc = planesSvc;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = null!;

        public List<PlanSuscripcion> Planes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            // Cargar usuario
            var u = await _usuarioSvc.ObtenerPorIdAsync(id);
            if (u == null) return NotFound();
            Usuario = u;

            // Cargar planes para el select
            Planes = new List<PlanSuscripcion>(await _planesSvc.ObtenerTodosAsync());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Planes = new List<PlanSuscripcion>(await _planesSvc.ObtenerTodosAsync());
                return Page();
            }

            await _usuarioSvc.ActualizarAsync(Usuario.Id, Usuario);
            return RedirectToPage("Index");
        }
    }
}
