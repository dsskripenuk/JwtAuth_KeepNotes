using System.ComponentModel.DataAnnotations;

namespace Drivers.Api.Models
{
    public class Note
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? UserId { get; set; }
    }
}
