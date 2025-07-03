using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public class CancionesApiService : ICancionesApiService
    {
        private readonly HttpClient _http;
        public CancionesApiService(IHttpClientFactory httpFactory)
            => _http = httpFactory.CreateClient("RaymiMusicApi");

        public async Task<IEnumerable<Cancion>> ObtenerTodosAsync()
            => await _http.GetFromJsonAsync<IEnumerable<Cancion>>("api/Canciones")
               ?? Array.Empty<Cancion>();

        public async Task<Cancion?> ObtenerPorIdAsync(Guid id)
            => await _http.GetFromJsonAsync<Cancion>($"api/Canciones/{id}");

        public async Task<Cancion> CrearAsync(Cancion cancion)
        {
            var resp = await _http.PostAsJsonAsync("api/Canciones", cancion);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<Cancion>()
                   ?? throw new ApplicationException("Error al crear canción");
        }

        public async Task ActualizarAsync(Guid id, Cancion cancion)
        {
            var resp = await _http.PutAsJsonAsync($"api/Canciones/{id}", cancion);
            resp.EnsureSuccessStatusCode();
        }

        public async Task EliminarAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/Canciones/{id}");
            resp.EnsureSuccessStatusCode();
        }

        public string ObtenerUrlStreaming(Guid id)
        {
            // Aquí devolvemos directamente la ruta al endpoint de tu API
            return $"https://localhost:7153/api/Canciones/Play/{id}";
        }
    }
}
