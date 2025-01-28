using Microsoft.EntityFrameworkCore;
using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class RecordInfoDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RecordCancelReason {  get; set; } = string.Empty;
        public string? AppUserId { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int? PetId { get; set; }
        public string PetName { get; set; } = string.Empty;

        public bool IsReviewed { get; set; } = false;
    }
}
