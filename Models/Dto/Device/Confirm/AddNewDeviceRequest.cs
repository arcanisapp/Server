using MessagePack;
using System.Collections.Immutable;

namespace Server.Models.Dto.Device.Confirm
{
    [MessagePackObject]
    public class AddNewDeviceRequest
    {
        [Key(0)] public byte[] DevicePayload { get; set; }

        [Key(1)] public byte[] DeviceTrustedSignature { get; set; }

        [Key(2)] public ImmutableArray<AddNewDevicePreKeysRequest> PreKeysPayload { get; set; }

        [Key(3)] public long Timestamp { get; set; }

    }
}
