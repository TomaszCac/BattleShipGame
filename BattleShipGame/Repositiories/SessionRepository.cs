using BattleShipGame.Interfaces;
using BattleShipGame.Models;
using Newtonsoft.Json;

namespace BattleShipGame.Repositiories
{
    public class SessionRepository : ISessionRepository
    {
        private List<Session> Sessions = new();

        public string CreateSession(User user)
        {
            Session session = new Session();
            session.Host = user;
            if (Sessions.Any())
                session.Id = Sessions.Last().Id + 1;
            else
                session.Id = 1;
            Sessions.Add(session);
            return JsonConvert.SerializeObject(session);
        }

        public List<Session>? GetAvailableSessions()
        {
            return Sessions.FindAll(b => b.Guest == null);
        }

        public Session? GetSession(int id)
        {
            return Sessions.FirstOrDefault(b => b.Id == id);
        }

        public bool RemoveSession(int id)
        {
            var sessionToRemove = Sessions.FirstOrDefault(b => b.Id == id);
            if (sessionToRemove != null)
                return Sessions.Remove(sessionToRemove);
            else
                return false;
        }
    }
}
