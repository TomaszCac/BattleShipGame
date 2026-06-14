using BattleShipGame.Api.Hubs;
using BattleShipGame.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BattleShipGame.Infrastructure.SignalR
{
    public class SignalRGameNotificationService : IGameNotificationService
    {
        private readonly IHubContext<BattleShipHub> _hubContext;

        public SignalRGameNotificationService(IHubContext<BattleShipHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Sends message to specified group if ship has been hit
        /// </summary>
        /// <param name="x">Int x value of x,y board</param>
        /// <param name="y">Int y value of x,y board</param>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="isHit">Bool value if ship has been hit</param>
        /// <returns></returns>
        public async Task ShipHit(int x, int y, int sessionId, bool isHit)
        {
            await _hubContext.Clients.Group(sessionId.ToString()).SendAsync("ShipHit", x, y, isHit);
        }
        /// <summary>
        /// Sends message to specified group if player has won the game
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <param name="turn">Bool value of which user has won true if host false if guest</param>
        /// <returns></returns>
        public async Task WinGame(int sessionId, bool turn)
        {
            await _hubContext.Clients.Group(sessionId.ToString()).SendAsync("WinGame", turn);
        }
        /// <summary>
        /// Sends message to specified group about started fight
        /// </summary>
        /// <param name="sessionId">Id value of a specific session</param>
        /// <returns></returns>
        public async Task StartFight(int sessionId)
        {
            await _hubContext.Clients.Group(sessionId.ToString()).SendAsync("StartFight");
        }
    }
}
