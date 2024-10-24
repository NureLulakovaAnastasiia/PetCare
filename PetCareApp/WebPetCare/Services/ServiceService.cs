using Microsoft.JSInterop;
using PetCareApp.Dtos;
using PetCareApp.Models;
using System.Text.Json;
using WebPetCare.IServices;

namespace WebPetCare.Services
{
    public class ServiceService: BaseService, IServiceService
    {
        private HttpClient httpClient { get; set; }
        private IJSRuntime jsRuntime;
        public ServiceService(HttpClient httpClientbase, IConfiguration configuration, IJSRuntime runtime) :
            base(httpClientbase, configuration, runtime)
        {
            httpClient = httpClientbase;
            jsRuntime = runtime;
        }
        public async Task<Result<GetServiceDto>> getServiceDetails(int serviceId)
        {
            var res = new Result<GetServiceDto>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Service/getServiceDetails?serviceId={serviceId}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<GetServiceDto>(result, options);
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

        public async Task<string> UpdateService(GetServiceDto service)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(service, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string error = string.Empty;
            try
            {
                string fullUrl = $"{_apiUrl}/api/Service/updateService";

                HttpResponseMessage response = await _httpClient.PatchAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                }
                else
                {
                    res = await response.Content.ReadAsStringAsync();
                }

                res = error;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
    }
}
