using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Identity;
using PetCareApp.Models;
using PetCareApp.Data;

namespace PetCareApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _dBContext;

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDBContext dBContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dBContext = dBContext;
        }

        public string SendEmail(EmailConfirmationDto emailConfirmationDto)
        {
            
            string header = "PetCare Email Confirmation";
            string body = $"Please, write this code on the page: \n {emailConfirmationDto.checkNumber} \n";
            string fromEmail = "anastasiia.lulakova@nure.ua";
            string password = "";

            MailAddress from = new MailAddress(fromEmail);
            MailAddress to = new MailAddress(emailConfirmationDto.Email);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage(from, to)
            {
                Subject = header,
                Body = body,
            };

            try
            {
                smtpClient.Send(mailMessage);
                return "Success";
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }

        }

        public async Task<List<string>> GetUserRole(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser == null)
            {
                return new List<string>();
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            return roles as List<string>;
        }   //check

        public string SubmitEmail(string email)
        {
            try
            {
                var user = _dBContext.Users.FirstOrDefault(x => x.Email == email);
                user.EmailConfirmed = true;
                _dBContext.Update(user);
                return _dBContext.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
