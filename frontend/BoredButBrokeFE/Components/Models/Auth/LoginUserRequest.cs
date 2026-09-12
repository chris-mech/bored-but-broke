using System.ComponentModel.DataAnnotations;

namespace BoredButBrokeFE.Components.Models.Auth
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Email must not be empty")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password must not be empty")]
        public required string Password { get; set; }
    }
}
