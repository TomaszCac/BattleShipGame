using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using Newtonsoft.Json;

namespace BattleShipGame.Infrastructure.Repositiories
{
    public class SessionRepository : ISessionRepository
    {
        private List<Session> _sessions = new();

        /// <summary>
        /// Creates session in database, returns session object as JSON string
        /// </summary>
        /// <param name="user">User class of host</param>
        /// <returns>JSON string of session</returns>
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

        /// <summary>
        /// Returns all sessions
        /// </summary>
        /// <returns>List of sessions</returns>
        public List<Session>? GetAvailableSessions()
        {
            return _sessions.FindAll(b => b.Guest == null);
        }
        /// <summary>
        /// Checks if board has all battleships destroyed and returns bool
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="turn">Current board to check</param>
        /// <returns>Bool value based on destroyed ships</returns>
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
        /// <summary>
        /// Checks if specific place has ship and if it is shot
        /// </summary>
        /// <param name="x">Int x value of x,y board</param>
        /// <param name="y">Int y value of x,y board</param>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="turn">Current board to check</param>
        /// <returns>Bool value if ship has been shot</returns>
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
        /// <summary>
        /// Deletes session from data and returns if session has been deleted successfully
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <returns>Bool value of succesfull session deletion</returns>
        public bool EndSession(int sessionId)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == sessionId);
            return _sessions.Remove(session);
        }
        /// <summary>
        /// Sets board [x,y] for specific user in session
        /// </summary>
        /// <param name="board">Two dimensional board to set for user</param>
        /// <param name="sessionId">Id value of specific session</param>
        /// <param name="host">Current user (true if host, false if guest) to set to board</param>
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

        /// <summary>
        /// Checks if both players have set their boards
        /// </summary>
        /// <param name="sessionId">Id value of specific session</param>
        /// <returns>Bool value based on both board set</returns>
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

        /// <summary>
        /// Get session based on Id
        /// </summary>
        /// <param name="id">Id of a specific session</param>
        /// <returns>Session class based on id</returns>
        public Session? GetSession(int id)
        {
            return _sessions.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Adds user to session as guest
        /// </summary>
        /// <param name="user">User class to add to session</param>
        /// <param name="id">Id of a specific session</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns two user Ids (host,guest) from specific session
        /// </summary>
        /// <param name="sessionId">Id of a specific session</param>
        /// <returns>Two string values (host,guest)</returns>
        public (string, string?) GetUserIdsFromSession(int sessionId)
        {
            var session = _sessions.FirstOrDefault(b => b.Id == sessionId);
            return (session.Host.Id, session.Guest != null ? session.Guest.Id : null);

        }
    }
}
