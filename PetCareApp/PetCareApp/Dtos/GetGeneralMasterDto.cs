using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class GetGeneralMasterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public GetContactsDto Contacts { get; set; }
        public List<GetServiceDto> Services { get; set; } = new List<GetServiceDto>();

    }
}
