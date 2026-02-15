using System.ComponentModel.DataAnnotations;

namespace IncidentApiRimel.Models
{
    public class Incident
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage ="erreur")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "erreur")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Severity { get; set; } = string.Empty;

        
        public DateTime CreatedAt { get; set; }

        
        public string Status { get; set; } = string.Empty;
    }
}
