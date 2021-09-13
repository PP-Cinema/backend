using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Backend.DTO
{
    public class ReservationDto
    {
        [Required(ErrorMessage = "E-mail is required")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Number of normal tickets is required")]
        public int NormalTickets { get; set; }
        
        [Required(ErrorMessage = "Number of discounted tickets is required")]
        public int DiscountedTickets { get; set; }
        
        [Required(ErrorMessage = "FirstName is required")] 
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Lastname is required")] 
        public string LastName { get; set; }
        
        public string Remarks { get; set; }
        
        public int PerformanceId { get; set; }
        
        public string Seats { get; set; }
        
    }
}