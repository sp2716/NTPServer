namespace NTPServer.Models;
using MemoryPack;

[MemoryPackable]
public partial class NtpPacket
{
    public byte Header { get; set; } //LI/VN/Mode/Strat/Poll/Prec
    public uint RootDelay { get; set; }
    public uint RootDispersion { get; set; }
    public uint ReferenceId { get; set; }
    /// <summary>
    /// Seconds Since 01.01.1900 expressed in 32-bits Seconds, 32-bits Fractions of a second
    /// </summary>
    public ulong ReferenceTimestamp { get; set; }
    public ulong OriginTimestamp { get; set; }
    public ulong RxTimestamp { get; set; }
    public ulong TxTimestamp { get; set; }
    public byte[] ToBytes()
    {
        var ret = new List<byte>();
        ret.AddRange(MemoryPackSerializer.Serialize(this).Skip(1).ToArray()); //skip the first byte - which is count required for deserialization
        return [..ret];
    }

    public static NtpPacket? Deserialize(byte[] bytes)
    {
        var ret = new List<byte>
        {
            0x08 //first byte is count of public members, add this back in for memorypack
        };
        ret.AddRange(bytes);
        return MemoryPackSerializer.Deserialize<NtpPacket>(ret.ToArray());
    }
}