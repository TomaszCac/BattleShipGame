using System.Security.Claims;
using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IJwtTokenService jwtTokenService, IHttpContextAccessor httpContext)
        {
            _jwtTokenService = jwtTokenService;
            _httpContext = httpContext;
        }

        public string? GetId()
        {
            if (_httpContext.HttpContext != null)
                return _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.Empty;
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
