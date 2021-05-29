using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class RefreshDto
    {
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}