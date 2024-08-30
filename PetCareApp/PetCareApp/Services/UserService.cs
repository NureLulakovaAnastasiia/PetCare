using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Reflection.PortableExecutable;

namespace PetCareApp.Services
{
    public class UserService : IUserService
    {
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
    }
}
