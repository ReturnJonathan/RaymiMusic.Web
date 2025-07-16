using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RaymiMusic.Modelos;
using RaymiMusic.AppWeb.Models.Auth;
using System.Security.Claims;
using System.Text;
using RaymiMusic.Api.Data;
using System.Security.Cryptography;

namespace RaymiMusic.AppWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _ctx;

        public AccountController(AppDbContext ctx) => _ctx = ctx;

        /* ---------- REGISTRO ---------- */

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (_ctx.Usuarios.Any(u => u.Correo == vm.Email))
            {
                ModelState.AddModelError("", "El correo ya está registrado");
                return View(vm);
            }

            // Recuperar el GUID del plan Free
            var planFreeId = _ctx.Planes
                                 .Where(p => p.Nombre == "Free")
                                 .Select(p => p.Id)
                                 .First();          // existe gracias a la semilla o a tus datos

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Correo = vm.Email,
                HashContrasena = Hash(vm.Password),
                Rol = Roles.Free,
                PlanSuscripcionId = planFreeId
            };

            _ctx.Usuarios.Add(usuario);
            _ctx.SaveChanges();

            return RedirectToAction(nameof(Login));
        }



        /* ---------- LOGIN ---------- */

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var usuario = _ctx.Usuarios.FirstOrDefault(u => u.Correo == vm.Email);

            if (usuario == null || !Verify(vm.Password, usuario.HashContrasena))
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View(vm);
            }

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name,          usuario.Correo),
            new Claim(ClaimTypes.Role,          usuario.Rol)
        };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,           // “Recuérdame”
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                });

            return RedirectToAction("Index", "Home");
        }

        /* ---------- LOGOUT ---------- */

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        /* ---------- Helpers ---------- */

        private static string Hash(string plain)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
            return Convert.ToBase64String(bytes);
        }

        /* ---------- REGISTRO ARTISTA (GET) ---------- */
        [HttpGet]
        public IActionResult RegisterArtist() => View();

        /* ---------- REGISTRO ARTISTA (POST) ---------- */
        [HttpPost]
        public IActionResult RegisterArtist(RegisterArtistVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (_ctx.Usuarios.Any(u => u.Correo == vm.Email))
            {
                ModelState.AddModelError("", "El correo ya está registrado");
                return View(vm);
            }

            /* 1) Id del plan Free */
            var planFreeId = _ctx.Planes
                                 .Where(p => p.Nombre == "Free")
                                 .Select(p => p.Id)
                                 .First();

            /* 2) Crear usuario con rol ARTISTA */
            var usuarioId = Guid.NewGuid();
            var usuario = new Usuario
            {
                Id = usuarioId,
                Correo = vm.Email,
                HashContrasena = Hash(vm.Password),
                Rol = Roles.Artista,
                PlanSuscripcionId = planFreeId
            };
            _ctx.Usuarios.Add(usuario);

            /* 3) Crear registro en Artistas */
            var artista = new Artista
            {
                Id = Guid.NewGuid(),
                NombreArtistico = vm.NombreArtistico,
                Biografia = vm.Biografia,
                UrlFotoPerfil = vm.UrlFotoPerfil,
                UrlFotoPortada = vm.UrlFotoPortada
                // Si luego quieres relacionar Artista↔Usuario, añade la FK aquí
            };
            _ctx.Artistas.Add(artista);

            _ctx.SaveChanges();

            return RedirectToAction(nameof(Login));
        }


        private static bool Verify(string plain, string hashed) => Hash(plain) == hashed;
    }
}
