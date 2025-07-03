using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public class UsuarioApiService : IUsuarioApiService
    {
        private readonly HttpClient _http;

        public UsuarioApiService(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("RaymiMusicApi");
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<Usuario>>("api/Usuarios")
                   ?? Array.Empty<Usuario>();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<Usuario>($"api/Usuarios/{id}");
        }

        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            var resp = await _http.PostAsJsonAsync("api/Usuarios", usuario);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<Usuario>()
                   ?? throw new ApplicationException("Error al crear usuario");
        }

        public async Task ActualizarAsync(Guid id, Usuario usuario)
        {
            var resp = await _http.PutAsJsonAsync($"api/Usuarios/{id}", usuario);
            resp.EnsureSuccessStatusCode();
        }

        public async Task EliminarAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/Usuarios/{id}");
            resp.EnsureSuccessStatusCode();
        }
    }
}
