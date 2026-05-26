using BattleShipGame.Application.Common;
using BattleShipGame.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<Result<IdentityError[]>> CreateUserAsync(User user, string password);
        public Task<User?> GetUserByIdAsync(string id);
        public Task<User?> GetUserByUserNameAsync(string userName);
        public Task<Result<IdentityError[]>> UpdateUserAsync(User user);
        public Task<bool> DeleteUserAsync(string id);
    }
}
