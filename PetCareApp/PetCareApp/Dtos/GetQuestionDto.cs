namespace PetCareApp.Dtos
{
    public class GetQuestionDto : IQuestionDto<GetAnswerDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasAnswerWithFixedTime { get; set; }
        public List<GetAnswerDto> Answers { get; set; } = new List<GetAnswerDto>();
    }
}
