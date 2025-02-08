using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string Text { get; set; } = string.Empty;
        public byte[]? Photo { get; set; } = null;
        public int Time { get; set; } = 0;
        public bool IsTimeMinimum { get; set; } = false;
        public bool IsTimeMaximum { get; set; } = false;
        public bool IsTimeFixed { get; set; } = false;

        public int? QuestionId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Question? Question { get; set; }
    }
}
