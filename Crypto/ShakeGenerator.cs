using Org.BouncyCastle.Crypto.Digests;
using System.Text;

namespace Server.Crypto
{
    public interface IShakeGenerator
    {
        string ComputeHash128(string input, int outputLength = 32);
    }
    public class ShakeGenerator : IShakeGenerator
    {
        public string ComputeHash128(string input, int outputLength = 32)
        {
            var digest = new ShakeDigest(128);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            digest.BlockUpdate(inputBytes, 0, inputBytes.Length);

            byte[] output = new byte[outputLength];
            digest.DoFinal(output, 0);
            return Convert.ToHexString(output).ToLowerInvariant();
        }
    }
}
