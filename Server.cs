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
            ret.AddRange([0x42,0x47,0x50,0x53,0x00]);

            return [..ret];
        }
    }
}