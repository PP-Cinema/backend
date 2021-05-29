using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class AuthenticationDto
    {
        [Required] public string AccessToken { get; set; }
        [Required] public string RefreshToken { get; set; }
        [Required] public string Role { get; set; }
    }
}