using PetCareApp.Dtos;
using WebPetCare.Services;

namespace WebPetCare.IServices
{
    public interface IAuthorizationService
    {
        public Task<ResultData> Login(LoginDto loginDto);
        public Task<ResultData> Register(RegisterDto registerDto, string role);
        public Task<bool> ConfirmEmail(string email);
    }
}
