using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RaymiMusic.AppWeb.Controllers
{
    [AllowAnonymous]
    public class TestEmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public TestEmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        // GET: /TestEmail/Send
        public async Task<IActionResult> Send()
        {
            // Cambia este email por uno tuyo para probar
            var destinatario = "llorestalinjn@gmail.com";
            await _emailSender.SendEmailAsync(
                destinatario,
                "Prueba de correo RaymiMusic",
                "<p>¡Hola! Este es un correo de prueba desde RaymiMusic.AppWeb.</p>"
            );

            return Content($"Correo de prueba enviado a {destinatario}");
        }
    }
}
