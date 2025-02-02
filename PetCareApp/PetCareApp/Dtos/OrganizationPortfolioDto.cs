using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class OrganizationPortfolioDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public byte[] Photo { get; set; } = [];
        public string masterName { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
    }
}
