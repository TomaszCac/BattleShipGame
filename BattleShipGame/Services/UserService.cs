using BattleShipGame.Models;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Services
{
    public class UserService : IUserService
    {
        public bool Login(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
