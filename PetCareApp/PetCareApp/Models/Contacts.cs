using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareApp.Models
{
    public class Contacts
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public int CityId { get; set; }

        public string Phone { get; set; } = string.Empty;
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public City? City { get; set; }

        public Location? Location { get; set; }

    }
}
