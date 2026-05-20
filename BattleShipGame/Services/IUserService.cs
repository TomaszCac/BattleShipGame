using BattleShipGame.Models;

namespace BattleShipGame.Services
{
    public interface IUserService
    {
        public bool Login(User user, string password);

    }
}
