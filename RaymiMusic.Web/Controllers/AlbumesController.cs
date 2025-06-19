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
    public class AlbumesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlbumesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Albumes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbumes()
        {
            return await _context.Albumes
                                 .Include(a => a.Artista)
                                 .Include(a => a.Canciones)
                                 .ToListAsync();
        }

        // GET: api/Albumes/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Album>> GetAlbum(Guid id)
        {
            var album = await _context.Albumes
                                      .Include(a => a.Artista)
                                      .Include(a => a.Canciones)
                                      .FirstOrDefaultAsync(a => a.Id == id);
            if (album == null) return NotFound();
            return album;
        }

        // POST: api/Albumes
        [HttpPost]
        public async Task<ActionResult<Album>> PostAlbum(Album album)
        {
            album.Id = Guid.NewGuid();
            _context.Albumes.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlbum),
                                   new { id = album.Id },
                                   album);
        }

        // PUT: api/Albumes/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutAlbum(Guid id, Album album)
        {
            if (id != album.Id) return BadRequest();

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Albumes.AnyAsync(a => a.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Albumes/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAlbum(Guid id)
        {
            var album = await _context.Albumes.FindAsync(id);
            if (album == null) return NotFound();

            _context.Albumes.Remove(album);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
