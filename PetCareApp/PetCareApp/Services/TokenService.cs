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
        public string CreateToken(AppUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["JWT:Issuer"],
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds,
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }


        public ClaimsPrincipal? CheckToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            ClaimsPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }

        public string GetTokenClaims(string token)
        {
            var principal = CheckToken(token);
            if (principal == null)
            {

            }
            var username = principal.Identity.Name;
            var roles = principal.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
            if (!roles.Any())
            {
                return string.Empty;
            }

            return roles[0];
        }
    }
}
