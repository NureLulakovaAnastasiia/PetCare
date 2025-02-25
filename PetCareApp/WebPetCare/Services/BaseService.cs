using Microsoft.JSInterop;
using PetCareApp.Dtos;
using PetCareApp.Models;
using System.Net.Http;
using System.Text.Json;

namespace WebPetCare.Services
{
    public class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _apiUrl = "";
        private readonly IJSRuntime _jsRuntime;

        public BaseService(HttpClient httpClient, IConfiguration configuration, IJSRuntime jSRuntime)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"];
            _jsRuntime = jSRuntime;
        }

        public async Task<string> GetStoreItemAsync(string name)
        {
            var res = await _jsRuntime.InvokeAsync<string>("sessionStorageGetItem", name);
            return res;
        }

        public async Task<string?> getCurrentRole()
        {
            try
            {
                string fullUrl = $"{_apiUrl}/api/Account/getUserRole";

                HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return string.Empty;
        }

    }
}
