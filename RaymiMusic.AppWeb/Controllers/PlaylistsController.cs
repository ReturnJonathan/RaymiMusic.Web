using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaymiMusic.AppWeb.Models;
using RaymiMusic.AppWeb.Services;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistService _plService;
        public PlaylistsController(IPlaylistService plService) =>
            _plService = plService;

        // GET: /Playlists
        public async Task<IActionResult> Index()
        {
            var listas = await _plService.GetAllAsync();
            return View(listas);
        }

        // GET: /Playlists/Create
        public IActionResult Create()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var vm = new CreatePlaylistVM
            {
                UsuarioId = userId
            };
            return View(vm);
        }

        // POST: /Playlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePlaylistVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _plService.CreateAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Playlists/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var vm = await _plService.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        // GET: /Playlists/AddSong/3e...-cancionId
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddSong(Guid songId)
        {
            var listas = await _plService.GetAllAsync();
            var vm = new AddSongVM
            {
                SongId = songId,
                Playlists = listas
            };
            return View(vm);
        }

        // POST: /Playlists/AddSong
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddSong(AddSongVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _plService.AddSongAsync(vm.PlaylistId, vm.SongId);

            // Opcional: volver al reproductor o al detalle de la playlist
            return RedirectToAction("Play", "Player", new { id = vm.SongId });
        }

    }
}
