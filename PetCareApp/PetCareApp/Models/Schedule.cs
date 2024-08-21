using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public int? DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
    }
}
