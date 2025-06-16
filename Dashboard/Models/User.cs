using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        // Navigation property — one user has many projects
        public List<Project> Projects { get; set; }
    }
}
