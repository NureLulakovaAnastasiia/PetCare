using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class AddPortfolioDto
    {
        [StringLength(500)]
        public string Text { get; set; }
        public byte[] Photo { get; set; } = [];
    }
}
