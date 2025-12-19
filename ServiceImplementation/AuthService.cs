using DomainLayer.Models.Identity;
using DomainLayer.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceImplementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> CreateTokenAsync(
      ApplicationUser user,
      UserManager<ApplicationUser> userManager,
      string role)
        {
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),   // 🔴 أهم سطر
        new Claim(ClaimTypes.Name, user.DisplayName ?? ""),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Role, role),
        new Claim("UserType", user.UserType ?? "")
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtOptions:Key"]!)
            );

            var token = new JwtSecurityToken(
                issuer: _config["JwtOptions:Issuer"],
                audience: _config["JwtOptions:Audience"],
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(
                    double.Parse(_config["JwtOptions:DurationInDays"]!)
                ),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }


}

