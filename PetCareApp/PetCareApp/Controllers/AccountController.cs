using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    LastName = registerDto.LastName,
                    FirstName = registerDto.FirstName
                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded) {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        Random generator = new Random();
                        return Ok(new NewUserDto
                        {
                            FirstName = appUser.FirstName,
                            Email = appUser.Email,
                            token = _tokenService.CreateToken(appUser),
                            checkNumber = generator.Next(0, 1000000).ToString("D6")
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
