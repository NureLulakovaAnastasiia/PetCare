using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using PetCareApp.Dtos;
using System.Net;
using System.Text.Json;
using WebPetCare.IServices;
using PetCareApp.Models;
using WebPetCare.Components.Pages.Account;
using System.Collections.Generic;

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

        public async Task<Result<List<BreakDto>>> getMasterBreaks(string? masterId)
        {
            var res = new Result<List<BreakDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                var strToAdd = !String.IsNullOrEmpty(masterId) ? $"?masterId={masterId}" : "";
                string fullUrl = $"{_apiUrl}/api/Master/getMasterBreaks{strToAdd}";

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

        public async Task<string> UpsertBreaks(List<BreakDto> breaks, string? masterId = null)
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
                var strToAdd = !String.IsNullOrEmpty(masterId) ? $"?masterId={masterId}" : "";
                string fullUrl = $"{_apiUrl}/api/Master/upsertBreaks{strToAdd}";

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
        public async Task<Result<Dictionary<int, string>>> GetMasterServicesNames(string? masterId = null)
        {
            var res = new Result<Dictionary<int, string>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                var strToAdd = !String.IsNullOrEmpty(masterId) ? $"?masterId={masterId}" : "";
                string fullUrl = $"{_apiUrl}/api/Service/getMasterServicesNames{strToAdd}";

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

        public async Task<string?> GetCurrentUserRole()
        {
            var res = await getCurrentRole();
            return res;
        }

        public async Task<Result<int>> AddPet(PetDto pet)
        {
            var res = new Result<int>();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(pet, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/User/addPet";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var data = int.TryParse(result, out int id);
                    if (data)
                    {
                        res.Data = id;
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

        public async Task<Result<string>> UpdatePets(List<PetDto> pets)
        {
            var res = new Result<string>();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(pets, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/User/updatePets";

                HttpResponseMessage response = await _httpClient.PutAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    res.Data = result;
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

        public async Task<string> DeletePet(int petId)
        {
            var res = "";
            try
            {
                string fullUrl = $"{_apiUrl}/api/User/deletePet?petId={petId}";

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

        public async Task<Result<MasterDto>> GetMasterData(string masterId)
        {
            var res = new Result<MasterDto>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/User/getMasterData?masterId={masterId}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<MasterDto>(result, options);
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

        public async Task<Result<List<ReviewDto>>> GetMasterReviews(string masterId)
        {
            var res = new Result<List<ReviewDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/User/getMasterReviews?masterId={masterId}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<ReviewDto>>(result, options);
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

        public async Task<Result<List<GetQuestionDto>>> GetServiceQuestionary(int serviceId)
        {
            var res = new Result<List<GetQuestionDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/getUserQuestionary?serviceId={serviceId}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<GetQuestionDto>>(result, options);
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

        public async Task<Result<QuestionaryAnalisysDto>> AnalizeQuestionary(List<QuestionDto> questionary, int serviceId)
        {
            var res = new Result<QuestionaryAnalisysDto>();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(questionary, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/Master/analizeQuestionary?serviceId={serviceId}";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<QuestionaryAnalisysDto>(result, options);
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

        public async Task<Result<List<RecordInfoDto>>> GetUserRecords()
        {
            var res = new Result<List<RecordInfoDto>>();
            
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);

                string fullUrl = $"{_apiUrl}/api/User/getUserRecords";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<RecordInfoDto>> (result, options);
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

        public async Task<string> CancelRecord(int recordId)
        {
            var res = string.Empty;
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/User/userCancelRecord?recordId={recordId}";

                HttpResponseMessage response = await httpClient.PatchAsync(fullUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return res;
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

        public async Task<string> AddReview(ReviewDto review)
        {
            var res = string.Empty;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(review, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/User/addReview";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return res;
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

        public async Task<Result<List<ScheduleDto>>> GetMasterSchedule(string? masterId)
        {
            var res = new Result<List<ScheduleDto>>();
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                var strToAdd = !String.IsNullOrEmpty(masterId) ? $"?masterId={masterId}" : "";
                string fullUrl = $"{_apiUrl}/api/Master/getMasterSchedule{strToAdd}";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    };
                    var data = JsonSerializer.Deserialize<List<ScheduleDto>>(result, options);
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

        public async Task<string> UpdateMasterSchedule(List<ScheduleDto> schedules)
        {
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/updateSchedule";
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                string json = JsonSerializer.Serialize(schedules, options);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                else
                {
                   return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> DeleteMasterSchedule(int scheduleId)
        {
            var res = string.Empty;
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/deleteSchedule?scheduleId={scheduleId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    return res;
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

        public async Task<string> AddMasterSchedule(ScheduleDto schedule)
        {
            try
            {
                httpClient = await HttpService.GetHttpClient(httpClient, jsRuntime);
                string fullUrl = $"{_apiUrl}/api/Master/addSchedule";
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                string json = JsonSerializer.Serialize(schedule, options);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    
}

