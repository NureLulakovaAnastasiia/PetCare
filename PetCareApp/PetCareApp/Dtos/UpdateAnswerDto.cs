using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class UpdateAnswerDto: IAnswerDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        public byte[]? Photo { get; set; } = null;
        public int Time { get; set; } = 0;  //+- time or if fixed == true => fixed end time
        public bool IsTimeMinimum { get; set; } = false;
        public bool IsTimeMaximum { get; set; } = false;
        public bool IsTimeFixed { get; set; } = false; //also must be true when isMin or isMax selected
    }
}
