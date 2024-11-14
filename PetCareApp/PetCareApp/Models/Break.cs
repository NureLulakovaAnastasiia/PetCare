using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Break
    {
        [Key]
        public int Id { get; set; }
        [Range(0,6)]
        public int? DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
    }
}
