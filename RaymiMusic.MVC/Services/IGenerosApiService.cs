using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public interface IGenerosApiService
    {
        Task<IEnumerable<Genero>> ObtenerTodosAsync();
        Task<Genero?> ObtenerPorIdAsync(Guid id);
        Task<Genero> CrearAsync(Genero genero);
        Task ActualizarAsync(Guid id, Genero genero);
        Task EliminarAsync(Guid id);
    }
}
