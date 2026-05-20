using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BattleShipGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BattleShipGame.Services
{
    public class UserService : IUserService
    {
        private readonly string _jwtKey;

        public UserService(IConfiguration configuration)
        {
            _jwtKey = configuration.GetSection("Token:Key").Value;
        }

        public string? Login(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
                return GenerateToken(user);
            else
                return null;
        }

        public string GenerateToken(User user)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
