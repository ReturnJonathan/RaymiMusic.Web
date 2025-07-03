using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public interface ICancionesApiService
    {
        Task<IEnumerable<Cancion>> ObtenerTodosAsync();
        Task<Cancion?> ObtenerPorIdAsync(Guid id);
        Task<Cancion> CrearAsync(Cancion cancion);
        Task ActualizarAsync(Guid id, Cancion cancion);
        Task EliminarAsync(Guid id);
        /// <summary>
        /// Construye la URL para el streaming de la canción
        /// </summary>
        string ObtenerUrlStreaming(Guid id);
    }
}
