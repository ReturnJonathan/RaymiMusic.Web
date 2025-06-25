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
    public class ArtistasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArtistasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Artistas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artista>>> GetArtistas()
        {
            return await _context.Artistas
                                 .Include(a => a.Canciones)
                                 .Include(a => a.Albumes)
                                 .ToListAsync();
        }

        // GET: api/Artistas/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Artista>> GetArtista(Guid id)
        {
            var artista = await _context.Artistas
                                        .Include(a => a.Canciones)
                                        .Include(a => a.Albumes)
                                        .FirstOrDefaultAsync(a => a.Id == id);
            if (artista == null) return NotFound();
            return artista;
        }

        // POST: api/Artistas
        [HttpPost]
        public async Task<ActionResult<Artista>> PostArtista(Artista artista)
        {
            artista.Id = Guid.NewGuid();
            _context.Artistas.Add(artista);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArtista),
                                   new { id = artista.Id },
                                   artista);
        }

        // PUT: api/Artistas/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutArtista(Guid id, Artista artista)
        {
            if (id != artista.Id) return BadRequest();

            _context.Entry(artista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Artistas.AnyAsync(a => a.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Artistas/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteArtista(Guid id)
        {
            var artista = await _context.Artistas.FindAsync(id);
            if (artista == null) return NotFound();

            _context.Artistas.Remove(artista);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("por-correo/{correo}")]
        public async Task<ActionResult<Artista>> ObtenerPorCorreo(string correo)
        {
            var artista = await _context.Artistas
                .Where(a => a.NombreArtistico == correo)  // Ajusta esto según cómo enlazas artistas a usuarios
                .SingleOrDefaultAsync();

            if (artista == null) return NotFound();
            return artista;
        }

    }
}
