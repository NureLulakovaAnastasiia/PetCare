using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class RequestToOrganization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Status { get; set; } = "New";  //Rejected, Accepted

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }    
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
