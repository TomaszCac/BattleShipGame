using BattleShipGame.Data;
using BattleShipGame.Interfaces;
using BattleShipGame.Models;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Repositiories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(UserDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return  result == IdentityResult.Success;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _userManager.DeleteAsync(user) == IdentityResult.Success;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var currentUser = await _userManager.FindByIdAsync(user.Id);
            currentUser.UserName = user.UserName;
            return await _userManager.UpdateAsync(currentUser) == IdentityResult.Success;
        }
    }
}
