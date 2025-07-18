using System.Collections.Generic;
using System.Threading.Tasks;
using RaymiMusic.AppWeb.Models;

namespace RaymiMusic.AppWeb.Services
{
    public interface ISongService
    {
        Task<IEnumerable<SongDTO>> GetAllAsync();
        Task<IEnumerable<SongDTO>> SearchAsync(string query);
        Task<SongDTO?> GetByIdAsync(Guid id);

    }
}
