namespace Server.Models.Entitys
{
    public class AddDeviceData
    {
        public Guid Id { get; set; }
        public string Payload { get; set; }
        public string TrustedSignature { get; set; }
        public string PayloadHash { get; set; }
        public string TempId { get; set; }
    }
}
