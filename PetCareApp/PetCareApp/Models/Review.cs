using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
        public byte[] Photo { get; set; } = [];
        [Range(1, 5)]
        public int Rate { get; set; }
        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.ClientCascade)]
        public AppUser? AppUser { get; set; }
        public int? ServiceId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Service? Service { get; set; }
        public List<ReviewComment> Reviews { get; set; } = new List<ReviewComment>();
    }
}
