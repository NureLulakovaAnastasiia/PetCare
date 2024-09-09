namespace PetCareApp.Dtos
{
    public class AnswerDto
    {
        public string Text { get; set; }
        public int Time { get; set; } = 0;  //+- time or if fixed == true => fixed end time
        public bool IsTimeMinimum { get; set; } = false;
        public bool IsTimeMaximum { get; set; } = false;
        public bool IsTimeFixed { get; set; } = false;
    }
}
