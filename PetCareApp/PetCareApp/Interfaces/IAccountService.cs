using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IAccountService
    {
        string SendEmail(EmailConfirmationDto emailConfirmationDto);

        Task<List<string>> GetUserRoleByEmail(string email);

        string SubmitEmail(string email);

        public Task<string?> GetCurrentRole();
    }
}
