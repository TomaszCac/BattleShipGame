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
        public bool WinGame(int sessionId, bool turn)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == sessionId);
            if (turn)
            {
                foreach (int coordinates in session.BoardGuest)
                {
                    if (coordinates != 0 && coordinates != -1)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                foreach (int coordinates in session.BoardHost)
                {
                    if (coordinates != 0 && coordinates != -1)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public bool ShootShip(int x, int y, int sessionId, bool turn)
        {
            var game = _sessions.FirstOrDefault(b => b.Id == sessionId);
            if (turn)
            {
                if (game.BoardGuest[x, y] != 0)
                {
                    game.BoardGuest[x, y] = -1;
                    return true;
                }
                return false;
            }
            else
            {
                if (game.BoardHost[x, y] != 0)
                {
                    game.BoardHost[x, y] = -1;
                    return true;
                }
                return false;
            }
        }
        public bool EndSession(int sessionId)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == sessionId);
            return _sessions.Remove(session);
        }
        public void SetBoard(int[,] board, int sessionId, bool host)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == sessionId);
            if (host)
            {
                session.BoardHost = board;
            }
            else
            {
                session.BoardGuest = board;
            }
        }

        public bool CheckStart(int sessionId)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == sessionId);
            if (session.BoardHost != null)
            {
                if (session.BoardGuest != null)
                {
                    return true;
                }
            }
            return false;
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

        public (string, string?) GetUserIdsFromSession(int sessionId)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == sessionId);
            return (session.Host.Id, session.Guest != null ? session.Guest.Id : null);

        }
    }
}
