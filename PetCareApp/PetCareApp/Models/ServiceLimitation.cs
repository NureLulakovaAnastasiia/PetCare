using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class ServiceLimitation
    {
        [Key]
        public int Id { get; set; }
        [Range(0, 6)]
        public int? DayOfWeek { get; set; }
        public DateTime Date { get; set; }

        public int? ServiceId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Service? Service { get; set; }
    }
}
