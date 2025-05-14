using Server.Crypto;
using System.Threading.Tasks;

namespace Server.Services.Validation
{
    public interface IProofOfWorkService
    {
        Task<bool> VerifyAsync(string accountId, string hashProof, string nonce, string PK);
    }
    public class ProofOfWorkService(IShakeGenerator shakeGenerator) : IProofOfWorkService
    {
        public async Task<bool> VerifyAsync(string accountId, string hashProof, string nonce, string PK)
        {
            try
            {
                // На сервере нужно вычислить хеш от publicKey + nonce
                string input = PK + nonce;
                string recomputedHash = Convert.ToBase64String(await shakeGenerator.ComputeHash128(input)).ToLowerInvariant();

                // Проверяем, что хеш соответствует hashProof, полученному от клиента
                if (recomputedHash != hashProof)
                    return false;

                // Проверка сложности (префикс "000")
                if (!recomputedHash.StartsWith("000"))
                    return false;

                // Проверка совпадения ID аккаунта с вычисленным
                var computeId = Convert.ToBase64String(await shakeGenerator.ComputeHash256(Convert.FromBase64String(PK), 64));
                if (computeId != accountId)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
