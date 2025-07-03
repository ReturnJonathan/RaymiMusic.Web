using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Planes
{
    public class IndexModel : PageModel
    {
        private readonly IPlanesApiService _svc;
        public IEnumerable<PlanSuscripcion> Planes { get; private set; } = new List<PlanSuscripcion>();

        public IndexModel(IPlanesApiService svc)
        {
            _svc = svc;
        }

        public async Task OnGetAsync()
        {
            Planes = await _svc.ObtenerTodosAsync();
        }
    }
}
