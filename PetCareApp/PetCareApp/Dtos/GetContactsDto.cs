using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetContactsDto
    {
        public int Id { get; set; }
        public string Address { get; set; } 
        public string City { get; set; } 
        public string Phone { get; set; } 
    }
}
