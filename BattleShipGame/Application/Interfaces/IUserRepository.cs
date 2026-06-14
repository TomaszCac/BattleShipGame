using BattleShipGame.Application.Common;
using BattleShipGame.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BattleShipGame.Application.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates user in database based on username and password, returns result array
        /// </summary>
        /// <param name="user">User class</param>
        /// <param name="password">User password</param>
        /// <returns>Result array if something went wrong, empty array if everything is okay</returns>
        public Task<Result<IdentityError[]>> CreateUserAsync(User user, string password);
        /// <summary>
        /// Returns user based on id
        /// </summary>
        /// <param name="id">User id to find</param>
        /// <returns>User class</returns>
        public Task<User?> GetUserByIdAsync(string id);
        /// <summary>
        /// Returns user based on his username
        /// </summary>
        /// <param name="userName">User name to find</param>
        /// <returns>User class</returns>
        public Task<User?> GetUserByUserNameAsync(string userName);
        /// <summary>
        /// Updates user in database
        /// </summary>
        /// <param name="user">Current user to replace from database</param>
        /// <returns>Result array if something went wrong, empty array if everything is okay</returns>
        public Task<Result<IdentityError[]>> UpdateUserAsync(User user);
        /// <summary>
        /// Increments win for user who won and lose for user who lost
        /// </summary>
        /// <param name="hostId">Id of a host</param>
        /// <param name="guestId">Id of a guest</param>
        /// <param name="hostWon">Bool if host won or lost</param>
        /// <returns></returns>
        public Task AddWinOrLose(string hostId, string guestId, bool hostWon);
        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns>Bool value based on successful deletion from database</returns>
        public Task<bool> DeleteUserAsync(string id);
    }
}
