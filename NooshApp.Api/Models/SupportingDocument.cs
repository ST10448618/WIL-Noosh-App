using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshApp.Api.Models
{
    public class SupportingDocument
    {
        [Key]
        public int Id { get; set; }
        //Attaches a supporting document to each job application.
        //One job application can have multiple supporting documents.
        [Required]
        public int JobApplicationId { get; set; }

        [ForeignKey(nameof(JobApplicationId))]
        public JobApplication? JobApplication { get; set; }

        [Required, MaxLength(300)]
        public string FilePath { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string OriginalFileName { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}