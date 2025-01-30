using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(300)]
        public string Description { get; set; } = string.Empty;
        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
        public List<OrganizationEmployee> OrganizationEmployees { get; set; } = new List<OrganizationEmployee>();
        public List<OrganizationPorfolio> Portfolios { get; set; } = new List<OrganizationPorfolio>(); //masters portfolios
    }
}
