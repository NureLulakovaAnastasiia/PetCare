using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class PortfolioDto
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Text { get; set; }
        public byte[] Photo { get; set; } = [];
    }
}
