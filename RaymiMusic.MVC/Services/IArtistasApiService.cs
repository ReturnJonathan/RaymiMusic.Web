using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public interface IArtistasApiService
    {
        Task<IEnumerable<Artista>> ObtenerTodosAsync();
        Task<Artista?> ObtenerPorIdAsync(Guid id);
        Task<Artista> CrearAsync(Artista artista);
        Task ActualizarAsync(Guid id, Artista artista);
        Task EliminarAsync(Guid id);

        Task<Artista?> ObtenerPorCorreoAsync(string correo);

    }
}
