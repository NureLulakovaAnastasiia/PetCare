using PetCareApp.Dtos;

namespace WebPetCare.Components.IServices
{
    public interface IAuthorizationService
    {
        public Task<string> Login(LoginDto loginDto);
    }
}
