using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.AppWeb.Models.Auth;

public class RegisterArtistVM
{
    /* Credenciales */
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    /* Datos del artista */
    [Required]
    public string NombreArtistico { get; set; } = string.Empty;

    public string? Biografia { get; set; }
    public string? UrlFotoPerfil { get; set; }
    public string? UrlFotoPortada { get; set; }
}
