using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class RecordDto
    {
        [StringLength(200)]
        public string Description { get; set; } //data from questionary
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [StringLength(200)]
        public string? Comment { get; set; }
        [StringLength(20)]
        public string Status { get; set; } = "Created";
        public int ServiceId { get; set; }
    }
}
