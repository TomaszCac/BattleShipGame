using BattleShipGame.Models;

namespace BattleShipGame.Services
{
    public interface IUserService
    {
        public string? Login(User user, string password);
        public string GenerateToken(User user);

    }
}
