using PetCareApp.Dtos;
using WebPetCare.Components.Services;

namespace WebPetCare.Components.IServices
{
    public interface IAuthorizationService
    {
        public Task<ResultData> Login(LoginDto loginDto);
        public Task<ResultData> Register(RegisterDto registerDto, string role);
        public Task<bool> ConfirmEmail(string email);
    }
}
