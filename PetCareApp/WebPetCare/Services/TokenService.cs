using Microsoft.JSInterop;
using PetCareApp.Services;
using System.Net.Http.Headers;
using WebPetCare.IServices;


namespace WebPetCare.Services
{
    public class TokenService :BaseService, ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        public TokenService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient, 
            IConfiguration configuration, IJSRuntime jSRuntime): base(httpClient, configuration, jSRuntime)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string?> GetToken()
        {
            var token = await GetStoreItemAsync("token");
            return token;
        }

        public async Task<HttpClient> CreateHttpClient()
        {
            var client = new HttpClient();
            var token = await GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        
    }
}
