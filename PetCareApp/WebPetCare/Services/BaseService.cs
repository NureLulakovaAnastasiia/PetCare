namespace WebPetCare.Services
{
    public class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _apiUrl;

        public BaseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"];
        }

    }
}
