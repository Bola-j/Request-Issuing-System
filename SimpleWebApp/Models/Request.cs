using System.ComponentModel.DataAnnotations;

namespace SimpleWebApp.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        [Display(Name = "Request Type")]
        public string RequestType { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
