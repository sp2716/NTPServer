namespace NTPServer
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    public class NtpServer
    {
        private volatile bool running = false;
        public NtpServer()
        {
            
        }
        public void Stop()
        {
            running = false;
        }
        public void Start()
        {
            running = true;
            new Thread(new ThreadStart(ListenerThread)).Start();
        }
        private async void ListenerThread()
        {
            Console.WriteLine("Starting NTP Server Listener thread on UDP Port 123");
            var server = new UdpClient(123);
            var ep = new IPEndPoint(0,0);   
            while (running)
            {
                var dgram = server.Receive(ref ep);
                //if something is received just hit pool.ntp.org and push the message back to the client...
                
                var send = Reply();
                server.Send(send, send.Length);
            }
            Console.WriteLine("Stopping NTP Server thread");
        }
        private byte[] GetTime(byte[] request)
        {
            var ret = new List<byte>();
            using (var client = new UdpClient("pool.ntp.org", 123))
            {
                var rec = client.Send(request);
                //client.Receive()
                //return 
            }
            return [.. ret];
        }
        private static byte[] Reply()
        {
            var ret = new List<byte>();
            ret.AddRange([0x1c, 0x01, 0x00, 0xe9]);
            ret.AddRange(new byte[7]);
            ret.AddRange(GetNtp());

            return [..ret];
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