using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class AddContactsDto
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
