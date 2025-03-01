using PetCareApp.Dtos;
using PetCareApp.Models;
using WebPetCare.Services;

namespace WebPetCare.IServices
{
    public interface IAuthorizationService
    {
        public Task<ResultData> Login(LoginDto loginDto);
        public Task<ResultData> Register(RegisterDto registerDto, string role);
        public Task<bool> ConfirmEmail(string email, string? newPassword = null);
        public Task<ResultData> SendEmailForPasswordRestore(string email);
        public Task<Result<bool>> CheckGoogleLogin(string email);

        public Task<Result<bool>> RegisterGoogleUser(AppUser user, string role);
        public Task<string> Logout();
    }
}
