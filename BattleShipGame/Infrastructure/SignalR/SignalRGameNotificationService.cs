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

        public async Task ShipHit(int x, int y, int sessionId, bool isHit)
        {
            await _hubContext.Clients.Group(sessionId.ToString()).SendAsync("ShipHit", x, y, isHit);
        }
        public async Task WinGame(int sessionId, bool turn)
        {
            await _hubContext.Clients.Group(sessionId.ToString()).SendAsync("WinGame", turn);
        }
        public async Task StartFight(int sessionId)
        {
            await _hubContext.Clients.Group(sessionId.ToString()).SendAsync("StartFight");
        }
    }
}
