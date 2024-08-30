using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IUserService
    {
        string SendEmail(EmailConfirmationDto emailConfirmationDto);
    }
}
