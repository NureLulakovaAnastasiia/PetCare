namespace WebPetCare.Components.IServices
{
    public interface ITokenService
    {
        public string? GetToken();
        public HttpClient CreateHttpClient();
    }
}
