using Microsoft.EntityFrameworkCore;
using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
        public byte[]? Photo { get; set; } = [];
        [Range(1, 5)]
        public int Rate { get; set; }
        public string? AppUserId { get; set; }
        public string? AppUserName { get; set; }
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public List<ReviewCommentDto> Comments { get; set; } = new List<ReviewCommentDto>();
        public bool isYours { get; set; } = false;

    }
}
