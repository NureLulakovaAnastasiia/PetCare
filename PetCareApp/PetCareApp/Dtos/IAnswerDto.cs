namespace PetCareApp.Dtos
{
    public interface IAnswerDto
    {
        public string Text { get; set; }
        public byte[] Photo { get; set; } 
        public int Time { get; set; }
        public bool IsTimeMinimum { get; set; }
        public bool IsTimeMaximum { get; set; } 
        public bool IsTimeFixed { get; set; } 
    }
}
