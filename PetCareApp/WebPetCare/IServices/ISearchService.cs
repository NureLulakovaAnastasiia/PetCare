﻿using static System.Net.WebRequestMethods;
using System.Text.Json;
using PetCareApp.Models;
using WebPetCare.Services;
using PetCareApp.Dtos;

namespace WebPetCare.IServices
{
    public interface ISearchService
    {
        public Task<Result<List<CityDto>>> GetCityNames(int countryId, string localization = "en");
        
        public Task<List<GetServiceDto>> GetFilteredData(FiltersModel filters);

        public Task<List<ContactsDto>> GetServicesContacts(List<string> userIds);

        public Task<List<Tag>> GetAllTags();
    }


   
}
