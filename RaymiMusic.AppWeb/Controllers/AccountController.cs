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

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Correo = vm.Email,
                HashContrasena = Hash(vm.Password),
                Rol = Roles.Free        // todos inician como free
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

        private static bool Verify(string plain, string hashed) => Hash(plain) == hashed;
    }
}
