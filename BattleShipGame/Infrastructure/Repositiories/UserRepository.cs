using BattleShipGame.Application.Common;
using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using BattleShipGame.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Infrastructure.Repositiories
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

        public async Task<Result<IdentityError[]>> CreateUserAsync(User user, string password)
        {
            var dbResult = await _userManager.CreateAsync(user, password);
            Result<IdentityError[]> result = new Result<IdentityError[]>(
                dbResult == IdentityResult.Success
            );
            result.Errors = dbResult.Errors.ToArray();
            return result;
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

        public async Task<Result<IdentityError[]>> UpdateUserAsync(User user)
        {
            var currentUser = await _userManager.FindByIdAsync(user.Id);
            currentUser.UserName = user.UserName;
            var dbResult = await _userManager.UpdateAsync(currentUser);
            Result<IdentityError[]> result = new Result<IdentityError[]>(
                dbResult == IdentityResult.Success
            );
            result.Errors = dbResult.Errors.ToArray();
            return result;
        }
    }
}
