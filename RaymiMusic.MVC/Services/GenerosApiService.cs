using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public class GenerosApiService : IGenerosApiService
    {
        private readonly HttpClient _http;
        public GenerosApiService(IHttpClientFactory httpFactory)
            => _http = httpFactory.CreateClient("RaymiMusicApi");

        public async Task<IEnumerable<Genero>> ObtenerTodosAsync()
            => await _http.GetFromJsonAsync<IEnumerable<Genero>>("api/Generos")
               ?? Array.Empty<Genero>();

        public async Task<Genero?> ObtenerPorIdAsync(Guid id)
            => await _http.GetFromJsonAsync<Genero>($"api/Generos/{id}");

        public async Task<Genero> CrearAsync(Genero genero)
        {
            var resp = await _http.PostAsJsonAsync("api/Generos", genero);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<Genero>()
                   ?? throw new ApplicationException("Error al crear género");
        }

        public async Task ActualizarAsync(Guid id, Genero genero)
        {
            var resp = await _http.PutAsJsonAsync($"api/Generos/{id}", genero);
            resp.EnsureSuccessStatusCode();
        }

        public async Task EliminarAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/Generos/{id}");
            resp.EnsureSuccessStatusCode();
        }
    }
}
