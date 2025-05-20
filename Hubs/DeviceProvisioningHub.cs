using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Server.Hubs
{
    public class DeviceProvisioningHub : Hub
    {
        // Группы = каналы
        public async Task SubscribeToProvisioningChannel(string channelId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
        }

        public async Task UnsubscribeFromProvisioningChannel(string channelId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);
        }
    }
}
