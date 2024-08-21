using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class AppUser: IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string? Phone {  get; set; } = string.Empty;
    }
}
