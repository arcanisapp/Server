namespace Server.Models.Entitys
{
    public class AddDeviceData
    {
        public Guid Id { get; set; }
        public byte[] Payload { get; set; }
        public byte[] TrustedSignature { get; set; }
        public byte[] PayloadHash { get; set; }
        public string TempId { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
