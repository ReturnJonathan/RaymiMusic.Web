using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Controllers
{
    public class PlansController : Controller
    {
        private readonly AppDbContext _ctx;
        public PlansController(AppDbContext ctx) => _ctx = ctx;

        // GET: /Plans
        public async Task<IActionResult> Index()
        {
            var planes = await _ctx.Planes
                .AsNoTracking()
                .ToListAsync();
            return View(planes);
        }

        // GET: /Plans/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var plan = await _ctx.Planes.FindAsync(id);
            if (plan == null) return NotFound();
            return View(plan);
        }

        // GET: /Plans/Create
        public IActionResult Create() => View();

        // POST: /Plans/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nombre,Precio,DescargasMaximas")] PlanSuscripcion plan)
        {
            if (!ModelState.IsValid) return View(plan);

            plan.Id = Guid.NewGuid();
            _ctx.Planes.Add(plan);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Plans/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var plan = await _ctx.Planes.FindAsync(id);
            if (plan == null) return NotFound();
            return View(plan);
        }

        // POST: /Plans/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("Id,Nombre,Precio,DescargasMaximas")] PlanSuscripcion plan)
        {
            if (id != plan.Id) return BadRequest();
            if (!ModelState.IsValid) return View(plan);

            _ctx.Planes.Update(plan);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Plans/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var plan = await _ctx.Planes.FindAsync(id);
            if (plan == null) return NotFound();
            return View(plan);
        }

        // POST: /Plans/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var plan = await _ctx.Planes.FindAsync(id);
            if (plan != null)
            {
                _ctx.Planes.Remove(plan);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
