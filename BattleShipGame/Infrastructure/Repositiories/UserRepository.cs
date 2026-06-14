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

        /// <summary>
        /// Increments win for user who won and lose for user who lost
        /// </summary>
        /// <param name="hostId">Id of a host</param>
        /// <param name="guestId">Id of a guest</param>
        /// <param name="hostWon">Bool if host won or lost</param>
        /// <returns></returns>
        public async Task AddWinOrLose(string hostId, string guestId, bool hostWon)
        {
            var host = await _userManager.FindByIdAsync(hostId);
            var guest = await _userManager.FindByIdAsync(guestId);
            if (hostWon)
            {
                host.Wins++;
                guest.Losses++;
            } else
            {
                host.Losses++;
                guest.Wins++;
            }
            await _userManager.UpdateAsync(host);
            await _userManager.UpdateAsync(guest);
        }

        /// <summary>
        /// Creates user in database based on username and password, returns result array
        /// </summary>
        /// <param name="user">User class</param>
        /// <param name="password">User password</param>
        /// <returns>Result array if something went wrong, empty array if everything is okay</returns>
        public async Task<Result<IdentityError[]>> CreateUserAsync(User user, string password)
        {
            var dbResult = await _userManager.CreateAsync(user, password);
            Result<IdentityError[]> result = new Result<IdentityError[]>(
                dbResult == IdentityResult.Success
            );
            result.Errors = dbResult.Errors.ToArray();
            return result;
        }

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns>Bool value based on successful deletion from database</returns>
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _userManager.DeleteAsync(user) == IdentityResult.Success;
        }

        /// <summary>
        /// Returns user based on id
        /// </summary>
        /// <param name="id">User id to find</param>
        /// <returns>User class</returns>
        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        /// <summary>
        /// Returns user based on his username
        /// </summary>
        /// <param name="userName">User name to find</param>
        /// <returns>User class</returns>
        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// Updates user in database
        /// </summary>
        /// <param name="user">Current user to replace from database</param>
        /// <returns>Result array if something went wrong, empty array if everything is okay</returns>
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
