// RaymiMusic.AppWeb/Services/ArtistService.cs
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.AppWeb.Models;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public class ArtistService : IArtistService
    {
        private readonly HttpClient _http;
        public ArtistService(HttpClient http) => _http = http;

        public async Task<ArtistDashboardVM?> GetDashboardAsync(string userId)
        {
            // 1) Traigo todos los artistas
            var artistas = await _http.GetFromJsonAsync<Artista[]>("api/Artistas");
            if (artistas == null)
                return null;

            // Aquí asumimos que el artistId coincide con el userId
            // Si no: tendrías que buscar por alguna propiedad de Artista, p.ej. correo
            var artista = artistas.FirstOrDefault(a => a.Id.ToString() == userId);
            if (artista == null)
                return null;

            // 2) Traigo sus canciones
            var allSongs = await _http.GetFromJsonAsync<Cancion[]>("api/Canciones");
            var canciones = allSongs?
                .Where(c => c.ArtistaId == artista.Id)
                // Si quieres orden, usa c.Id o cualquier otra propiedad existente
                .OrderByDescending(c => c.Id)
                .ToArray()
                ?? Array.Empty<Cancion>();

            // 3) Traigo álbumes
            var allAlbums = await _http.GetFromJsonAsync<Album[]>("api/Albumes");
            var albumes = allAlbums?
                .Where(al => al.ArtistaId == artista.Id)
                .ToArray()
                ?? Array.Empty<Album>();

            // 4) Traigo playlists suyas filtrando por el mismo userId
            var listas = await _http.GetFromJsonAsync<ListaReproduccion[]>("api/ListasReproduccion");
            var propias = listas?
                .Where(l => l.UsuarioId.ToString() == userId)
                .ToArray()
                ?? Array.Empty<ListaReproduccion>();

            // 5) Proyectar al VM
            return new ArtistDashboardVM
            {
                NombreArtista = artista.NombreArtistico,
                TotalCanciones = canciones.Length,
                TotalAlbumes = albumes.Length,
                TotalPlaylists = propias.Length,
                UltimasCanciones = canciones
                    .Take(5)
                    .Select(c => new SongDTO
                    {
                        Id = c.Id,
                        Titulo = c.Titulo,
                        AlbumNombre = c.Album?.Titulo ?? "—",
                        ArtistaNombre = artista.NombreArtistico,
                        Duracion = c.Duracion,
                        RutaArchivo = c.RutaArchivo
                    })
            };
        }
    }
}
