using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using RaymiMusic.Modelos;
using RaymiMusic.MVC.Services;
namespace RaymiMusic.MVC.Pages.Canciones { 
public class CreateModel : PageModel
{
    private readonly ICancionesApiService _svc;
    private readonly IArtistasApiService _artSvc;
    private readonly IGenerosApiService _genSvc;

    public CreateModel(
        ICancionesApiService svc,
        IArtistasApiService artSvc,
        IGenerosApiService genSvc)
    {
        _svc = svc;
        _artSvc = artSvc;
        _genSvc = genSvc;
    }

    [BindProperty]
    public Cancion Cancion { get; set; } = new();

    public IEnumerable<Artista> Artistas { get; set; } = Array.Empty<Artista>();
    public IEnumerable<Genero> Generos { get; set; } = Array.Empty<Genero>();

    public List<string> Errores { get; set; } = new();

    // ✅ Carga inicial de géneros y artistas
    public async Task<IActionResult> OnGetAsync()
    {
        Generos = await _genSvc.ObtenerTodosAsync();
        Artistas = await _artSvc.ObtenerTodosAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Generos = await _genSvc.ObtenerTodosAsync();
        Artistas = await _artSvc.ObtenerTodosAsync();

        if (!ModelState.IsValid)
        {
            Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Page();
        }

        var rol = HttpContext.Session.GetString("Rol");
        var correo = HttpContext.Session.GetString("Correo");

        if (rol == "Artista")
        {
            var artista = await _artSvc.ObtenerPorCorreoAsync(correo!);
            if (artista == null) return Unauthorized();
            Cancion.ArtistaId = artista.Id;
        }

        Cancion.Id = Guid.NewGuid();
        await _svc.CrearAsync(Cancion);
        return RedirectToPage("Index");
    }
}
}