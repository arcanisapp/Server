namespace Server.Models.Dto.Account.Create
{
    public class RegisterPreKeyRequest
    {
        public string Id { get; set; }
        public byte[] PK { get; set; } //Base 64
        public byte[] PKSignature { get; set; } //Base 64
    }


}
