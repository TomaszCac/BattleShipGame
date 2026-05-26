using BattleShipGame.Domain.Entities;

namespace BattleShipGame.Application.Interfaces
{
    public interface IJwtTokenService
    {
        public string GenerateToken(User user);
    }
}
