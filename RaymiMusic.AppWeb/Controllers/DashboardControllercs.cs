using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.AppWeb.Models;

namespace RaymiMusic.AppWeb.Controllers
{
    [Authorize(Roles = "artista")]
    [Route("Artistas/[action]")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _ctx;
        public DashboardController(AppDbContext ctx) => _ctx = ctx;

        // GET: /Artistas/Dashboard/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Dashboard(Guid id)
        {
            // 1) Recupero el artista
            var artista = await _ctx.Artistas
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (artista == null)
                return NotFound("No se encontró el artista.");

            // 2) Totales de canciones y álbumes
            var totalCanciones = await _ctx.Canciones
                .CountAsync(c => c.ArtistaId == artista.Id);
            var totalAlbumes = await _ctx.Albumes
                .CountAsync(a => a.ArtistaId == artista.Id);

            // 3) Totales de playlists públicas creadas por este artista
            var totalPlaylists = await _ctx.ListasPublicas
                .CountAsync(p => p.CreadaPor == artista.NombreArtistico);

            // 4) Últimas 5 canciones (ordenadas por Id descendente)
            var ultimas = await _ctx.Canciones
                .Where(c => c.ArtistaId == artista.Id)
                .OrderByDescending(c => c.Id)
                .Take(5)
                .Select(c => new SongDTO
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    AlbumNombre = c.Album != null ? c.Album.Titulo : "—",
                    ArtistaNombre = artista.NombreArtistico,
                    Duracion = c.Duracion,
                    RutaArchivo = c.RutaArchivo
                })
                .ToListAsync();

            // 5) Lleno el ViewModel
            var vm = new ArtistDashboardVM
            {
                NombreArtista = artista.NombreArtistico,
                TotalCanciones = totalCanciones,
                TotalAlbumes = totalAlbumes,
                TotalPlaylists = totalPlaylists,
                UltimasCanciones = ultimas
            };

            return View(vm);
        }
    }
}
