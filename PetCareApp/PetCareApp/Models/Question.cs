using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
        public bool HasAnswerWithFixedTime { get; set; } = false;

        public int? ServiceId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Service? Service { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
