using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public string Client { get; set; }

        [Display(Name = "Project Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Budget { get; set; }

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Started;

        // Foreign key
        [Required]
        public int UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public enum ProjectStatus
    {
        Started,
        Completed
    }
}
