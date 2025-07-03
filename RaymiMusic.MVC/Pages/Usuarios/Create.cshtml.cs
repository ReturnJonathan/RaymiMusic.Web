// Pages/Usuarios/Create.cshtml.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Usuarios
{
    public class CreateModel : PageModel
    {
        private readonly IUsuarioApiService _usuarioSvc;
        private readonly IPlanesApiService _planesSvc;

        public CreateModel(IUsuarioApiService usuarioSvc,
                           IPlanesApiService planesSvc)
        {
            _usuarioSvc = usuarioSvc;
            _planesSvc = planesSvc;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = new();

        // Aquí expongo la lista de planes al view
        public List<PlanSuscripcion> Planes { get; set; } = new();

        // Carga la lista de planes antes de renderizar la página
        public async Task OnGetAsync()
        {
            var lista = await _planesSvc.ObtenerTodosAsync();
            Planes = new List<PlanSuscripcion>(lista);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // recarga los planes para volver a mostrar el select en caso de error
                await OnGetAsync();
                return Page();
            }

            await _usuarioSvc.CrearAsync(Usuario);
            return RedirectToPage("Index");
        }
    }
}
