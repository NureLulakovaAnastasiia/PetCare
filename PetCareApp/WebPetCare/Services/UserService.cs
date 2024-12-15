using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using PetCareApp.Dtos;
using System.Net;
using System.Text.Json;
using WebPetCare.IServices;
using PetCareApp.Models;

namespace WebPetCare.Services
{
    public class UserService : BaseService, IUserService
    {
        private  HttpClient httpClient { get; set; }
        private IJSRuntime jsRuntime;
        public UserService(HttpClient httpClientbase, IConfiguration configuration, IJSRuntime runtime) :
            base(httpClientbase, configuration, runtime) 
        {
            httpClient = httpClientbase;
            jsRuntime = runtime;
        }
        public async Task<Result<GetGeneralMasterDto>> getGeneralMasterData()
        {
            var res = new Result<GetGeneralMasterDto>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/getMasterGeneralData";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<GetGeneralMasterDto>(result, options);
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

        public async Task<string> UpdateGeneralMasterData(GetGeneralMasterDto dto)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(dto, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/updateMasterGeneralData";

                HttpResponseMessage response = await _httpClient.PatchAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    
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

        public async Task<Result<List<BreakDto>>> getMasterBreaks()
        {
            var res = new Result<List<BreakDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/getMasterBreaks";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<BreakDto>>(result, options);
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

        public async Task<string> UpdateAppointments(List<GetRecordDto> records)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(records, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/updateAppointments";

                HttpResponseMessage response = await _httpClient.PutAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<string> UpsertBreaks(List<BreakDto> breaks)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(breaks, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/upsertBreaks";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<string> DeleteBreaks(List<int> breaks)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(breaks, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/deleteBreaks";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<string> AddRecord(RecordDto record)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(record, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/makeAnAppointment";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<string> CancelAppointment(int appointmentId, string reason)
        {
            var res = "";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(reason, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/CancelRecord?recordId={appointmentId}";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public async Task<Result<Dictionary<int, string>>> GetMasterServicesNames()
        {
            var res = new Result<Dictionary<int, string>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Service/getMasterServicesNames";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<Dictionary<int, string>>(result, options);
                    if (data != null)
                    {
                        res.Data = data;
                    }
                }
                else
                {
                    res.ErrorMessage = await response.Content.ReadAsStringAsync();
                    if(res.ErrorMessage == null)
                    {
                        res.ErrorMessage = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<List<PortfolioDto>>> GetMasterPortfolio(string? masterId = null)
        {
            var res = new Result<List<PortfolioDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/getPortfolio{(masterId != null ? "?masterId=" + masterId : "")}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<PortfolioDto>>(result, options);
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

        public async Task<Result<List<int>>> UpsertPortfolio(List<PortfolioDto> portfolioDtos)
        {
            var res = new Result<List<int>>();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(portfolioDtos, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/upsertPortfolio";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<int>>(result, options);
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

        public async Task<string> DeletePortfolio(int portfolioId)
        {
            var res = "";
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/deletePortfolio?porfolioId={portfolioId}";

                HttpResponseMessage response = await _httpClient.DeleteAsync(fullUrl);
                if (response.IsSuccessStatusCode)
                {
                    return res;
                }
                res = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public async Task<Result<List<GetServiceDto>>> GetMasterServices(string? masterId = null)
        {
            var res = new Result<List<GetServiceDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/getMasterServices{(masterId != null ? "?masterId=" + masterId : "")}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<GetServiceDto>>(result, options);
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

