using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class BreakDto
    {
        public int Id { get; set; } = 0;
        [Range(0, 6)]
        public int? DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
