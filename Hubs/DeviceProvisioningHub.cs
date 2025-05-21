using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class DeviceProvisioningHub : Hub
    {
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
