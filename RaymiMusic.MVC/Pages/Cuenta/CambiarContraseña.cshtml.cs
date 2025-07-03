using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.Modelos.DTOs.Cuenta;
using RaymiMusic.MVC.Services;

namespace RaymiMusic.MVC.Pages.Cuenta
{
    public class CambiarContraseñaModel : PageModel
    {
        private readonly ICuentaService _cuentaService;

        public CambiarContraseñaModel(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        [BindProperty]
        public string NuevaContrasena { get; set; } = null!;

        [BindProperty]
        public string ConfirmarContrasena { get; set; } = null!;

        [BindProperty]
        public string CodigoRecuperacion { get; set; } = null!; // Nuevo campo

        public string? ErrorMensaje { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(NuevaContrasena) || string.IsNullOrEmpty(ConfirmarContrasena) || string.IsNullOrEmpty(CodigoRecuperacion))
            {
                ErrorMensaje = "El código de recuperación y las contraseñas son obligatorios.";
                return Page();
            }

            if (NuevaContrasena != ConfirmarContrasena)
            {
                ErrorMensaje = "Las contraseñas no coinciden.";
                return Page();
            }

            try
            {
                // Cambia la contraseña usando el código de recuperación
                var request = new ValidarRecuperacionRequest
                {
                    NuevaContraseña = NuevaContrasena,
                    Codigo = CodigoRecuperacion
                };

                await _cuentaService.ValidarRecuperacion(request);  // Pasa el objeto con el código y la nueva contraseña
                return RedirectToPage("/Cuenta/Login");
            }
            catch (ApplicationException ex)
            {
                ErrorMensaje = ex.Message;
                return Page();
            }
        }
    }


}

