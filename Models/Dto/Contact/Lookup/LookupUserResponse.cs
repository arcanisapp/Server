using MessagePack;

namespace Server.Models.Dto.Contact.Lookup
{
    [MessagePackObject]
    public class LookupUserResponse
    {
        [Key(0)] public string Nick { get; set; }
        [Key(1)] public long Timetamp { get; set; }
    }
}
