using Server.Crypto;

namespace Server.Services
{
    public interface IProofOfWorkService
    {
        bool VerifyAsync(string accountId, string hashProof, string nonce, string PK);
    }
    public class ProofOfWorkService(IShakeGenerator shakeGenerator) : IProofOfWorkService
    {
        public bool VerifyAsync(string accountId, string hashProof, string nonce, string PK)
        {
            try
            {
                var computeId = shakeGenerator.ComputeHash128(PK);

                if (computeId != accountId)
                    return false;

                var recomputedHash = shakeGenerator.ComputeHash128(hashProof + nonce);

                if (!recomputedHash.StartsWith("00000"))
                    return false;

                if (recomputedHash != hashProof)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
