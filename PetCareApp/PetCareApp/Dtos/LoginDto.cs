using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class PasswordChange
    {
        public string NewPassword { get; set; } = string.Empty;
        public string NewPasswordSubmit { get; set; } = string.Empty;
    }
}
