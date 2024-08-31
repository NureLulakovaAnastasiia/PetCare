using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IUserService
    {
        string SendEmail(EmailConfirmationDto emailConfirmationDto);

        Task<List<string>> GetUserRole(string email);

        string SubmitEmail(string email);
    }
}
