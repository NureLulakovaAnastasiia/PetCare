namespace PetCareApp.Dtos
{
    public class QuestionDto
    {
        public string Name { get; set; }
        public bool HasAnswerWithFixedTime { get; set; }
        public AnswerDto Answer { get; set; }
    }
}
