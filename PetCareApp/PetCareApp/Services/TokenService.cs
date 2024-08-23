using Microsoft.IdentityModel.Tokens;
using PetCareApp.Interfaces;
using PetCareApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetCareApp.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration )
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:SigningKey"]));
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
               // new Claim(JwtRegisteredClaimNames.)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["JWT:Issuer"],
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = creds,
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
