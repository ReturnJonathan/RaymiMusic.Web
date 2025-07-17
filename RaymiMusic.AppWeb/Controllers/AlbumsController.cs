using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;
using RaymiMusic.AppWeb.Models;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize]
    public class AlbumsController : Controller
    {
        private readonly AppDbContext _ctx;
        public AlbumsController(AppDbContext ctx) => _ctx = ctx;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsArtist => User.IsInRole("Artista");

        // GET: /Albums
        public async Task<IActionResult> Index()
        {
            var query = _ctx.Albumes
                            .Include(a => a.Artista)
                            .AsQueryable();

            if (IsArtist)
                query = query.Where(a => a.ArtistaId == CurrentUserId);

            var list = await query.AsNoTracking().ToListAsync();
            return View(list);
        }

        // GET: /Albums/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var album = await _ctx.Albumes
                                  .Include(a => a.Artista)
                                  .FirstOrDefaultAsync(a => a.Id == id);
            if (album == null) return NotFound();
            return View(album);
        }

        // GET: /Albums/Create
        public IActionResult Create()
        {
            var vm = new AlbumCreateVM();
            if (User.IsInRole("Admin"))
            {
                vm.Artistas = new SelectList(_ctx.Artistas, "Id", "NombreArtistico");
            }
            else
            {
                // Para artista, forzarle su propio Id
                vm.ArtistaId = CurrentUserId;
            }
            return View(vm);
        }


        // POST: /Albums/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                    vm.Artistas = new SelectList(_ctx.Artistas, "Id", "NombreArtistico", vm.ArtistaId);
                return View(vm);
            }

            var entidad = new Album
            {
                Id = Guid.NewGuid(),
                Titulo = vm.Titulo,
                FechaLanzamiento = vm.FechaLanzamiento,
                ArtistaId = User.IsInRole("Admin")
                               ? vm.ArtistaId
                               : CurrentUserId
            };
            _ctx.Albumes.Add(entidad);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: /Albums/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var album = await _ctx.Albumes.FindAsync(id);
            if (album == null) return NotFound();

            var vm = new AlbumCreateVM
            {
                Titulo = album.Titulo,
                FechaLanzamiento = album.FechaLanzamiento,
                ArtistaId = album.ArtistaId,
                // si no soy artista, dejo que elija
                Artistas = IsArtist
                    ? null
                    : new SelectList(_ctx.Artistas, "Id", "NombreArtistico", album.ArtistaId)
            };
            vm.Artistas = vm.Artistas;
            ViewData["Id"] = album.Id; // lo usamos en el form
            return View(vm);
        }

        // POST: /Albums/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AlbumCreateVM vm)
        {
            if (IsArtist)
                vm.ArtistaId = CurrentUserId;

            if (!ModelState.IsValid)
            {
                if (!IsArtist)
                    vm.Artistas = new SelectList(_ctx.Artistas, "Id", "NombreArtistico", vm.ArtistaId);
                ViewData["Id"] = id;
                return View(vm);
            }

            var album = await _ctx.Albumes.FindAsync(id);
            if (album == null) return NotFound();

            album.Titulo = vm.Titulo;
            album.FechaLanzamiento = vm.FechaLanzamiento;
            album.ArtistaId = vm.ArtistaId;

            _ctx.Update(album);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Albums/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var album = await _ctx.Albumes
                                  .Include(a => a.Artista)
                                  .FirstOrDefaultAsync(a => a.Id == id);
            if (album == null) return NotFound();
            return View(album);
        }

        // POST: /Albums/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var album = await _ctx.Albumes.FindAsync(id);
            if (album != null)
            {
                _ctx.Albumes.Remove(album);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
