using PetCareApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class MasterDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? OrgId { get; set; }
        public string OrgName { get; set; } = string.Empty;
        public ContactsDto Contacts { get; set; } = new ContactsDto();
        public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();
        public List<PortfolioDto> Portfolios { get; set; } = new List<PortfolioDto>();


    }
}
