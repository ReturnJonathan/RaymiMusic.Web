using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.AppWeb.Models;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public class SongService : ISongService
    {
        private readonly HttpClient _http;

        public SongService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<SongDTO>> GetAllAsync()
        {
            // Llama a GET /api/canciones
            var canciones = await _http.GetFromJsonAsync<IEnumerable<Cancion>>("api/canciones");
            return Map(canciones);
        }

        public async Task<IEnumerable<SongDTO>> SearchAsync(string query)
        {
            // Si tu API tiene endpoint de búsqueda, úsalo; 
            // Si no, filtramos en cliente:
            var canciones = await _http.GetFromJsonAsync<IEnumerable<Cancion>>("api/canciones");
            var q = query.Trim().ToLower();
            var filtradas = canciones.Where(c =>
                c.Titulo.ToLower().Contains(q) ||
                (c.Album != null && c.Album.Titulo.ToLower().Contains(q)) ||
                (c.Artista != null && c.Artista.NombreArtistico.ToLower().Contains(q))
            );
            return Map(filtradas);
        }

        private IEnumerable<SongDTO> Map(IEnumerable<Cancion>? lista)
        {
            if (lista == null) return Array.Empty<SongDTO>();

            return lista.Select(c => new SongDTO
            {
                Id = c.Id,
                Titulo = c.Titulo,
                Duracion = c.Duracion,
                RutaArchivo = c.RutaArchivo,
                AlbumNombre = c.Album?.Titulo ?? "—",
                ArtistaNombre = c.Artista?.NombreArtistico ?? "—"
            });
        }

        public async Task<SongDTO?> GetByIdAsync(Guid id)
        {
            // GET /api/canciones/{id}
            var c = await _http.GetFromJsonAsync<Cancion>($"api/canciones/{id}");
            if (c == null) return null;
            return new SongDTO
            {
                Id = c.Id,
                Titulo = c.Titulo,
                Duracion = c.Duracion,
                RutaArchivo = c.RutaArchivo,
                AlbumNombre = c.Album?.Titulo ?? "—",
                ArtistaNombre = c.Artista?.NombreArtistico ?? "—"
            };
        }

    }
}
