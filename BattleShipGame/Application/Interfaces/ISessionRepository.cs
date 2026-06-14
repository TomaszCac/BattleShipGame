using BattleShipGame.Domain.Entities;

namespace BattleShipGame.Application.Interfaces
{
    public interface ISessionRepository
    {
        /// <summary>
        /// Get session based on Id
        /// </summary>
        /// <param name="id">Id of a specific session</param>
        /// <returns>Session class based on id</returns>
        public Session? GetSession(int id);
        /// <summary>
        /// Creates session in database, returns session object as JSON string
        /// </summary>
        /// <param name="user">User class of host</param>
        /// <returns>JSON string of session</returns>
        public string CreateSession(User user);
        public bool RemoveSession(int id);
        /// <summary>
        /// Returns all sessions
        /// </summary>
        /// <returns>List of sessions</returns>
        public List<Session>? GetAvailableSessions();
        /// <summary>
        /// Adds user to session as guest
        /// </summary>
        /// <param name="user">User class to add to session</param>
        /// <param name="id">Id of a specific session</param>
        /// <returns></returns>
        public Session JoinSession(User user, int id);
        /// <summary>
        /// Checks if board has all battleships destroyed and returns bool
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="turn">Current board to check</param>
        /// <returns>Bool value based on destroyed ships</returns>
        public bool WinGame(int sessionId, bool turn);
        /// <summary>
        /// Checks if specific place has ship and if it is shot
        /// </summary>
        /// <param name="x">Int x value of x,y board</param>
        /// <param name="y">Int y value of x,y board</param>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="turn">Current board to check</param>
        /// <returns>Bool value if ship has been shot</returns>
        public bool ShootShip(int x, int y, int sessionId, bool turn);
        /// <summary>
        /// Deletes session from data and returns if session has been deleted successfully
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <returns>Bool value of succesfull session deletion</returns>
        public bool EndSession(int sessionId);
        /// <summary>
        /// Returns two user Ids (host,guest) from specific session
        /// </summary>
        /// <param name="sessionId">Id of a specific session</param>
        /// <returns>Two string values (host,guest)</returns>
        public (string, string) GetUserIdsFromSession(int sessionId);
        /// <summary>
        /// Sets board [x,y] for specific user in session
        /// </summary>
        /// <param name="board">Two dimensional board to set for user</param>
        /// <param name="sessionId">Id value of specific session</param>
        /// <param name="host">Current user (true if host, false if guest) to set to board</param>
        public void SetBoard(int[,] board, int sessionId, bool host);
        /// <summary>
        /// Checks if both players have set their boards
        /// </summary>
        /// <param name="sessionId">Id value of specific session</param>
        /// <returns>Bool value based on both board set</returns>
        public bool CheckStart(int sessionId);
    }
}
