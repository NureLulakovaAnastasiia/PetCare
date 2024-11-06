using Microsoft.JSInterop;
using PetCareApp.Dtos;
using PetCareApp.Models;
using System.Net;
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

        public async Task<string> AddService(AddServiceDto serviceDto, List<AddQuestionDto> questions)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(serviceDto, options);
            httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string error = string.Empty;
            try
            {
                string fullUrl = $"{_apiUrl}/api/Service/addService";

                HttpResponseMessage response = await httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(result, out int id))
                    {
                        if (id != 0)
                        {
                            if (questions != null && questions.Count > 0)
                            {
                                res = await AddQuestionary(questions, id);
                            }
                            return res;
                        }
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return "Unauthorized";
                }
                
                res = await response.Content.ReadAsStringAsync();
                res = error;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        private async Task<string> AddQuestionary(List<AddQuestionDto> questions, int serviceId)
        {
            var res = "";
            
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(questions, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
            string error = string.Empty;
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/addQuestionary?serviceId={serviceId}";

                HttpResponseMessage response = await httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(result, out int num))
                    {
                        if (num > 0)
                        {
                            return res;
                        }
                    }
                }

                res = await response.Content.ReadAsStringAsync();
                res = error;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<string> DeleteService(int serviceId)
        {
            var res = "";
            
            httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
            try
            {
                string fullUrl = $"{_apiUrl}/api/Service/deleteService?serviceId={serviceId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return "Unauthorized";
                }

                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<string> ChangeServiceVisibility(int serviceId)
        {
            var res = "";

            httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
            try
            {
                string fullUrl = $"{_apiUrl}/api/Service/changeServiceVisibility?serviceId={serviceId}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return "Unauthorized";
                }

                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<string> UpdateQuestionary(List<UpdateQuestionDto> questionaryDto)
        {
            var res = "error";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(questionaryDto, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/updateQuestionary";

                HttpResponseMessage response = await _httpClient.PutAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return "";

                }
                else
                {
                    res = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<Result<List<UpdateQuestionDto>>> GetQuestionary(int serviceId)
        {
            var res = new Result<List<UpdateQuestionDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/getQuestionary?serviceId={serviceId}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<UpdateQuestionDto>>(result, options);
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
    }
}
