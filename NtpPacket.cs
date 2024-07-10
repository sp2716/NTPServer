namespace NTPServer.Models;
using MemoryPack;
using System.Text;
[MemoryPackable]
public partial class NtpV3Packet
{
    public byte Header { get; set; } //LI/VN/Mode/Strat/Poll/Prec
    public byte Stratum { get; set; } = 2;
    public byte PollingInterval { get; set; } = 0;
    public byte PeerClockPrecision { get; set; } = 0;
    public uint RootDelay { get; set; }
    public uint RootDispersion { get; set; }
    public uint ReferenceId { get; set; }
    public ulong ReferenceTimestamp { get; set; } = 0;
    public ulong OriginTimestamp { get; set; } = 0;
    public ulong RxTimestamp { get; set; } = 0;
    public ulong TxTimestamp { get; set; } = 0;

    public static NtpV3Packet? Deserialize(byte[] bytes)
    {
        var ret = new List<byte>
        {
            11 //first byte is count of public members, add this back in for memorypack
        };
        ret.AddRange(bytes);
        return MemoryPackSerializer.Deserialize<NtpV3Packet>(ret.ToArray());
    }
    public byte[] ToBytes()
    {
        return MemoryPackSerializer.Serialize(this).Skip(1).ToArray();
    }
}

[MemoryPackable]
public partial class NtpV4Packet
{
    public byte Header { get; set; } //LI/VN/Mode/Strat/Poll/Prec
    public byte Stratum { get; set; } = 1;
    public byte PollingInterval { get; set; } = 8;
    public byte PeerClockPrecision { get;set; } = 0xec;
    public uint RootDelay { get; set; }
    public uint RootDispersion { get; set; }
    public uint ReferenceId { get; set; } 
    /// <summary>
    /// Seconds Since 01.01.1900 expressed in 32-bits Seconds, 32-bits Fractions of a second
    /// </summary>
    public ulong ReferenceTimestamp { get; set; } = 0;
    public ulong OriginTimestamp { get; set; } = 0;
    public ulong RxTimestamp { get; set; } = 0;
    public ulong TxTimestamp { get; set; } = 0;
    public byte[] ToBytes()
    {
        var ret = new List<byte>();
        ret.AddRange(MemoryPackSerializer.Serialize(this).Skip(1).ToArray()); //skip the first byte - which is count required for deserialization
        return [..ret];
    }

    public static NtpV4Packet? Deserialize(byte[] bytes)
    {
        var ret = new List<byte>
        {
            11 //first byte is count of public members, add this back in for memorypack
        };
        ret.AddRange(bytes);
        return MemoryPackSerializer.Deserialize<NtpV4Packet>(ret.ToArray());
    }
}