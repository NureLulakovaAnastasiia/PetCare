using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PetCareApp.Dtos;
using PetCareApp.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebPetCare.IServices;
using static System.Net.WebRequestMethods;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace WebPetCare.Services
{
    public class SearchService : BaseService, ISearchService
    {

        private HttpClient httpClient { get; set; }
        private IJSRuntime jsRuntime;
        public SearchService(HttpClient httpClientbase, IConfiguration configuration, IJSRuntime runtime) :
            base(httpClientbase, configuration, runtime)
        {
            httpClient = httpClientbase;
            jsRuntime = runtime;
        }


        public async Task<Result<List<CityDto>>> GetCityNames(int countryId = 690791)
        {
            var res = new Result<List<CityDto>>();
            var localization = "en";
            //add getting localization

            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Search/getCitiesList?countryId={countryId}&localization={localization}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<CityDto>>(result, options);
                    if (data != null)
                    {
                        res.Data = data;
                    }
                }
                else
                {
                    res.ErrorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<List<GetServiceDto>> GetFilteredData(FiltersModel filters)
        {
            var res = new List<GetServiceDto>();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(filters, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Search/filters?all=false";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    
                    var data = JsonSerializer.Deserialize<List<GetServiceDto>>(result, options);
                    if (data != null)
                    {
                        return data;
                    }
                }
               
            }
            catch (Exception ex)
            {
               
            }
            return res;
        }

        public async Task<List<ContactsDto>> GetServicesContacts(List<string> userIds)
        {
            var res = new List<ContactsDto>();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(userIds, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Search/getServicesContacts";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    var data = JsonSerializer.Deserialize<List<ContactsDto>>(result, options);
                    if (data != null)
                    {
                        return data;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<List<Tag>> GetAllTags()
        {
            var res = new List<Tag>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Search/allTags";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<Tag>>(result, options);
                    if (data != null)
                    {
                        res = data;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return res;
        }
    }

    
}
