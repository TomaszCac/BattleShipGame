using BattleShipGame.Models;

namespace BattleShipGame.Interfaces
{
    public interface ISessionRepository
    {
        public Session? GetSession(int id);
        public string CreateSession(User user);
        public bool RemoveSession(int id);
        public List<Session>? GetAvailableSessions();
    }
}
