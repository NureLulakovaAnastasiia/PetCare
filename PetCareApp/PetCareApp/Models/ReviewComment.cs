using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class ReviewComment
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string Text { get; set; } = string.Empty;
        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
        public int? ReviewId { get; set; }
        [DeleteBehavior(DeleteBehavior.ClientCascade)]
        public Review? Review { get; set; }
    }
}
