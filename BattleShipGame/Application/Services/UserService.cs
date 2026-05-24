using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public string? Login(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
                return _jwtTokenService.GenerateToken(user);
            else
                return null;
        }

        
    }
}
