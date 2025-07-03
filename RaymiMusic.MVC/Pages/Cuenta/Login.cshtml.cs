using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RaymiMusic.Api.Data;
using RaymiMusic.Data;
using RaymiMusic.Modelos;
using RaymiMusic.Modelos.DTOs.Cuenta;
using RaymiMusic.MVC.Services;
using System;

namespace RaymiMusic.MVC.Pages.Cuenta
{
    public class LoginModel : PageModel
    {
        private readonly ICuentaService _cuentaService;

        public LoginModel(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        [BindProperty]
        public string Correo { get; set; } = null!;

        [BindProperty]
        public string Contrasena { get; set; } = null!;

        public string? ErrorMensaje { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            // Crear el objeto LoginRequest con los valores de Correo y Contraseña
            var loginRequest = new LoginRequest
            {
                Correo = Correo,
                Contrasena = Contrasena
            };

            try
            {
                // Llamada al servicio de login
                var loginResponse = await _cuentaService.Login(loginRequest);

                // Si el login es exitoso, guardamos los valores en la sesión
                HttpContext.Session.SetString("UsuarioId", loginResponse.UsuarioId.ToString());
                HttpContext.Session.SetString("Correo", loginResponse.Correo);
                HttpContext.Session.SetString("Rol", loginResponse.Rol);

                // Redirigimos al usuario a la página de inicio
                return RedirectToPage("/Index");
            }
            catch (ApplicationException ex)
            {
                // Si ocurre un error, se muestra el mensaje de error
                ErrorMensaje = ex.Message;
                return Page();
            }
        }
    }
}
