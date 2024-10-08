using WebPetCare.Components.IServices;
using System.Security.Claims;
using PetCareApp.Dtos;
using System.Text.Json;
using Azure;
using System.Net.Http;
using Microsoft.JSInterop;

namespace WebPetCare.Components.Services
{
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJSRuntime _jsRuntime;
        public AuthorizationService(HttpClient httpClient, IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor, IJSRuntime jsRuntime)
        : base(httpClient, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _jsRuntime = jsRuntime;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(loginDto, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string error = String.Empty;
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/Login";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    NewUserDto user = JsonSerializer.Deserialize<NewUserDto>(result, options);
                    if (user != null)
                    {
                       await SetStoreItemAsync(user.Token, "token");
                       await SetStoreItemAsync(user.Role, "role");
                    }
                }
                else
                {
                    error = await response.Content.ReadAsStringAsync(); 
                }

                return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<ResultData> Register(RegisterDto registerDto, string role)
        {
            var result = new ResultData();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(registerDto, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/register?role={role}";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseResult = await response.Content.ReadAsStringAsync();

                    NewUserDto user = JsonSerializer.Deserialize<NewUserDto>(responseResult, options);
                    if (user != null)
                    {
                        await SetStoreItemAsync(user.Token, "token");
                        await SetStoreItemAsync(user.Role, "role");

                        result = await SendEmail(user.Email);
                    }
                }
                else
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<ResultData> SendEmail(string email)
        {
            Random generator = new Random();
            var result = new ResultData();
            var randomNum = generator.Next(0, 1000000).ToString("D6");
            var data = new EmailConfirmationDto { Email = email , checkNumber = randomNum };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(data, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/sendEmail";

                HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    result.Result = randomNum;
                }
                else
                {
                    result.Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<bool> ConfirmEmail(string email)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(email, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                string fullUrl = $"{_apiUrl}/api/account/confirmEmail";

                HttpResponseMessage response = await _httpClient.PatchAsync(fullUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }


        public async Task SetStoreItemAsync(string token, string name)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorageSetItem",name, token);
        }


        

    }

    public class ResultData
    {
        public string Result { get; set; }
        public string Error { get; set; }
    }
}
