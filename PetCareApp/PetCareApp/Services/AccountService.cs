﻿using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Identity;
using PetCareApp.Models;
using PetCareApp.Data;
using AutoMapper;

namespace PetCareApp.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _dBContext;

        public AccountService(UserManager<AppUser> userManager, IMapper mapper, ApplicationDBContext context,
           IHttpContextAccessor httpContextAccessor,RoleManager<IdentityRole> roleManager) : base(userManager, httpContextAccessor)
        {
            _roleManager = roleManager;
            _dBContext = context;
        }

        public string SendEmail(EmailConfirmationDto emailConfirmationDto)
        {
            
            string header = "PetCare Email Confirmation";
            string body = $"Please, write this code on the page: \n {emailConfirmationDto.checkNumber} \n";
            string fromEmail = "anastasiia.lulakova@nure.ua";
            string password = "zkds rfne ikwa rnjc";

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

        public async Task<List<string>> GetUserRoleByEmail(string email)
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

        public async Task<string?> GetCurrentRole()
        {
            return await GetCurrentUserRole();
        }

        public string CreateOrganization(string userId)
        {
            var organization = new Organization { AppUserId = userId };
            try
            {
                _dBContext.Add(organization);
                return _dBContext.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void AddChangePasswordHistory(string userId, bool isSuccess)
        {
            _dBContext.Add(HistoryEventFactory.CreatePasswordChangeEvent(userId, isSuccess));
            _dBContext.SaveChanges();
        }

        //public async Task<string> ChangeRoleToMaster()
        //{
        //    try
        //    {
        //        var user = await GetCurrentUserAsync();
        //        if (user != null)
        //        {
        //            var roles = await _userManager.GetRolesAsync(user);
        //            if (roles != null && roles.Contains("User"))
        //            {
        //                var res = await _userManager.RemoveFromRoleAsync(user, "User");
        //                if(res != null && res.Succeeded)
        //                {
        //                    res = await _userManager.AddToRoleAsync(user, "Master");
        //                    if(res != null && res.Succeeded)
        //                    {
        //                        return string.Empty;
        //                    }
        //                }

        //                return res.Errors.ToString();
        //            }
        //            return "You don't have acces to this action";
        //        }
        //        return "User was not found";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
    }
}
