using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [StringLength(100)]
        public string Breed { get; set; } = string.Empty;
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;
        public byte[] Photo { get; set; } = [];


        
        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
        public List<Record> Records { get; set; } = new List<Record>();

    }
}
