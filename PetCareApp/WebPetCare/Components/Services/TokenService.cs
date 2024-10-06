using System.Net.Http.Headers;
using WebPetCare.Components.IServices;


namespace WebPetCare.Components.Services
{
    public class TokenService: ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.ContainsKey("token"))
            {
                return context.Request.Cookies["token"];
            }

            return null;
        }

        public HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            var token = GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
