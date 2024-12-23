using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Mappings;
using PetCareApp.Models;
using System.Net.Http;
using System.Text.Json;

namespace PetCareApp.Services
{
    public class SearchService : BaseService, ISearchService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;
        private HttpClient _httpClient;
        private const int maxData = 1000;
        public static string defaultLocalization = "en";
        public SearchService(UserManager<AppUser> userManager, IMapper mapper, ApplicationDBContext context,
           IHttpContextAccessor httpContextAccessor, HttpClient httpClient) : base(userManager, httpContextAccessor)
        {
            _mapper = mapper;
            _dbContext = context;
            _httpClient = httpClient;
        }

        public async Task<string> AddCities(string countryCode, string localization)
        {
            try
            {
                var cities = await GetAllCities(countryCode, localization);
                if (cities == null || cities.Count == 0)
                {
                    return "No data to add";
                }
                var country = _dbContext.Countries.FirstOrDefault(c => c.Id == cities[0].CountryId);
                var countryExists = country != null;
                var existing = _dbContext.Cities.ToList();
                if (existing != null)
                {
                    var newCities = cities.ExceptBy(existing.Select(e => e.Id), c => c.Id).ToList();
                    var citiesToUpdate = cities.IntersectBy(existing.Select(e => e.Id), c => c.Id).ToList();

                    foreach (var city in newCities)
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add(localization, city.LocalizedName);
                        if (countryExists)
                        {
                            city.Country = null;
                        }
                        var serialized = JsonSerializer.Serialize(dict);
                        if (serialized != null)
                        {
                            city.LocalizedName = serialized;
                        }
                        else
                        {
                            city.LocalizedName = "";
                        }
                    }


                    _dbContext.AddRange(newCities);
                    //_dbContext.SaveChanges();
                    //foreach (var c in citiesToUpdate)
                    //{
                    //    var exists = existing.FirstOrDefault(e => e.Id == c.Id);
                    //    if (exists != null)
                    //    {
                    //        var localized = JsonSerializer.Deserialize<Dictionary<string, string>>(exists.LocalizedName);
                    //        if (localized != null)
                    //        {
                    //            localized[localization] = c.LocalizedName;
                    //        }
                    //        c.LocalizedName = JsonSerializer.Serialize(localized);
                    //    }
                    //}

                    //_dbContext.UpdateRange(citiesToUpdate);
                    return _dbContext.SaveChanges().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
           // var countries = cities.Select
        }

        private async Task<List<City>?> GetAllCities(string countryCode, string localization)
        {
            var max = 0;
            var startRow = 5000;
            List<City> cities = new List<City>();
            do
            {
                string apiUrl = $"http://api.geonames.org/searchJSON?country={countryCode}&featureClass=P&startRow={startRow}&maxRows={maxData}{(localization == defaultLocalization ? "" : "&lang=" + localization)}&username=anas_tasi_ia_311003";
                var response = await _httpClient.GetStringAsync(apiUrl);
                try
                {
                    var data = JsonSerializer.Deserialize<GeoNamesResponseCountry>(response);
                    var gottenCities = JsonSerializer.Deserialize<GeoNamesResponseCity>(response).geonames;
                    var countries = data.geonames;
                    if(max == 0)
                    {
                        max = data.totalResultsCount;
                    }
                    countries = countries.OfType<Country>().DistinctBy(c => c.Id).ToList();
                    gottenCities = gottenCities.OfType<City>().DistinctBy(c => c.Id).ToList();
                    if (gottenCities != null)
                    {
                        foreach (var city in gottenCities)
                        {
                            var country = countries.FirstOrDefault(c => c.Id == city.CountryId);
                            city.Country = country;
                        }
                        cities.AddRange(gottenCities);
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                startRow += maxData;

            } while (startRow < max);
            
            
            return cities.DistinctBy(c => c.Id).ToList();
        }

        public List<GetServiceDto>? FindServices(FiltersModel filters)
        {
            var services = _dbContext.Services.Include(s => s.Reviews)
                    .Include(s => s.AppUser).ThenInclude(u => u.Contacts)
                    .Include(s => s.Tags)
                    .ToList();

            services.ForEach(s =>
            {
                s.Reviews.ForEach(r =>
                {
                    r.AppUser = null;
                    r.Service = null;
                });
            });
            if (services == null || services.Count == 0)
            {
                return null;
            }
            if (filters.City != null)
            {
                services = services
                    .Where(s => s.AppUser.Contacts.CityId == filters.City)
                    .ToList();
            }
            if (filters.Tags != null && filters.Tags.Count > 0)
            {
                services = services.Where(s => s.Tags != null && filters.Tags.All(t => s.Tags.Contains(t))).ToList();
            }

            if (filters.Rate != null)
            {
                services = services.Where(s => s.Rate >= filters.Rate).ToList();
            }
             if(filters.MinPrice != null && filters.MaxPrice != null)
            {
                services = services.Where(s => 
                    s.MinimumPrice >= filters.MinPrice &&
                    s.MaximumPrice <= filters.MaxPrice
                ).ToList();
            }else if(filters.MaxPrice != null)
            {
                services = services.Where(s => s.MaximumPrice <= filters.MaxPrice).ToList();
            }
            else if (filters.MinPrice != null)
            {
                services = services.Where(s => s.MinimumPrice >= filters.MinPrice).ToList();
            }

            
 
            var res = _mapper.Map<List<GetServiceDto>>(services);
            return res;
        }

        public List<CityDto> GetCitiesList(int countryId, string localization = "en")
        {
            var res = new List<CityDto>();
            var existingServiceCities = _dbContext.Contacts.Select(c => c.CityId).Distinct().ToList();
            var cities = _dbContext.Cities.Where(c => c.CountryId == countryId).ToList();
            if(existingServiceCities.Count > 0)
            {
                cities = cities.FindAll(c => existingServiceCities.Contains(c.Id)); 
            }
            foreach (var c in cities)
            {
                var dto = new CityDto
                {
                    CountryId = c.CountryId,
                    Id = c.Id,
                    AdminName = c.AdminRegion
                };
                var localized = JsonSerializer.Deserialize<Dictionary<string, string>>(c.LocalizedName);
                if (localized != null)
                {
                    dto.Name = localized[localization] ?? string.Empty;
                }
                if (!String.IsNullOrEmpty(dto.Name))
                {
                    res.Add(dto);
                }
            }
            return res;
        }

        public List<ContactsDto> GetServicesContacts(List<string> userIds)
        {
            var res = new List<ContactsDto> ();
            var contacts = _dbContext.Contacts.Include(c => c.Location)
                .Where(c=> userIds.Contains(c.AppUserId))
                .ToList();

            if (contacts == null || contacts.Count == 0)
            {
                return res;
            }
            foreach (var contact in contacts)
            {
                var tmp = ContactsMapping.MapContact(contact);
                if (tmp != null && tmp.Location != null)
                {
                    res.Add(tmp);
                }
            }
            
            return res;
        }

        public List<Tag> GetAllTags()
        {
           var res = _dbContext.Tags.Where(t => t.Id > 0).ToList();
            if(res == null)
            {
                return new List<Tag>();
            }

            return res;
        }
    }
}

public class GeoNamesResponseCity
{
    public int totalResultsCount { get; set; }
    public List<City> geonames { get; set; }
}
public class GeoNamesResponseCountry
{
    public int totalResultsCount { get; set; }
    public List<Country> geonames { get; set; }
}
