using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Contacts
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        [StringLength(200)]
        public string Phone { get; set; } = string.Empty;
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
