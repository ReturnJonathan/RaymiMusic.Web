using RaymiMusic.Modelos;
using RaymiMusic.Modelos.DTOs.Cuenta;

namespace RaymiMusic.MVC.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly HttpClient _http;
        public CuentaService(IHttpClientFactory httpFactory)
            => _http = httpFactory.CreateClient("RaymiMusicApi");


        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var resp = await _http.PostAsJsonAsync("api/Cuenta/login", loginRequest);

            // Si la respuesta no es exitosa, verificamos el código de estado
            if (!resp.IsSuccessStatusCode)
            {
                // Si el código de estado es 401, lanzamos un mensaje específico
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new ApplicationException("Correo o contraseña incorrectos.");
                }

                // Si no es un error 401, lanza una excepción genérica
                resp.EnsureSuccessStatusCode();
            }

            // Si la respuesta es exitosa, procesamos la respuesta
            return await resp.Content.ReadFromJsonAsync<LoginResponse>()
                   ?? throw new ApplicationException("Error al procesar la respuesta de login.");

        }

        async Task ICuentaService.SolicitarRecuperacion(RecuperacionRequest recuperacionRequest)
        {
            var resp = await _http.PostAsJsonAsync("api/Cuenta/solicitar-recuperacion", recuperacionRequest);

            // Si la respuesta no es exitosa, lanzamos una excepción
            if (!resp.IsSuccessStatusCode)
            {
                // Si el código de estado es 404, lanzamos un mensaje específico
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new ApplicationException("No se encontró un usuario con ese correo.");
                }

                // Si no es un error 404, lanzamos una excepción genérica
                resp.EnsureSuccessStatusCode();
            }

        }

        async Task ICuentaService.ValidarRecuperacion(ValidarRecuperacionRequest validarRecuperacionRequest)
        {
            var resp = await _http.PostAsJsonAsync("api/Cuenta/validar-recuperacion", validarRecuperacionRequest);

            // Si la respuesta no es exitosa, lanzamos una excepción
            if (!resp.IsSuccessStatusCode)
            {
                // Si el código de estado es 401, lanzamos un mensaje específico
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new ApplicationException("Código de recuperación inválido o expirado.");
                }

                // Si no es un error 401, lanzamos una excepción genérica
                resp.EnsureSuccessStatusCode();
            }

            // Si la respuesta es exitosa, no se espera un retorno específico
        }
    }
}

