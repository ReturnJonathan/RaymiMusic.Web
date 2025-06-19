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
    public class ListasPublicasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListasPublicasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ListasPublicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListaPublica>>> GetListasPublicas()
        {
            return await _context.ListasPublicas
                                 .Include(lp => lp.CancionesEnListas)
                                     .ThenInclude(cl => cl.Cancion)
                                 .ToListAsync();
        }

        // GET: api/ListasPublicas/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ListaPublica>> GetListaPublica(Guid id)
        {
            var lista = await _context.ListasPublicas
                                      .Include(lp => lp.CancionesEnListas)
                                          .ThenInclude(cl => cl.Cancion)
                                      .FirstOrDefaultAsync(lp => lp.Id == id);

            if (lista == null) return NotFound();
            return lista;
        }

        // POST: api/ListasPublicas
        [HttpPost]
        public async Task<ActionResult<ListaPublica>> PostListaPublica(ListaPublica lista)
        {
            lista.Id = Guid.NewGuid();
            _context.ListasPublicas.Add(lista);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetListaPublica), new { id = lista.Id }, lista);
        }

        // PUT: api/ListasPublicas/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutListaPublica(Guid id, ListaPublica lista)
        {
            if (id != lista.Id) return BadRequest();

            _context.Entry(lista).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.ListasPublicas.AnyAsync(lp => lp.Id == id);
                if (!exists) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/ListasPublicas/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteListaPublica(Guid id)
        {
            var lista = await _context.ListasPublicas.FindAsync(id);
            if (lista == null) return NotFound();

            _context.ListasPublicas.Remove(lista);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/ListasPublicas/{listaId}/AgregarCancion/{cancionId}
        [HttpPost("{listaId:guid}/AgregarCancion/{cancionId:guid}")]
        public async Task<IActionResult> AgregarCancion(Guid listaId, Guid cancionId)
        {
            var lista = await _context.ListasPublicas.FindAsync(listaId);
            var cancion = await _context.Canciones.FindAsync(cancionId);
            if (lista == null || cancion == null) return NotFound();

            var enlace = new CancionLista
            {
                Id = Guid.NewGuid(),
                ListaReproduccionId = listaId,
                CancionId = cancionId
            };
            _context.CancionesEnListas.Add(enlace);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ListasPublicas/{listaId}/QuitarCancion/{cancionId}
        [HttpDelete("{listaId:guid}/QuitarCancion/{cancionId:guid}")]
        public async Task<IActionResult> QuitarCancion(Guid listaId, Guid cancionId)
        {
            var enlace = await _context.CancionesEnListas
                .FirstOrDefaultAsync(cl => cl.ListaReproduccionId == listaId && cl.CancionId == cancionId);
            if (enlace == null) return NotFound();

            _context.CancionesEnListas.Remove(enlace);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
