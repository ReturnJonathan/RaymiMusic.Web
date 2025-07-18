using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaymiMusic.AppWeb.Services;
using RaymiMusic.AppWeb.Models;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly ISongService _songService;

        public PlayerController(ISongService songService)
        {
            _songService = songService;
        }

        // GET: /Player/Play/{id}
        public async Task<IActionResult> Play(Guid id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();

            return View(song);
        }
    }
}
