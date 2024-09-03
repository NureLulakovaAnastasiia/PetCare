using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public interface IQuestionDto<TAnswerDto> where TAnswerDto : IAnswerDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool HasAnswerWithFixedTime { get; set; }
        [Required]
        public List<TAnswerDto> Answers { get; set; }

    }
}
