using Microsoft.JSInterop;

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

    }
}
