using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Employee
    {
        [Key] public int Id { get; set; }
        [Required] public string Login { get; set; }
        [Required] public string PasswordHash { get; set; }
    }
}