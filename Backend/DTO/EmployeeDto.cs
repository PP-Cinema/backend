using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required] public bool IsAdmin { get; set; }
    }
}