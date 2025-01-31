using Microsoft.JSInterop;
using PetCareApp.Dtos;
using PetCareApp.Models;
using System.Text.Json;
using WebPetCare.IServices;

namespace WebPetCare.Services
{
    public class OrganizationService: BaseService, IOrganizationService
    {
        private HttpClient httpClient { get; set; }
        private IJSRuntime jsRuntime;
        public OrganizationService(HttpClient httpClientbase, IConfiguration configuration, IJSRuntime runtime) :
            base(httpClientbase, configuration, runtime)
        {
            httpClient = httpClientbase;
            jsRuntime = runtime;
        }
        public async Task<string?> GetCurrentUserRole()
        {
            var res = await getCurrentRole();
            return res;
        }
        public async Task<Result<OrganizationInfo>> GetOrganizationInfo()
        {
            var res = new Result<OrganizationInfo>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Organization/getOrganizationData";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<OrganizationInfo>(result, options);
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

        public async Task<string> UpdateOrganizationInfo(OrganizationInfo organizationInfo)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(organizationInfo, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Organization/updateOrganizationInfo";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                   return string.Empty;
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

        public async Task<Result<OrganizationDetailsDto>> GetOrganizationDetails(int organizationId)
        {
            var res = new Result<OrganizationDetailsDto>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Organization/getOrganizationDetails?organizationId={organizationId}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<OrganizationDetailsDto>(result, options);
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

        public async Task<string> MakeRequest(int organizationId)
        {
            var res = "";
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/makeRequest?organizationId={organizationId}";

                HttpResponseMessage response = await _httpClient.PatchAsync(fullUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return string.Empty;
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

        public async Task<List<GetRequestDto>> GetRequests()
        {
            var res = new List<GetRequestDto>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Organization/getRequests";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<GetRequestDto>>(result, options);
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

        public async Task<string> AcceptRequest(int requestId)
        {
            var res = "";
            try
            {
                string fullUrl = $"{_apiUrl}/api/Organization/acceptRequest?requestId={requestId}";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return string.Empty;
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

        public async Task<string> RejectRequest(int requestId)
        {
            var res = "";
            try
            {
                string fullUrl = $"{_apiUrl}/api/Organization/rejectRequest?requestId={requestId}";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return string.Empty;
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
    }
}
