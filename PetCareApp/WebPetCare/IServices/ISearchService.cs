using static System.Net.WebRequestMethods;
using System.Text.Json;
using PetCareApp.Models;
using WebPetCare.Services;

namespace WebPetCare.IServices
{
    public interface ISearchService
    {
        public Task<Result<List<GeoName>>> GetCityNames(string countryName);
        
    }


   
}
