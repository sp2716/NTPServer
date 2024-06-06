// See https://aka.ms/new-console-template for more information
//var server = new NTPServer.NtpServer();
//server.Start();
//Console.ReadKey();
//server.Stop();
using NTPServer.Models;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;



Console.WriteLine();

var client = new UdpClient("pool.ntp.org", 123);
var packet = new List<byte>
    {
    0b_0001_1011 //flags => Leap Indicator 0b_00 no warning leap indicator (0), 0b_xx011 ntp version 3 (3), 0b_xxxx_x011 mode = client (3)
};
var ts = DateTime.Now;
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
Console.WriteLine(JsonConvert.SerializeObject(ntp, Formatting.Indented));
Console.ReadKey();