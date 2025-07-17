using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;
using RaymiMusic.AppWeb.Models;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize]
    public class SongsController : Controller
    {
        private readonly AppDbContext _ctx;
        private readonly IWebHostEnvironment _env;

        public SongsController(AppDbContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsAdmin => User.IsInRole(Roles.Admin);
        private bool IsArtist => User.IsInRole(Roles.Artista);

        // GET: /Songs
        public async Task<IActionResult> Index()
        {
            var query = _ctx.Canciones
                .Include(c => c.Artista)
                .Include(c => c.Genero)
                .Include(c => c.Album)
                .AsQueryable();

            if (IsArtist && !IsAdmin)
                query = query.Where(c => c.ArtistaId == CurrentUserId);

            var list = await query.AsNoTracking().ToListAsync();
            return View(list);
        }

        // GET: /Songs/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var c = await _ctx.Canciones
                .Include(c => c.Artista)
                .Include(c => c.Genero)
                .Include(c => c.Album)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (c == null) return NotFound();
            if (IsArtist && c.ArtistaId != CurrentUserId) return Forbid();
            return View(c);
        }

        // GET: /Songs/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new CancionCreateVM());
        }

        // POST: /Songs/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CancionCreateVM vm)
        {
            // Validar archivo
            if (vm.AudioFile == null || vm.AudioFile.Length == 0)
                ModelState.AddModelError(nameof(vm.AudioFile), "Debes seleccionar un archivo de audio.");

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(vm);
            }

            try
            {
                // Guardar archivo en wwwroot/uploads
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.AudioFile.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                using var stream = System.IO.File.Create(filePath);
                await vm.AudioFile.CopyToAsync(stream);

                // Mapear a entidad Cancion
                var entidad = new Cancion
                {
                    Id = Guid.NewGuid(),
                    Titulo = vm.Titulo,
                    Duracion = vm.Duracion,
                    GeneroId = vm.GeneroId,
                    AlbumId = vm.AlbumId,
                    RutaArchivo = $"/uploads/{fileName}",
                    ArtistaId = IsAdmin
                        ? vm.ArtistaId!.Value
                        : CurrentUserId
                };

                _ctx.Canciones.Add(entidad);
                await _ctx.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al subir la canción: " + ex.Message);
                await PopulateDropdowns();
                return View(vm);
            }
        }

        // GET: /Songs/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var c = await _ctx.Canciones.FindAsync(id);
            if (c == null) return NotFound();
            if (IsArtist && c.ArtistaId != CurrentUserId) return Forbid();

            await PopulateDropdowns(c.ArtistaId);
            return View(c);
        }

        // POST: /Songs/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("Id,Titulo,Duracion,GeneroId,AlbumId,ArtistaId")] Cancion c,
            IFormFile? audioFile)
        {
            if (id != c.Id) return BadRequest();
            if (IsArtist && c.ArtistaId != CurrentUserId) return Forbid();

            // Si viene nuevo archivo, reemplazar
            if (audioFile != null && audioFile.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(audioFile.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                using var stream = System.IO.File.Create(filePath);
                await audioFile.CopyToAsync(stream);
                c.RutaArchivo = $"/uploads/{fileName}";
            }

            if (!ModelState.IsValid)
                return await ReloadEditView(c);

            if (IsArtist && !IsAdmin)
                c.ArtistaId = CurrentUserId;

            _ctx.Update(c);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Songs/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var c = await _ctx.Canciones
                .Include(c => c.Artista)
                .Include(c => c.Genero)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (c == null) return NotFound();
            if (IsArtist && c.ArtistaId != CurrentUserId) return Forbid();
            return View(c);
        }

        // POST: /Songs/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var c = await _ctx.Canciones.FindAsync(id);
            if (c == null) return NotFound();
            if (IsArtist && c.ArtistaId != CurrentUserId) return Forbid();

            _ctx.Canciones.Remove(c);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helpers

        private async Task PopulateDropdowns(Guid? artistaId = null)
        {
            ViewData["Generos"] = new SelectList(
                await _ctx.Generos.ToListAsync(), "Id", "Nombre");
            ViewData["Albums"] = new SelectList(
                await _ctx.Albumes.ToListAsync(), "Id", "Titulo");
            if (IsAdmin)
            {
                ViewData["Artistas"] = new SelectList(
                    await _ctx.Artistas.ToListAsync(),
                    "Id", "NombreArtistico",
                    artistaId);
            }
        }

        private async Task<IActionResult> ReloadEditView(Cancion c)
        {
            await PopulateDropdowns(c.ArtistaId);
            return View("Edit", c);
        }
    }
}
