using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
      
    }
}
