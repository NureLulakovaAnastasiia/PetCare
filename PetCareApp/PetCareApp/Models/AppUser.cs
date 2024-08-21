using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class AppUser: IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone {  get; set; } = string.Empty;

        public List<Pet> Pets { get; set; } = new List<Pet>();
        public Contacts Contacts { get; set; } = new Contacts();
        public List<Service> Services { get; set; } = new List<Service>();
        public List<Break> Breaks { get; set; } = new List<Break>();
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
