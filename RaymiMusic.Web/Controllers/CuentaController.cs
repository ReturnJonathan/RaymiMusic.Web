using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using RaymiMusic.Api.Data;
using RaymiMusic.Modelos;
using RaymiMusic.Modelos.DTOs.Cuenta;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace RaymiMusic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {

        private readonly AppDbContext _context;

        public CuentaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == request.Correo);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.HashContrasena))
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }
            // Aquí podrías generar un token JWT o establecer una sesión
           var loginResponse = new LoginResponse
           {
               UsuarioId = usuario.Id,
               Correo = usuario.Correo,
               Rol = usuario.Rol
           };
            return Ok(loginResponse);
        }
        [HttpPost("solicitar-recuperacion")]
        public async Task<IActionResult> SolicitarRecuperacion([FromBody] RecuperacionRequest request)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == request.Correo);
            if (usuario == null)
            {
                return NotFound("No se encontró un usuario con ese correo.");
            }

            // Eliminar cualquier código de recuperación anterior para este usuario
            var recuperacionExistente = _context.Recuperaciones.FirstOrDefault(r => r.UsuarioId == usuario.Id);
            if (recuperacionExistente != null)
            {
                _context.Recuperaciones.Remove(recuperacionExistente);
                await _context.SaveChangesAsync();
            }

            // Generar un código de recuperación único
            var codigoRecuperacion = GenerateRecoveryCode();

            // Guardar el código de recuperación en la base de datos con un tiempo de expiración
            var recuperacion = new Recuperaciones
            {
                UsuarioId = usuario.Id,
                Codigo = codigoRecuperacion,
                Expiracion = DateTime.Now.AddMinutes(15)  // El código expirará en 15 minutos
            };
            _context.Recuperaciones.Add(recuperacion);
            await _context.SaveChangesAsync();

            // Enviar el código de recuperación por correo
            await SendRecoveryEmail(usuario.Correo, codigoRecuperacion);

            return Ok("El código de recuperación ha sido enviado a su correo.");
        }


        // Método para generar un código de recuperación único
        private string GenerateRecoveryCode()
        {
            var rng = new Random();
            return rng.Next(100000, 999999).ToString(); // Genera un código de 6 dígitos
        }

        // Método para enviar un correo con el código de recuperación

        private async Task SendRecoveryEmail(string correo, string codigo)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Soporte", "RaymiMusic@outlook.com"));
            mensaje.To.Add(new MailboxAddress("", correo));
            mensaje.Subject = "Código de Recuperación de Contraseña";
            mensaje.Body = new TextPart("plain")
            {
                Text = $"Su código de recuperación es: {codigo}"
            };

            using (var cliente = new MailKit.Net.Smtp.SmtpClient())
            {
                // Desactivar la validación de certificados SSL (solo para depuración)
                cliente.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                await cliente.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                await cliente.AuthenticateAsync("RaymiMusic@outlook.com", "dydemduuzvljfkyo");

                await cliente.SendAsync(mensaje);
                await cliente.DisconnectAsync(true);
            }
        }

        [HttpPost("validar-recuperacion")]
        public IActionResult ValidarRecuperacion([FromBody] ValidarRecuperacionRequest request)
        {
            // Buscar el código de recuperación en la base de datos
            var recuperacion = _context.Recuperaciones
                                       .FirstOrDefault(r => r.Codigo == request.Codigo);

            // Si no se encuentra el código de recuperación
            if (recuperacion == null)
            {
                return Unauthorized("Código de recuperación inválido.");
            }

            // Verificar si el código ha expirado
            if (recuperacion.Expiracion < DateTime.Now)
            {
                return Unauthorized("El código de recuperación ha expirado.");
            }

            // Buscar al usuario que corresponde al código de recuperación
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == recuperacion.UsuarioId);

            // Si no se encuentra al usuario
            if (usuario == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            // Actualizar la contraseña del usuario con la nueva contraseña
            usuario.HashContrasena = BCrypt.Net.BCrypt.HashPassword(request.NuevaContraseña);

            // Eliminar el código de recuperación (opcional, ya que el código solo se usa una vez)
            _context.Recuperaciones.Remove(recuperacion);

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return Ok("Contraseña cambiada exitosamente.");
        }



    }
}
