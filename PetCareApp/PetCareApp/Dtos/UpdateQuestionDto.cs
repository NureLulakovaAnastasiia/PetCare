using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class UpdateQuestionDto: IQuestionDto<UpdateAnswerDto>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool HasAnswerWithFixedTime { get; set; } = false;
        [Required]
        public int ServiceId { get; set; }
        [Required]
        public List<UpdateAnswerDto> Answers { get; set; }
    }
}
