using BattleShipGame.Domain.Entities;

namespace BattleShipGame.Application.Interfaces
{
    public interface ISessionRepository
    {
        public Session? GetSession(int id);
        public string CreateSession(User user);
        public bool RemoveSession(int id);
        public List<Session>? GetAvailableSessions();
        public Session JoinSession(User user, int id);
        public bool WinGame(int sessionId, bool turn);
        public bool ShootShip(int x, int y, int sessionId, bool turn);
        public bool EndSession(int sessionId);
        public (string, string) GetUserIdsFromSession(int sessionId);
        public void SetBoard(int[,] board, int sessionId, bool host);
        public bool CheckStart(int sessionId);
    }
}
