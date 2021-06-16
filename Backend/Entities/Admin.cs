using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    public class Admin
    {
        [Key]
        public int EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")] 
        public Employee Employee { get; set; }
    }
}