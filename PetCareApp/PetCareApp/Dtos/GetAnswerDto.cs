namespace PetCareApp.Dtos
{
    public class GetAnswerDto : IAnswerDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public byte[] Photo { get; set ; } = new byte[0];
        public int Time { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsTimeMinimum { get; set; } = false;
        public bool IsTimeMaximum { get; set; } = false;
        public bool IsTimeFixed { get; set; } = false;

    }
}