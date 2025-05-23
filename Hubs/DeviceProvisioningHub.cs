using Microsoft.AspNetCore.SignalR;
using Server.Data.RedisStore;

namespace Server.Hubs
{
    public class DeviceProvisioningHub(ITempIdConnectionStore tempIdConnectionStore) : Hub
    {
        public async Task SubscribeToProvisioningChannel(string tempId)
        {
            string connectionId = Context.ConnectionId;
            await tempIdConnectionStore.AddTempIdConnectionAsync(tempId, connectionId);

        }

        public async Task UnsubscribeFromProvisioningChannel(string tempId)
        {
            //await tempIdConnectionStore.RemoveAsync(tempId);
        }
    }
}
