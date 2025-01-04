using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class PetDto
    {
        public int Id { get; set; } = 0;

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [StringLength(100)]
        public string Breed { get; set; } = string.Empty;
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;
        public byte[] Photo { get; set; } = [];



        public string? AppUserId { get; set; }
    }
}
