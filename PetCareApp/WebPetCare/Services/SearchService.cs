using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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


        public async Task<Result<List<GeoName>>> GetCityNames(string countryCode)
        {
            var res = new Result<List<GeoName>>();
            var localization = "en";
            //add getting localization

            try
            {
            
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

    public class GeoName
    {
        public int GeonameId { get; set; }

        public string Name { get; set; }

        public string AdminName1 { get; set; }
    }
}
