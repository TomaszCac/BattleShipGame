using BattleShipGame.Api.Hubs;
using BattleShipGame.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BattleShipGame.Infrastructure.SignalR
{
    public class SignalRGameNotificationService : IGameNotificationService
    {
        private readonly IHubContext<BattleShipHub> _hubContext;
        public async Task StartFight(int sessionId)
        {
            await _hubContext.Clients.Group(sessionId.ToString()).SendAsync("StartFight");
        }
    }
}
