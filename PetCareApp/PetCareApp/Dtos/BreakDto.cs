using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class BreakDto
    {
        public int Id { get; set; } = 0;
        [Range(0, 6)]
        public int? DayOfWeek { get; set; } = null;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        public static bool HasSameTimeDate(BreakDto a, BreakDto b)
        {
            if(a.EndDate == b.EndDate && a.StartDate == b.StartDate 
                && a.StartTime == b.StartTime && a.EndTime == b.EndTime) return true;
            else return false;
        }

     
    }
}
