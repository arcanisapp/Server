using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Crypto;
using Server.Data;
using Server.Models.Dto.Account.Create;
using Server.Models.Entitys;
using Server.Services.Validation;
using System.Text;

namespace Server.Services
{
    public interface IAccountService
    {
        Task<bool> CreateAccountAsync(byte[] requestSignature, string rawJson);
    }
    public class AccountService(
        AppDbContext dbContext,
        IProofOfWorkService proofOfWorkService,
        IHCaptchaService hCaptchaService,
        ITimestampValidator timestampValidator,
        IMlDsaKeyVerifier mlDsaKeyVerifier) : IAccountService
    {
        public async Task<bool> CreateAccountAsync(byte[] requestSignature, string rawJson)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };

                var request = JsonConvert.DeserializeObject<CreateAccountRequest>(rawJson, settings);

                if (!timestampValidator.IsValid(request.Timestamp))
                    return false;

                if (!await hCaptchaService.VerifyAsync(request.ChaptchaToken))
                    return false;

                if (request.Device.PreKeys.Count < 50)
                    return false;

                if (!await mlDsaKeyVerifier.VerifyAsync(Convert.FromBase64String(request.Device.SPK), Encoding.UTF8.GetBytes(rawJson), requestSignature))
                    return false;

                if (!await proofOfWorkService.VerifyAsync(request.Id, request.ProofOfWork, request.Nonce, request.Device.SPK))
                    return false;

                if(await dbContext.Accounts.AnyAsync(a => a.Id == request.Id))
                    return false;

                Account newAccount = new();
                newAccount.Id = request.Id;
                newAccount.Name = request.Username ?? throw new ArgumentNullException();
                newAccount.Devices = [];
                newAccount.Devices.Add(new Device()
                {
                    Id = request.Device.Id,
                    SPK = request.Device.SPK,
                    Signature = request.Device.Signature,
                    PreKeys = []
                });
                foreach (var preKey in request.Device.PreKeys)
                {
                    newAccount.Devices[0].PreKeys.Add(new PreKey()
                    {
                        Id = preKey.Id,
                        PK = preKey.PK,
                        PKSignature = preKey.PKSignature
                    });
                }
                await dbContext.Accounts.AddAsync(newAccount);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
