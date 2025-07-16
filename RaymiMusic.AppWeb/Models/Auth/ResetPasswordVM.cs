using System.ComponentModel.DataAnnotations;

namespace RaymiMusic.AppWeb.Models.Auth
{
    public class ResetPasswordVM
    {
        public string Token { get; set; }
        [Required, MinLength(6)]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }

}
