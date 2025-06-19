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
    public class GenerosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GenerosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Generos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> GetGeneros()
        {
            return await _context.Generos
                                 .Include(g => g.Canciones)
                                 .ToListAsync();
        }

        // GET: api/Generos/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Genero>> GetGenero(Guid id)
        {
            var genero = await _context.Generos
                                       .Include(g => g.Canciones)
                                       .FirstOrDefaultAsync(g => g.Id == id);
            if (genero == null) return NotFound();
            return genero;
        }

        // POST: api/Generos
        [HttpPost]
        public async Task<ActionResult<Genero>> PostGenero(Genero genero)
        {
            genero.Id = Guid.NewGuid();
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGenero),
                                   new { id = genero.Id },
                                   genero);
        }

        // PUT: api/Generos/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutGenero(Guid id, Genero genero)
        {
            if (id != genero.Id) return BadRequest();

            _context.Entry(genero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Generos.AnyAsync(g => g.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Generos/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGenero(Guid id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null) return NotFound();

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
