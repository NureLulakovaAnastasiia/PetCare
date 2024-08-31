using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserService _userService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, 
            IUserService userService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _userService = userService;
            _signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, [FromQuery] string role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                role = role.ToLower();

                if (String.IsNullOrEmpty(role) || role == "admin")
                {
                    return BadRequest("Wrong role");
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
                    var roleResult = await _userManager.AddToRoleAsync(appUser, role);
                    if (roleResult.Succeeded)
                    {
                        Random generator = new Random();
                        return Ok(new NewUserDto
                        {
                            FirstName = appUser.FirstName,
                            Email = appUser.Email,
                            token = _tokenService.CreateToken(appUser, ""),
                            //checkNumber = generator.Next(0, 1000000).ToString("D6")
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _userManager.Users.FirstOrDefault(x => x.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email");
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!passwordCheck.Succeeded)
            {
                return Unauthorized("Username or password is incorrect");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any())
            {
                return StatusCode(400, "Somethimg went wromg with your access");
            }
            return Ok(
                new NewUserDto
                {
                    Email = loginDto.Email,
                    token = _tokenService.CreateToken(user, roles[0])
                }
            );
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail([FromBody]  EmailConfirmationDto emailConfirm)
        {
            try
            {
                var checkUser = await _userManager.FindByEmailAsync(emailConfirm.Email);
                if (checkUser != null && !checkUser.EmailConfirmed)
                {
                    var res = _userService.SendEmail(emailConfirm);
                    if(!String.Equals(res, "Success")) { 
                        return StatusCode(500, res);
                    }
                }
                else
                {
                    return Unauthorized("User is not found");
                }
                return Ok("Email was sent");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPatch("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] string email)
        {
            try
            {
                var checkUser = await _userManager.FindByEmailAsync(email);
                if (checkUser != null && !checkUser.EmailConfirmed) 
                {
                   var res = _userService.SubmitEmail(email);
                    if (int.TryParse(res, out int num))
                    {
                        return Ok("Email was confirmed");
                    }
                    return StatusCode(500, res);
                }
                else
                {
                    return Unauthorized("User is not found");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPatch("changeRoleToMaster")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ChangeRoleToMaster()
        {
            try
            {
                var email = User.Identity;
                if (email == null)
                {
                    return StatusCode(500, "Cannot authorize user");
                }
                var user = await _userManager.FindByEmailAsync(email.Name);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Master"))
                    {
                        return StatusCode(400, "User already has a master role");
                    }
                    else if (roles.Contains("User"))
                    {
                        var res = await _userManager.RemoveFromRoleAsync(user, "User");
                        if (res.Succeeded)
                        {
                            res = await _userManager.AddToRoleAsync(user, "Master");
                            if (res.Succeeded)
                            {
                                return Ok("User succesfully changed role to master");
                            }

                            return StatusCode(500, "Error during changing role");
                        }

                        return StatusCode(500, "User has no rights to change role");
                    }
                    return StatusCode(500, "User cannot change role");
                }
                return StatusCode(404, "User not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
