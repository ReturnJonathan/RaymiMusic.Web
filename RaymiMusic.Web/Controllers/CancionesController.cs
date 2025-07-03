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
    public class CancionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CancionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Canciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cancion>>> GetCanciones()
        {
            return await _context.Canciones
                                 .Include(c => c.Genero)
                                 .Include(c => c.Artista)
                                 .Include(c => c.Album)
                                 .ToListAsync();
        }

        // GET: api/Canciones/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Cancion>> GetCancion(Guid id)
        {
            var cancion = await _context.Canciones
                                        .Include(c => c.Genero)
                                        .Include(c => c.Artista)
                                        .Include(c => c.Album)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (cancion == null) return NotFound();
            return cancion;
        }

        // GET: api/Canciones/Play/{id}
        [HttpGet("Play/{id:guid}")]
        public async Task<IActionResult> Play(Guid id)
        {
            var cancion = await _context.Canciones.FindAsync(id);
            if (cancion == null) return NotFound();

            // Asumimos que RutaArchivo es un path físico o URL accesible en el servidor
            var filePath = cancion.RutaArchivo;
            if (!System.IO.File.Exists(filePath))
                return NotFound("Archivo de audio no encontrado.");

            var stream = System.IO.File.OpenRead(filePath);
            // Devuelve el stream con content-type de audio
            return File(stream, "audio/mpeg", enableRangeProcessing: true);
        }


        // POST: api/Canciones
        [HttpPost]
        public async Task<ActionResult<Cancion>> PostCancion(Cancion cancion)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(cancion.Titulo))
                return BadRequest("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(cancion.RutaArchivo))
                return BadRequest("La ruta del archivo es obligatoria.");

            if (cancion.Duracion == TimeSpan.Zero)
                return BadRequest("La duración debe ser mayor a 0.");

            if (cancion.GeneroId == Guid.Empty)
                return BadRequest("Debe especificar un género.");

            if (cancion.ArtistaId == Guid.Empty)
                return BadRequest("Debe especificar un artista.");

            try
            {
                cancion.Id = Guid.NewGuid();
                _context.Canciones.Add(cancion);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCancion),
                                       new { id = cancion.Id },
                                       cancion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al guardar la canción: " + ex.Message);
            }
        }


        // PUT: api/Canciones/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutCancion(Guid id, Cancion cancion)
        {
            if (id != cancion.Id) return BadRequest();

            _context.Entry(cancion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Canciones.AnyAsync(c => c.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Canciones/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCancion(Guid id)
        {
            var cancion = await _context.Canciones.FindAsync(id);
            if (cancion == null) return NotFound();

            _context.Canciones.Remove(cancion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
