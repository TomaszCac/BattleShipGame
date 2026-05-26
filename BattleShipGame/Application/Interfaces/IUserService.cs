using BattleShipGame.Domain.Entities;

namespace BattleShipGame.Application.Interfaces
{
    public interface IUserService
    {
        public string? Login(User user, string password);
        public string? GetId();
    }
}
