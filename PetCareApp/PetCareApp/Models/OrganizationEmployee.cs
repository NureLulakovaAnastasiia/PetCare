using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class OrganizationEmployee
    {
        [Key]
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.ClientCascade)]
        public AppUser? AppUser { get; set; }
        public int? OrganizationId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Organization? Organization { get; set; }

    }
}
