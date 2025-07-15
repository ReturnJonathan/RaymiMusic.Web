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
    public class CreateModel : PageModel
    {
        private readonly ICancionesApiService _svc;
        private readonly IArtistasApiService _artSvc;
        private readonly IGenerosApiService _genSvc;
        private readonly IWebHostEnvironment _env;

        public CreateModel(
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
        [BindProperty] public Cancion Cancion { get; set; } = new();
        [BindProperty] public IFormFile? Archivo { get; set; }

        public IEnumerable<Artista> Artistas { get; set; } = Array.Empty<Artista>();
        public IEnumerable<Genero> Generos { get; set; } = Array.Empty<Genero>();

        public List<string> Errores { get; set; } = new();

        /* ---------- GET ---------- */
        public async Task<IActionResult> OnGetAsync()
        {
            Generos = await _genSvc.ObtenerTodosAsync();
            Artistas = await _artSvc.ObtenerTodosAsync();
            return Page();
        }

        /* ---------- POST ---------- */
        public async Task<IActionResult> OnPostAsync()
        {
            Generos = await _genSvc.ObtenerTodosAsync();
            Artistas = await _artSvc.ObtenerTodosAsync();

            /* ⬇️  Evita que la ausencia de RutaArchivo marque el modelo como inválido */
            ModelState.Remove("Cancion.RutaArchivo");

            /* ---------- Validar archivo ---------- */
            if (Archivo is null || Archivo.Length == 0)
                ModelState.AddModelError("Archivo", "Debes seleccionar un archivo de audio.");
            else
            {
                var ext = Path.GetExtension(Archivo.FileName).ToLowerInvariant();
                var permitidas = new[] { ".mp3", ".wav", ".aac", ".flac", ".ogg" };
                if (!permitidas.Contains(ext))
                    ModelState.AddModelError("Archivo", "Formato no soportado.");
            }

            if (!ModelState.IsValid)
            {
                Errores = ModelState.Values
                                     .SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();
                return Page();
            }

            /* ---------- Asignar artista si rol = Artista ---------- */
            var rol = HttpContext.Session.GetString("Rol");
            var correo = HttpContext.Session.GetString("Correo");
            if (rol == "Artista")
            {
                var artista = await _artSvc.ObtenerPorCorreoAsync(correo!);
                if (artista is null) return Unauthorized();
                Cancion.ArtistaId = artista.Id;
            }

            /* ---------- Guardar archivo ---------- */
            var carpeta = Path.Combine(_env.WebRootPath, "uploads", "canciones");
            Directory.CreateDirectory(carpeta);

            var nombreNuevo = $"{Guid.NewGuid()}{Path.GetExtension(Archivo!.FileName)}";
            var rutaFisica = Path.Combine(carpeta, nombreNuevo);

            await using (var stream = System.IO.File.Create(rutaFisica))
            {
                await Archivo.CopyToAsync(stream);
            }

            Cancion.RutaArchivo = Path.Combine("uploads", "canciones", nombreNuevo)
                                  .Replace("\\", "/");

            /* ---------- Persistir canción ---------- */
            Cancion.Id = Guid.NewGuid();
            await _svc.CrearAsync(Cancion);

            return RedirectToPage("Index");
        }

    }
}
