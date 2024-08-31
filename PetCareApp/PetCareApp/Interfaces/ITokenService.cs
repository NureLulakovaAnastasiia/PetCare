using PetCareApp.Models;
using System.Security.Claims;

namespace PetCareApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser, string role);

        ClaimsPrincipal? CheckToken(string token);



    }
}
