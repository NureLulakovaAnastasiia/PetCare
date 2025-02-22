using PetCareApp.Dtos;

namespace PetCareApp.Models
{
    public class OrganizationInfo
    {
        public int Id { get; set; } = 0;
        public string? AppUserId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Photo { get; set; }

    }
}
