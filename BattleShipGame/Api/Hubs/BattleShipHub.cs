using Microsoft.AspNetCore.SignalR;

namespace BattleShipGame.Api.Hubs
{
    public class BattleShipHub : Hub
    {
        public async Task PlayerConnected(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Player Connected", $"{userName} has joined the game {groupName}");
        }
        public async Task HostGame(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

    }
}
