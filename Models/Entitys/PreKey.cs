namespace Server.Models.Entitys
{
    public class PreKey
    {
        /// <summary>
        /// Shake256 (PK)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Device id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Device
        /// </summary>
        public Device Device { get; set; }
        /// <summary>
        /// Kyber 1024 public key
        /// </summary>
        public string PK { get; set; }

        /// <summary>
        /// Dilitium 5 device signature of the public key
        /// </summary>
        public string PKSignature { get; set; }

        public bool IsUsed { get; set; }
    }
}
