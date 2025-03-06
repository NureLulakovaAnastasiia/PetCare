using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetPetDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
