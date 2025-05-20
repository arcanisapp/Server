using MessagePack;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Crypto;
using Server.Data;
using Server.Hubs;
using Server.Models.Dto.Device.Add;
using Server.Models.Dto.Device.Confirm;
using Server.Models.Entitys;
using Server.Services.Validation;
using System.Security.Principal;

namespace Server.Services
{
    public interface IDeviceService
    {
        Task<bool> AddDeviceRequestAsync(byte[] requestSignature, byte[] rawData);
        Task<bool> ConfirmDeviceRequestAsync(byte[] requestSignature, byte[] rawData);
    }
    public class DeviceService(
        ITimestampValidator timestampValidator,
        IMlDsaKeyVerifier mlDsaKeyVerifier,
        AppDbContext appDbContext,
        IHubContext<DeviceProvisioningHub> deviceProvisionHub) : IDeviceService
    {
        public async Task<bool> AddDeviceRequestAsync(byte[] requestSignature, byte[] rawData)
        {
            try
            {
                var request = MessagePackSerializer.Deserialize<NewDeviceRequest>(rawData);

                if (!timestampValidator.IsValid(request.Timestamp))
                    return false;

                byte[] trustedDeviceSPK = await appDbContext.Devices.Where(d => d.Id == request.TrustedDeviceId)
                    .Select(d => d.SPK)
                    .FirstOrDefaultAsync() ?? throw new ArgumentNullException();

                if (trustedDeviceSPK == null)
                    return false;

                if (!await mlDsaKeyVerifier.VerifyAsync(trustedDeviceSPK, rawData, requestSignature))
                {
                    return false;
                }

                var responce = new NewDeviceResponce()
                {
                    PrivatePayload = request.PrivatePayload,
                    TrustedSignature = request.TrustedSignature,
                    PrivatePayloadHash = request.PrivatePayloadHash,
                    PublicPayloadHash = request.PublicPayloadHash,
                    PublicPayload = request.PublicPayload
                };
                byte[] msgpackBytes = MessagePackSerializer.Serialize(responce);

                await deviceProvisionHub.Clients.Group(request.TempId).SendAsync("ReceiveProvisioningResponse", msgpackBytes);
               
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> ConfirmDeviceRequestAsync(byte[] requestSignature, byte[] rawData)
        {
            try
            {
                var request = MessagePackSerializer.Deserialize<AddNewDeviceRequest>(rawData);

                if (!timestampValidator.IsValid(request.Timestamp))
                    return false;

                var newDevicePayload = MessagePackSerializer.Deserialize<NewDevicePublicPayloadRequest>(request.DevicePayload);

                if (newDevicePayload == null)
                    return false;

                var trustedDeviceSPK = await appDbContext.Devices.FirstOrDefaultAsync(d => d.Id == newDevicePayload.TrustedDeviceId);

                if (trustedDeviceSPK == null)
                    return false;

                if (!await mlDsaKeyVerifier.VerifyAsync(trustedDeviceSPK.SPK, request.DevicePayload, request.DeviceTrustedSignature))
                {
                    return false;
                }

                var userAccount = await appDbContext.Accounts.Include(a => a.Devices).ThenInclude(d => d.PreKeys).FirstOrDefaultAsync(a => a.Id == newDevicePayload.AccountId);

                if (userAccount == null)
                    return false;

                var newDevice = new Device
                {
                    Id = newDevicePayload.DeviceId,
                    Name = newDevicePayload.Name,
                    SPK = newDevicePayload.SPK,
                    Signature = newDevicePayload.SPKSignature,
                    PreKeys = []
                };

                foreach (var pk in request.PreKeysPayload)
                {
                    newDevice.PreKeys.Add(new PreKey
                    {
                        Id = pk.Id,
                        PK = pk.PK,
                        PKSignature = pk.PKSignature
                    });
                }

                userAccount.Devices.Add(newDevice);
                await appDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
