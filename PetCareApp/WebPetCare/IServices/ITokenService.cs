namespace WebPetCare.IServices
{
    public interface ITokenService
    {
        public Task<string?> GetToken();
        public Task<HttpClient> CreateHttpClient();
    }
}
