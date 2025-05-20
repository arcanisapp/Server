﻿using MessagePack;

namespace Server.Models.Dto.Device.Confirm
{
    [MessagePackObject]
    internal class NewDevicePublicPayloadRequest
    {
        [Key(0)] public string AccountId { get; set; }
        [Key(1)] public string DeviceId { get; set; }
        [Key(2)] public string TrustedDeviceId { get; set; }
        [Key(3)] public string Name { get; set; }
        [Key(4)] public byte[] SPK { get; set; }
        [Key(5)] public byte[] SPKSignature { get; set; }
    }
}
