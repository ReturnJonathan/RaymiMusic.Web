using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaymiMusic.AppWeb.Models;
using RaymiMusic.AppWeb.Services;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden ver el Home
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISongService _songService;

        public HomeController(
            ILogger<HomeController> logger,
            ISongService songService)
        {
            _logger = logger;
            _songService = songService;
        }

        // GET: /Home/Index?q=texto
        public async Task<IActionResult> Index(string q)
        {
            // Si q está vacío, traemos todas; si no, filtramos
            var lista = string.IsNullOrWhiteSpace(q)
                ? await _songService.GetAllAsync()
                : await _songService.SearchAsync(q);

            var vm = new HomeIndexVM
            {
                Query = q,
                Songs = lista
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                            ?? HttpContext.TraceIdentifier
            });
        }
    }
}
