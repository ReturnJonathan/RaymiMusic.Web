using RaymiMusic.Modelos.DTOs.Cuenta;

namespace RaymiMusic.MVC.Services
{
    public interface ICuentaService
    {
        public Task<LoginResponse> Login(LoginRequest loginRequest);
        public Task SolicitarRecuperacion(RecuperacionRequest recuperacionRequest);
        public Task ValidarRecuperacion(ValidarRecuperacionRequest validarRecuperacionRequest);
    }
}
