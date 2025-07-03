using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RaymiMusic.Modelos;

namespace RaymiMusic.MVC.Services
{
    public interface IUsuarioApiService
    {
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(Guid id);
        Task<Usuario> CrearAsync(Usuario usuario);
        Task ActualizarAsync(Guid id, Usuario usuario);
        Task EliminarAsync(Guid id);
    }
}
