using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Controllers
{
    public class ArtistasController : Controller
    {
        private readonly AppDbContext _ctx;
        public ArtistasController(AppDbContext ctx) => _ctx = ctx;

        // GET: /Artistas
        public async Task<IActionResult> Index()
            => View(await _ctx.Artistas.AsNoTracking().ToListAsync());

        // GET: /Artistas/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var artista = await _ctx.Artistas.FindAsync(id);
            if (artista == null) return NotFound();
            return View(artista);
        }

        // GET: /Artistas/Create
        public IActionResult Create() => View();

        // POST: /Artistas/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("NombreArtistico,Biografia,UrlFotoPerfil,UrlFotoPortada")] Artista artista)
        {
            if (!ModelState.IsValid)
                return View(artista);

            artista.Id = Guid.NewGuid();
            _ctx.Artistas.Add(artista);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Artistas/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var artista = await _ctx.Artistas.FindAsync(id);
            if (artista == null) return NotFound();
            return View(artista);
        }

        // POST: /Artistas/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("Id,NombreArtistico,Biografia,UrlFotoPerfil,UrlFotoPortada")] Artista artista)
        {
            if (id != artista.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(artista);

            _ctx.Artistas.Update(artista);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Artistas/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var artista = await _ctx.Artistas.FindAsync(id);
            if (artista == null) return NotFound();
            return View(artista);
        }

        // POST: /Artistas/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var artista = await _ctx.Artistas.FindAsync(id);
            if (artista != null)
            {
                _ctx.Artistas.Remove(artista);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
