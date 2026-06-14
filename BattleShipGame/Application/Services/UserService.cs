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

        /// <summary>
        /// Creates a new instance of User Service
        /// </summary>
        /// <param name="jwtTokenService">Token service</param>
        /// <param name="httpContext">Http Context</param>
        public UserService(IJwtTokenService jwtTokenService, IHttpContextAccessor httpContext)
        {
            _jwtTokenService = jwtTokenService;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Returns id from JWT token claims
        /// </summary>
        /// <returns>Id value as string or null if claim is not present from JWT token</returns>
        public string? GetId()
        {
            if (_httpContext.HttpContext != null)
                return _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.Empty;
        }

        /// <summary>
        /// Returns token by comparing password hash from user object and password inserted by user
        /// </summary>
        /// <param name="user">The user who has specified password hash</param>
        /// <param name="password">Inserted by user password</param>
        /// <returns>Jwt token as string or null if password has been wrong</returns>
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
