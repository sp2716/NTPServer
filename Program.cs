// See https://aka.ms/new-console-template for more information
//var server = new NTPServer.NtpServer();
//server.Start();
//Console.ReadKey();
//server.Stop();
using NTPServer;
var server = new NtpServer();
Console.WriteLine("creating ntp server instance");
server.Start();
Console.ReadLine();
Console.WriteLine("ntp server stopping");
server.Stop();

/*
Console.WriteLine();

var client = new UdpClient("pool.ntp.org", 123);
var packet = new List<byte>
    {
    0b_0001_1011 //flags => Leap Indicator 0b_00 no warning leap indicator (0), 0b_xx011 ntp version 3 (3), 0b_xxxx_x011 mode = client (3)
};
var ts = DateTimeOffset.Now.ToUnixTimeSeconds();
packet.AddRange(new byte[47]);
client.Send([.. packet]);
var ep = new IPEndPoint(0, 123);
var rec = client.Receive(ref ep);
foreach (var bite in rec)
{
    Console.Write(bite.ToString("X2"));
}
Console.WriteLine();
var ntp = NtpPacket.Deserialize(rec);
var dt = NtpToDateTime(rec.TakeLast(8).ToArray());
Console.WriteLine(BitConverter.ToString(rec.TakeLast(8).ToArray()));
Console.WriteLine(ConvertToNtp(DateTime.Now.Ticks));
var seconds = (ntp.ReferenceTimestamp & 0x00000000FFFFFFFF);
var fract = (ntp.ReferenceTimestamp & 0xFFFFFFFF00000000);
var minutes = seconds / 60;
var hours = seconds / 3600;
var years = seconds / (3600 * 24 * 365.25);
Console.WriteLine($"{seconds}.{fract} seconds => {years} years");
Console.WriteLine(years);
var epoch = new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
var count = (DateTime.Now - epoch).TotalMilliseconds;
var result = ConvertToNtp((decimal)count);

Console.WriteLine($"{epoch}\t{dt:HH:mm:ss.fffffff}\t{BitConverter.ToString(result)}");
Console.WriteLine(JsonConvert.SerializeObject(ntp, Formatting.Indented));
Console.ReadKey();


static DateTime NtpToDateTime(byte[] ntpTime)
{
    decimal intpart = 0, fractpart = 0;

    for (var i = 0; i <= 3; i++)
        intpart = 256 * intpart + ntpTime[i];
    for (var i = 4; i <= 7; i++)
        fractpart = 256 * fractpart + ntpTime[i];

    var milliseconds = intpart * 1000 + ((fractpart * 1000) / 0x100000000L);

    Console.WriteLine("milliseconds: " + milliseconds);
    Console.WriteLine("intpart:      " + intpart);
    Console.WriteLine("fractpart:    " + fractpart);
    var epoch = new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    return epoch.AddMilliseconds((double)milliseconds);
}
static byte[] ConvertToNtp(decimal milliseconds)
{
    decimal intpart = 0, fractpart = 0;
    var ntpData = new byte[8];

    intpart = milliseconds / 1000;
    fractpart = ((milliseconds % 1000) * 0x100000000L) / 1000m;

    Console.WriteLine("milliseconds: " + milliseconds);
    Console.WriteLine("intpart:      " + intpart);
    Console.WriteLine("fractpart:    " + fractpart);

    var temp = intpart;
    for (var i = 3; i >= 0; i--)
    {
        ntpData[i] = (byte)(temp % 256);
        temp = temp / 256;
    }

    temp = fractpart;
    for (var i = 7; i >= 4; i--)
    {
        ntpData[i] = (byte)(temp % 256);
        temp = temp / 256;
    }
    return ntpData;
}
*/