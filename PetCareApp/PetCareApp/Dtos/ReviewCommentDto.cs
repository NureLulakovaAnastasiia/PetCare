using Microsoft.EntityFrameworkCore;
using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class ReviewCommentDto
    {
        public int Id { get; set; } = 0;
        [StringLength(200)]
        public string Text { get; set; } = string.Empty;
        public string? AppUserId { get; set; }
        public string? AppUserName { get; set; }
        public int? ReviewId { get; set; }
        public bool isYours { get; set; } = false;

    }
}
