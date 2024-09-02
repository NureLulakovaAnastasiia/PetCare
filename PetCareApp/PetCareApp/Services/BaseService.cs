using Microsoft.AspNetCore.Identity;
using PetCareApp.Models;

namespace PetCareApp.Services
{
    public abstract class BaseService
    {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<AppUser?> GetCurrentUserAsync()
        {
            var userEmail = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (userEmail == null) return null;
            return await _userManager.FindByEmailAsync(userEmail);
        }
    }

}
