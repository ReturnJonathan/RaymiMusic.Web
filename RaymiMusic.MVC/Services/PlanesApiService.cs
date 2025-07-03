using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public class PlanesApiService : IPlanesApiService
    {
        private readonly HttpClient _http;
        public PlanesApiService(IHttpClientFactory httpFactory)
            => _http = httpFactory.CreateClient("RaymiMusicApi");

        public async Task<IEnumerable<PlanSuscripcion>> ObtenerTodosAsync()
            => await _http.GetFromJsonAsync<IEnumerable<PlanSuscripcion>>("api/Planes")
               ?? Array.Empty<PlanSuscripcion>();

        public async Task<PlanSuscripcion> CrearAsync(PlanSuscripcion plan)
        {
            var resp = await _http.PostAsJsonAsync("api/Planes", plan);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<PlanSuscripcion>()
                   ?? throw new ApplicationException("Error al crear plan");
        }
    }
}
