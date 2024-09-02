using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class AddQuestionDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool HasAnswerWithFixedTime { get; set; } = false;
        [Required]
        public List<AddAnswerDto> Answers { get; set; }
    }
}
