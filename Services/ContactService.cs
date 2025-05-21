using MessagePack;
using Microsoft.EntityFrameworkCore;
using Server.Crypto;
using Server.Data;
using Server.Models.Dto.Contact.Lookup;
using Server.Services.Validation;

namespace Server.Services
{
    public interface IContactService
    {
        Task<byte[]> LookupUserRequestAsync(byte[] requestSignature, byte[] rawData);
    }
    public class ContactService(AppDbContext appDbContext, ITimestampValidator timestampValidator, IMlDsaKeyVerifier mlDsaKeyVerifier) : IContactService
    {
        public async Task<byte[]> LookupUserRequestAsync(byte[] requestSignature, byte[] rawData)
        {
            LookupUserRequest request = MessagePackSerializer.Deserialize<LookupUserRequest>(rawData) ?? throw new ArgumentException();

            if (!timestampValidator.IsValid(request.Timestamp))
                throw new ArithmeticException();

            var trustedDeviceSPK = appDbContext.Devices
                .Where(d => d.Id == request.DeviceId)
                .Select(d => d.SPK)
                .FirstOrDefault() ?? throw new ArgumentNullException();

            if (!await mlDsaKeyVerifier.VerifyAsync(trustedDeviceSPK, rawData, requestSignature))
            {
                throw new UnauthorizedAccessException();
            }

            var userData = await appDbContext.Accounts
                .Where(a => a.Id == request.AccountId)
                .Select(u => new LookupUserResponse
                {
                    Nick = u.Name,
                    Timetamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                })
                .FirstOrDefaultAsync() ?? throw new ArgumentNullException();

            byte[] msgpackBytes = MessagePackSerializer.Serialize(userData);

            return msgpackBytes;
        }
    }
}
