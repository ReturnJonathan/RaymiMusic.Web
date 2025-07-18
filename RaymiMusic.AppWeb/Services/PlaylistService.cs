﻿using RaymiMusic.AppWeb.Models;
using RaymiMusic.AppWeb.Services;
using RaymiMusic.Modelos;

public class PlaylistService : IPlaylistService
{
    private readonly HttpClient _http;
    public PlaylistService(HttpClient http) => _http = http;

    public async Task<IEnumerable<PlaylistDTO>> GetAllAsync()
    {
        return await _http
           .GetFromJsonAsync<IEnumerable<PlaylistDTO>>("api/ListasReproduccion")
           ?? Array.Empty<PlaylistDTO>();
    }

    public async Task<PlaylistDetailsVM?> GetByIdAsync(Guid id)
    {
        // 1) Trae el objeto ListaReproduccion bruto
        var lista = await _http.GetFromJsonAsync<ListaReproduccion>($"api/ListasReproduccion/{id}");
        if (lista == null) return null;

        // 2) Proyecta a PlaylistDetailsVM
        return new PlaylistDetailsVM
        {
            Id = lista.Id,
            Nombre = lista.Nombre,
            EsPublica = lista.EsPublica,
            Canciones = lista.CancionesEnListas.Select(cl => new SongDTO
            {
                Id = cl.Cancion.Id,
                Titulo = cl.Cancion.Titulo,
                AlbumNombre = cl.Cancion.Album?.Titulo ?? "—",
                ArtistaNombre = cl.Cancion.Artista?.NombreArtistico ?? "—",
                Duracion = cl.Cancion.Duracion,
                RutaArchivo = cl.Cancion.RutaArchivo
            })
        };
    }

    public async Task CreateAsync(CreatePlaylistVM vm)
    {
        // POST /api/ListasReproduccion
        await _http.PostAsJsonAsync("api/ListasReproduccion", new
        {
            nombre = vm.Nombre,
            esPublica = vm.EsPublica,
            usuarioId = vm.UsuarioId
        });
    }


    public async Task AddSongAsync(Guid playlistId, Guid songId)
    {
        // POST /api/ListasReproduccion/{listaId}/AgregarCancion/{cancionId}
        var response = await _http.PostAsync(
            $"api/ListasReproduccion/{playlistId}/AgregarCancion/{songId}",
            null
        );
        response.EnsureSuccessStatusCode();
    }

}
