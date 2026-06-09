using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using Newtonsoft.Json;

namespace BattleShipGame.Infrastructure.Repositiories
{
    public class SessionRepository : ISessionRepository
    {
        private List<Session> _sessions = new();

        public string CreateSession(User user)
        {
            Session session = new Session();
            session.Host = user;
            if (_sessions.Any())
                session.Id = _sessions.Last().Id + 1;
            else
                session.Id = 1;
            _sessions.Add(session);
            return JsonConvert.SerializeObject(session);
        }

        public List<Session>? GetAvailableSessions()
        {
            return _sessions.FindAll(b => b.Guest == null);
        }

        public Session? GetSession(int id)
        {
            return _sessions.FirstOrDefault(b => b.Id == id);
        }

        public Session JoinSession(User user, int id)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == id);
            session.Guest = user;
            return session;
        }

        public bool RemoveSession(int id)
        {
            var sessionToRemove = _sessions.FirstOrDefault(b => b.Id == id);
            if (sessionToRemove != null)
                return _sessions.Remove(sessionToRemove);
            else
                return false;
        }
    }
}
