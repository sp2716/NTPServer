namespace NTPServer
{
    using NTPServer.Models;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class NtpServer
    {
        CancellationTokenSource? cts = null;
        public NtpServer()
        {
            
        }
        public void Stop()
        {
            if(cts != null)
            {
                cts.Cancel();
                cts = null;
            }
        }
        public void Start()
        {
            if(cts == null)
            {
                var cts = new CancellationTokenSource();
                Task.Factory.StartNew(() => ListenerThread(cts.Token), cts.Token);
            }
        }
        private async void ListenerThread(CancellationToken ct)
        {
            ct.Register(() => { Console.WriteLine("Cancelling"); return; });
            Console.WriteLine("Starting NTP Server Listener thread on UDP Port 123");
            var ep = new IPEndPoint(0, 0);
            var server = new UdpClient(123);
            while (true)
            {
                var dgram = server.Receive(ref ep);
                //if something is received just hit pool.ntp.org and push the message back to the client...
                Console.WriteLine($"Received {dgram.Length} bytes as request {BitConverter.ToString(dgram)} from {ep.Address}:{ep.Port}. Replying...");
                var send = Reply(dgram);
                server.Send(send, send.Length, ep);
            }
        }
        private static byte[] Reply(byte[] rec)
        {
            var recNtp = NtpV3Packet.Deserialize(rec) ?? new NtpV3Packet(); 
            var bytes = GetNtp();
            Console.WriteLine(BitConverter.ToString(bytes));
            ulong time = BitConverter.ToUInt64(bytes);
            var ntppacket = new NtpV4Packet
            {
                Header = 0xa4,
                ReferenceId = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("NIST")),
                ReferenceTimestamp = time,
                OriginTimestamp = recNtp.TxTimestamp,
                RxTimestamp = time,
                TxTimestamp = time + 100, //fool a delay
            };
            //Calculate root dispersion, 
            return ntppacket.ToBytes();
        }
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
        static byte[] GetNtp()
        {
            var epoch = new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var milliseconds = (decimal)(DateTime.Now - epoch).TotalMilliseconds;
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
    }
}