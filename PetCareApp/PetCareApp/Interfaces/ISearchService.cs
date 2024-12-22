using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface ISearchService
    {
        public List<GetServiceDto>? FindServices(FiltersModel filters);

        public Task<string> AddCities(string countryCode, string localiation);

        public List<CityDto> GetCitiesList(int countryId, string localization = "en");
    }
}
