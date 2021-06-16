using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class SearchDto
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string SearchString { get; set; }
    }
}