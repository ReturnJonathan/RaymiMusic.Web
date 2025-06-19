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
    public class PerfilesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfilesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Perfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfiles()
        {
            return await _context.Perfiles
                                 .Include(p => p.Usuario)
                                 .ToListAsync();
        }

        // GET: api/Perfiles/{usuarioId}
        [HttpGet("{usuarioId:guid}")]
        public async Task<ActionResult<Perfil>> GetPerfil(Guid usuarioId)
        {
            var perfil = await _context.Perfiles
                                       .Include(p => p.Usuario)
                                       .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
            if (perfil == null) return NotFound();
            return perfil;
        }

        // POST: api/Perfiles
        [HttpPost]
        public async Task<ActionResult<Perfil>> PostPerfil(Perfil perfil)
        {
            // Asegurarse de que no exista un perfil para ese usuario
            if (await _context.Perfiles.AnyAsync(p => p.UsuarioId == perfil.UsuarioId))
                return Conflict("El usuario ya tiene un perfil.");

            perfil.Usuario = null!; // Evitar ciclos de referencia al insertar
            _context.Perfiles.Add(perfil);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerfil),
                                   new { usuarioId = perfil.UsuarioId },
                                   perfil);
        }

        // PUT: api/Perfiles/{usuarioId}
        [HttpPut("{usuarioId:guid}")]
        public async Task<IActionResult> PutPerfil(Guid usuarioId, Perfil perfil)
        {
            if (usuarioId != perfil.UsuarioId) return BadRequest();

            _context.Entry(perfil).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Perfiles.AnyAsync(p => p.UsuarioId == usuarioId);
                if (!exists) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/Perfiles/{usuarioId}
        [HttpDelete("{usuarioId:guid}")]
        public async Task<IActionResult> DeletePerfil(Guid usuarioId)
        {
            var perfil = await _context.Perfiles.FindAsync(usuarioId);
            if (perfil == null) return NotFound();

            _context.Perfiles.Remove(perfil);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
