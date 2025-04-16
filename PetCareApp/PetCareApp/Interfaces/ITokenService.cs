using PetCareApp.Models;
using System.Security.Claims;

namespace PetCareApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser, string role);

        ClaimsPrincipal? CheckToken(string token);

        bool CheckGoogleIds(string idToCheck, string realId);

        string CreateHash(string value);
    }
}
