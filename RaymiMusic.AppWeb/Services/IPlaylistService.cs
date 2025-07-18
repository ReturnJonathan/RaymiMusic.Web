using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RaymiMusic.AppWeb.Models;

namespace RaymiMusic.AppWeb.Services
{
    public interface IPlaylistService
    {
        Task<IEnumerable<PlaylistDTO>> GetAllAsync();
        Task<PlaylistDetailsVM?> GetByIdAsync(Guid id);
        Task CreateAsync(CreatePlaylistVM vm);
        Task AddSongAsync(Guid playlistId, Guid songId);
    }
}
