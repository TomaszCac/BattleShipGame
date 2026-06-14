using Microsoft.AspNetCore.SignalR;

namespace BattleShipGame.Api.Hubs
{
    public class BattleShipHub : Hub
    {
        public async Task PlayerConnected(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("PlayerConnected", $"{userName} has joined the game {groupName}");
        }
        public async Task HostGame(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task Disconnected(string groupName, string userName)
        {
            await Clients.Group(groupName).SendAsync("PlayerDisconnected", $"{userName} has disconnected from the game");
        }

    }
}
