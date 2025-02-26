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
        private readonly IAccountService _userService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, 
            IAccountService userService, SignInManager<AppUser> signInManager)
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
                appUser.Contacts = null;
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded) {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, role);
                    if (roleResult.Succeeded)
                    {
                        if (role == "Organization")
                        {
                            var res = _userService.CreateOrganization(appUser.Id);
                            if (!int.TryParse(res, out var num))
                            {
                                return StatusCode(500, "Error during organization creation");
                            }
                        }
                        return Ok(new NewUserDto
                        {
                            Role = role,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser, ""),
                        });
                    }
                    else
                    {
                        await _userManager.DeleteAsync(appUser);
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
            if (!user.EmailConfirmed)
            {
                return StatusCode(403, "You should confirm your email");
            }
            var newUser = new NewUserDto
            {
                Email = loginDto.Email,
                Role = roles.First(),
                Token = _tokenService.CreateToken(user, roles[0])
            };
            return Ok(
               newUser
            );
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail([FromBody]  EmailConfirmationDto emailConfirm)
        {
            try
            {
                var checkUser = await _userManager.FindByEmailAsync(emailConfirm.Email);
                if (checkUser != null && (!checkUser.EmailConfirmed || emailConfirm.IsPasswordChange))
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
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email,  string? newPassword)
        {
            try
            {
                var checkUser = await _userManager.FindByEmailAsync(email);
                if (checkUser != null) 
                {
                    if (!checkUser.EmailConfirmed) {
                        var res = _userService.SubmitEmail(email);
                        if (int.TryParse(res, out int num))
                        {
                            return Ok("Email was confirmed");
                        }
                        return StatusCode(500, res);
                    }else if (!String.IsNullOrEmpty(newPassword))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(checkUser);
                        var res = await _userManager.ResetPasswordAsync(checkUser, token, newPassword);
                        if (res.Succeeded)
                        {
                            _userService.AddChangePasswordHistory(checkUser.Id, true);
                            return Ok("You succesfully changed your password");
                        }
                        _userService.AddChangePasswordHistory(checkUser.Id, false);
                        return StatusCode(500, res);
                    }
                    return Unauthorized("User is not found");
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

        [HttpGet("getUserRole")]
        public async Task<IActionResult> GetUserRole()
        {
            var res = await _userService.GetCurrentRole();
            if (String.IsNullOrEmpty(res)){
                return NotFound();
            }
            {
                
            }
            return Ok(res);
        }

        
    }
}
