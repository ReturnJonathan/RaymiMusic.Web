using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;

namespace RaymiMusic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlanesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Planes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanSuscripcion>>> GetPlanes()
        {
            return await _context.Planes
                                 .Include(p => p.Usuarios)   // Navegación inversa
                                 .ToListAsync();
        }

        // GET: api/Planes/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PlanSuscripcion>> GetPlan(Guid id)
        {
            var plan = await _context.Planes
                                     .Include(p => p.Usuarios)
                                     .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null) return NotFound();
            return plan;
        }

        // POST: api/Planes
        [HttpPost]
        public async Task<ActionResult<PlanSuscripcion>> PostPlan(PlanSuscripcion plan)
        {
            plan.Id = Guid.NewGuid();
            _context.Planes.Add(plan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlan), new { id = plan.Id }, plan);
        }

        // PUT: api/Planes/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutPlan(Guid id, PlanSuscripcion plan)
        {
            if (id != plan.Id) return BadRequest();

            _context.Entry(plan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Planes.AnyAsync(p => p.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Planes/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePlan(Guid id)
        {
            var plan = await _context.Planes.FindAsync(id);
            if (plan == null) return NotFound();

            _context.Planes.Remove(plan);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
