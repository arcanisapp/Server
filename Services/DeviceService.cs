using Newtonsoft.Json;
using Server.Crypto;
using Server.Data;
using Server.Models.Dto.Device.Add;
using Server.Models.Entitys;
using Server.Services.Validation;
using System.Text;

namespace Server.Services
{
    public interface IDeviceService
    {
        Task<bool> AddDeviceRequestAsync(byte[] requestSignature, string rawJson);
    }
    public class DeviceService(
        ITimestampValidator timestampValidator,
        IMlDsaKeyVerifier mlDsaKeyVerifier,
        AppDbContext appDbContext) : IDeviceService
    {
        public async Task<bool> AddDeviceRequestAsync(byte[] requestSignature, string rawJson)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
                var request = JsonConvert.DeserializeObject<NewDeviceRequest>(rawJson, settings);

                if (!timestampValidator.IsValid(request.Timestamp))
                    return false;

                var trustedSeviceSPK = appDbContext.Devices.Where(d => d.Id == request.TrustedDeviceId)
                    .Select(d => d.SPK)
                    .FirstOrDefault();

                if (trustedSeviceSPK == null)
                    return false;

                if (!await mlDsaKeyVerifier.VerifyAsync(Convert.FromBase64String(trustedSeviceSPK), Encoding.UTF8.GetBytes(rawJson), requestSignature))
                    return false;

                var addDeviceData = new AddDeviceData
                {
                    Payload = request.Payload,
                    TrustedSignature = request.TrustedSignature,
                    PayloadHash = request.PayloadHash,
                    TempId = request.TempId,
                };

                await appDbContext.AddDeviceData.AddAsync(addDeviceData);

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
