using MessagePack;

namespace Server.Models.Dto.Contact.Lookup
{
    [MessagePackObject]
    public class LookupUserRequest
    {
        [Key(0)] public string DeviceId { get; set; }
        [Key(1)] public string AccountId { get; set; }
        [Key(2)] public long Timestamp { get; set; }
    }
}
