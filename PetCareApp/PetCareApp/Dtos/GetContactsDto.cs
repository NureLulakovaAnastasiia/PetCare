using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetContactsDto
    {
        public int Id { get; set; } = 0;
        public string Address { get; set; }  = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public LocationDto? Location { get; set; }
        public GetContactsDto()
        {

        }

        public GetContactsDto(GetContactsDto dto)
        {
            this.Id = dto.Id;
            this.Address = dto.Address;
            this.City = dto.City;
            this.Phone = dto.Phone;
            this.Location = dto.Location;
        }

        
    }
}
