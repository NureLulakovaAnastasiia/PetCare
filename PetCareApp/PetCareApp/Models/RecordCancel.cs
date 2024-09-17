using Microsoft.EntityFrameworkCore;

namespace PetCareApp.Models
{
    public class RecordCancel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Reason { get; set; } = string.Empty;
        public string? AppUserId { get; set; } //who canceled?
        public AppUser? AppUser { get; set; }
        public int? RecordId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Record? Record { get; set; }
    }
}
