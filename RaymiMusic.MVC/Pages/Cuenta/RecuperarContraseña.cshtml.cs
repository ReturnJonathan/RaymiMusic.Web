using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaymiMusic.MVC.Services;
using RaymiMusic.Modelos.DTOs.Cuenta;
using System.Threading.Tasks;

namespace RaymiMusic.MVC.Pages.Cuenta
{
    public class RecuperarContraseñaModel : PageModel
    {
        private readonly ICuentaService _cuentaService;

        public RecuperarContraseñaModel(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        [BindProperty]
        public string Correo { get; set; } = null!;

        public string? ErrorMensaje { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Correo))
            {
                ErrorMensaje = "El correo es obligatorio.";
                return Page();
            }

            var recuperarRequest = new RecuperacionRequest
            {
                Correo = Correo
            };

            try
            {
                await _cuentaService.SolicitarRecuperacion(recuperarRequest);
                return RedirectToPage("/Cuenta/CambiarContraseña");  // Redirige a login tras la solicitud exitosa
            }
            catch (ApplicationException ex)
            {
                ErrorMensaje = ex.Message;
                return Page();
            }
        }
    }
}
