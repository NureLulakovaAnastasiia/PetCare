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
                       await SetTokenAsync(user.Token);
                    }
                }
                else
                {
                    error = response.ToString();
                }

                return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task SetTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorageSetItem", "token", token);
        }


    }
}
