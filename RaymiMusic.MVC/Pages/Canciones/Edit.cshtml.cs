using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Canciones
{
    public class EditModel : PageModel
    {
        private readonly ICancionesApiService _svc;
        private readonly IArtistasApiService _artSvc;
        private readonly IGenerosApiService _genSvc;
        private readonly IWebHostEnvironment _env;

        public EditModel(
            ICancionesApiService svc,
            IArtistasApiService artSvc,
            IGenerosApiService genSvc,
            IWebHostEnvironment env)
        {
            _svc = svc;
            _artSvc = artSvc;
            _genSvc = genSvc;
            _env = env;
        }

        /* ---------- PROPIEDADES ---------- */
        [BindProperty] public Cancion Cancion { get; set; } = null!;
        [BindProperty] public IFormFile? Archivo { get; set; }

        public IEnumerable<Artista> Artistas { get; set; } = Array.Empty<Artista>();
        public IEnumerable<Genero> Generos { get; set; } = Array.Empty<Genero>();

        /* ---------- GET ---------- */
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var cancion = await _svc.ObtenerPorIdAsync(id);
            if (cancion is null) return NotFound();

            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            if (rol == "Artista" && cancion.Artista?.NombreArtistico != correo)
                return RedirectToPage("/Cuenta/Login");

            Cancion = cancion;
            Artistas = await _artSvc.ObtenerTodosAsync();
            Generos = await _genSvc.ObtenerTodosAsync();
            return Page();
        }

        /* ---------- POST ---------- */
        public async Task<IActionResult> OnPostAsync()
        {
            /* No queremos que la ausencia de RutaArchivo marque el modelo como inválido */
            ModelState.Remove("Cancion.RutaArchivo");

            /* --- Validar archivo si el usuario seleccionó uno nuevo --- */
            if (Archivo is not null && Archivo.Length > 0)
            {
                var permitidas = new[] { ".mp3", ".wav", ".aac", ".flac", ".ogg" };
                var ext = Path.GetExtension(Archivo.FileName).ToLowerInvariant();
                if (!permitidas.Contains(ext))
                    ModelState.AddModelError("Archivo", "Formato no soportado.");
            }

            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            if (rol == "Artista")
            {
                var original = await _svc.ObtenerPorIdAsync(Cancion.Id);
                if (original is null || original.Artista?.NombreArtistico != correo)
                    return RedirectToPage("/Cuenta/Login");

                Cancion.ArtistaId = original.ArtistaId; // proteger ArtistaId
                Cancion.RutaArchivo = original.RutaArchivo; // conservar ruta salvo que se reemplace
            }

            if (!ModelState.IsValid)
            {
                Artistas = await _artSvc.ObtenerTodosAsync();
                Generos = await _genSvc.ObtenerTodosAsync();
                return Page();
            }

            /* --- Si hay archivo nuevo, sustituir --- */
            if (Archivo is not null && Archivo.Length > 0)
            {
                // 1) Elimina el anterior (opcional)
                var rutaFisicaVieja = Path.Combine(_env.WebRootPath, Cancion.RutaArchivo ?? "");
                if (System.IO.File.Exists(rutaFisicaVieja))
                    System.IO.File.Delete(rutaFisicaVieja);

                // 2) Guarda el nuevo
                var carpeta = Path.Combine(_env.WebRootPath, "uploads", "canciones");
                Directory.CreateDirectory(carpeta);

                var nuevoNombre = $"{Guid.NewGuid()}{Path.GetExtension(Archivo.FileName)}";
                var rutaFisica = Path.Combine(carpeta, nuevoNombre);

                await using (var stream = System.IO.File.Create(rutaFisica))
                {
                    await Archivo.CopyToAsync(stream);
                }

                Cancion.RutaArchivo = Path.Combine("uploads", "canciones", nuevoNombre)
                                      .Replace("\\", "/");
            }

            await _svc.ActualizarAsync(Cancion.Id, Cancion);
            return RedirectToPage("Index");
        }
    }
}
