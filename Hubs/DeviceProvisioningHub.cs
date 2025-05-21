using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Ocsp;
using Server.Data.RedisStore;

namespace Server.Hubs
{
    public class DeviceProvisioningHub(ITempIdConnectionStore _tempIdConnectionStore) : Hub
    {
        public async Task SubscribeToProvisioningChannel(string tempId)
        {
            string connectionId = Context.ConnectionId;
            await tempIdConnectionStore.AddTempIdConnectionAsync(tempId, connectionId);

        }

        public async Task UnsubscribeFromProvisioningChannel(string tempId)
        {
            await tempIdConnectionStore.RemoveAsync(tempId);
        }
    }
}
