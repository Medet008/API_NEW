using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace API_NEW.Models.Dto
{
    public class MenuItemCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpecialTag { get; set; }
        public string Category { get; set; }
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public IFormFile File { get; set; }
    }
}
