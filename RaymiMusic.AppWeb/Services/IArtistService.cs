// RaymiMusic.AppWeb/Services/IArtistService.cs
using System.Threading.Tasks;
using RaymiMusic.AppWeb.Models;

namespace RaymiMusic.AppWeb.Services
{
    public interface IArtistService
    {
        Task<ArtistDashboardVM?> GetDashboardAsync(string userId);
    }
}
