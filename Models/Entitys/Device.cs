namespace Server.Models.Entitys
{
    public class Device
    {
        /// <summary>
        /// Shake256 (SPK)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Device name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Device account id
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Device account
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Dilitium 5 public key
        /// </summary>
        public byte[] SPK { get; set; }

        /// <summary>
        /// Dilitium 5 signature of the AccountId
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        /// Device prekeys
        /// </summary>
        public List<PreKey> PreKeys { get; set; }
    }
}
