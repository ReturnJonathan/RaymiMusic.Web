using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioApiService _svc;
        public IEnumerable<Usuario> Usuarios { get; private set; } = new List<Usuario>();

        public IndexModel(IUsuarioApiService svc)
        {
            _svc = svc;
        }

        public async Task OnGetAsync()
        {
            Usuarios = await _svc.ObtenerTodosAsync();
        }
    }
}
