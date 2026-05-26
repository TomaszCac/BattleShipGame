using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BattleShipGame.Infrastructure.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly string _jwtKey;

        public JwtTokenService(IConfiguration configuration)
        {
            _jwtKey = configuration.GetSection("Token:Key").Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "BattleShipGameApi",
                audience: "BattleShipGameApi-client",
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
