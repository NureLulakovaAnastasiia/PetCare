using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Portfolio
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
        public byte[] Photo { get; set; } = [];
        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
       
    }
}
