namespace Server.Models.Entitys
{
    public class Account
    {
        /// <summary>
        /// Shake256 first device (SPK)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Account devices
        /// </summary>
        public List<Device> Devices { get; set; }
    }
}
