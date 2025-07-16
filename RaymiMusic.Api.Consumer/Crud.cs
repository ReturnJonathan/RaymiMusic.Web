using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace RaymiMusic.Api.Consumer;

/// <summary>
/// Cliente CRUD genérico para consumir la API RaymiMusic.
/// Ejemplo de uso:
///   Crud<Cancion>.EndPoint = "https://localhost:7130/api/canciones";
///   var todas = await Crud<Cancion>.GetAllAsync();
/// </summary>
public static class Crud<T> where T : class
{
    /// <summary>URL base + recurso.  Ej.: https://…/api/canciones</summary>
    public static string EndPoint { get; set; } = string.Empty;

    // Reutilizamos una sola instancia de HttpClient (buena práctica)
    private static readonly HttpClient _http = new();

    /* ---------- MÉTODOS ---------- */

    public static async Task<List<T>> GetAllAsync()
    {
        var response = await _http.GetAsync(EndPoint);
        ThrowIfError(response);

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<T>>(json)!;
    }

    public static async Task<T?> GetByIdAsync(Guid id)
    {
        var response = await _http.GetAsync($"{EndPoint}/{id}");
        ThrowIfError(response);

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>Filtra por un campo numérico (ej.: /api/canciones/genero/5).</summary>
    public static async Task<List<T>> GetByAsync(string campo, int valor)
    {
        var response = await _http.GetAsync($"{EndPoint}/{campo}/{valor}");
        ThrowIfError(response);

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<T>>(json)!;
    }

    public static async Task<T?> CreateAsync(T item)
    {
        var payload = new StringContent(
            JsonConvert.SerializeObject(item),
            Encoding.UTF8,
            "application/json");

        var response = await _http.PostAsync(EndPoint, payload);
        ThrowIfError(response);

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static async Task<bool> UpdateAsync(Guid id, T item)
    {
        var payload = new StringContent(
            JsonConvert.SerializeObject(item),
            Encoding.UTF8,
            "application/json");

        var response = await _http.PutAsync($"{EndPoint}/{id}", payload);
        ThrowIfError(response);

        return true;
    }

    public static async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"{EndPoint}/{id}");
        ThrowIfError(response);

        return true;
    }

    /* ---------- Util ---------- */
    private static void ThrowIfError(HttpResponseMessage resp)
    {
        if (!resp.IsSuccessStatusCode)
            throw new Exception($"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}");
    }
}
