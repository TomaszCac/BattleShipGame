using BattleShipGame.Models;

namespace BattleShipGame.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> CreateUserAsync(User user, string password);
        public Task<User?> GetUserByIdAsync(string id);
        public Task<User?> GetUserByUserNameAsync(string userName);

        public Task<bool> UpdateUserAsync(User user);
        public Task<bool> DeleteUserAsync(string id);

    }
}
