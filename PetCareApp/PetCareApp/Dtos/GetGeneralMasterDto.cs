using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetGeneralMasterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public GetContactsDto Contacts { get; set; } = new GetContactsDto();
        public List<GetServiceDto> Services { get; set; } = new List<GetServiceDto>();

        public GetGeneralMasterDto() { }
        public GetGeneralMasterDto(GetGeneralMasterDto dto) { 
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.Email = dto.Email;
        }
    }
}
