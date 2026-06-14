namespace BattleShipGame.Application.Interfaces
{
    public interface IGameNotificationService
    {
        /// <summary>
        /// Sends message to specified group if ship has been hit
        /// </summary>
        /// <param name="x">Int x value of x,y board</param>
        /// <param name="y">Int y value of x,y board</param>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="isHit">Bool value if ship has been hit</param>
        /// <returns></returns>
        public Task ShipHit(int x, int y, int sessionId, bool isHit);
        /// <summary>
        /// Sends message to specified group if player has won the game
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="turn">Bool value of which user has won true if host false if guest</param>
        /// <returns></returns>
        public Task WinGame(int sessionId, bool turn);
        /// <summary>
        /// Sends message to specified group about started fight
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <returns></returns>
        public Task StartFight(int sessionId);

    }
}
