namespace Server.Models.Dto.Account.Create
{
    public class RegisterPreKeyRequest
    {
        public string Id { get; set; }
        public string PK { get; set; } //Base 64
        public string PKSignature { get; set; } //Base 64
    }


}
