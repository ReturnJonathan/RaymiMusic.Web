using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public class ArtistasApiService : IArtistasApiService
    {
        private readonly HttpClient _http;
        public ArtistasApiService(IHttpClientFactory httpFactory)
            => _http = httpFactory.CreateClient("RaymiMusicApi");

        public async Task<IEnumerable<Artista>> ObtenerTodosAsync()
            => await _http.GetFromJsonAsync<IEnumerable<Artista>>("api/Artistas")
               ?? Array.Empty<Artista>();

        public async Task<Artista?> ObtenerPorIdAsync(Guid id)
            => await _http.GetFromJsonAsync<Artista>($"api/Artistas/{id}");

        public async Task<Artista> CrearAsync(Artista artista)
        {
            var resp = await _http.PostAsJsonAsync("api/Artistas", artista);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<Artista>()
                   ?? throw new ApplicationException("Error al crear artista");
        }

        public async Task ActualizarAsync(Guid id, Artista artista)
        {
            var resp = await _http.PutAsJsonAsync($"api/Artistas/{id}", artista);
            resp.EnsureSuccessStatusCode();
        }

        public async Task EliminarAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/Artistas/{id}");
            resp.EnsureSuccessStatusCode();
        }
        public async Task<Artista?> ObtenerPorCorreoAsync(string correo)
        {
            var response = await _http.GetAsync($"api/artistas/por-correo/{correo}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<Artista>();
        }

    }
}
