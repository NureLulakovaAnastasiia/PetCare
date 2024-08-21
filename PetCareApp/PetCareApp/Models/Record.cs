using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Record
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [StringLength(200)]
        public string? Comment { get; set; } = string.Empty;
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;
        public RecordCancel? RecordCancel { get; set; } = null;

        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.ClientCascade)]
        public AppUser? AppUser { get; set; }
        public int? ServiceId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Service? Service { get; set; }
        public int? PetId { get; set; }
        public Pet? Pet { get; set; }

    }
}
