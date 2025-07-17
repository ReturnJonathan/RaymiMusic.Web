using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class GenerosController : Controller
    {
        private readonly AppDbContext _ctx;
        public GenerosController(AppDbContext ctx) => _ctx = ctx;

        // GET: /Generos
        public async Task<IActionResult> Index()
        {
            var lista = await _ctx.Generos
                .AsNoTracking()
                .ToListAsync();
            return View(lista);
        }

        // GET: /Generos/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var g = await _ctx.Generos.FindAsync(id);
            if (g == null) return NotFound();
            return View(g);
        }

        // GET: /Generos/Create
        public IActionResult Create() => View();

        // POST: /Generos/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nombre")] Genero genero)
        {
            if (!ModelState.IsValid) return View(genero);

            genero.Id = Guid.NewGuid();
            _ctx.Generos.Add(genero);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Generos/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var g = await _ctx.Generos.FindAsync(id);
            if (g == null) return NotFound();
            return View(g);
        }

        // POST: /Generos/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("Id,Nombre")] Genero genero)
        {
            if (id != genero.Id) return BadRequest();
            if (!ModelState.IsValid) return View(genero);

            _ctx.Generos.Update(genero);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Generos/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var g = await _ctx.Generos.FindAsync(id);
            if (g == null) return NotFound();
            return View(g);
        }

        // POST: /Generos/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var g = await _ctx.Generos.FindAsync(id);
            if (g != null)
            {
                _ctx.Generos.Remove(g);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
