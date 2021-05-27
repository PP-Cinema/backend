using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Article
    {
        [Key] public int Id { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Abstract { get; set; }
        [Required] public string FilePath { get; set; }
    }
}