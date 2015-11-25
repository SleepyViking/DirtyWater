using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWaterClient
{
    class Program
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Title = "Client";
            LoopConnect();
            SendLoop(new MetaMenu());
            Console.ReadLine();
        }

        private static void SendLoop(MetaMenu m)
        {
            //bool sending;

            while (true)
            {


                byte[] buffer = m.Prompt();
                Console.WriteLine("Request Sending...");
                _clientSocket.Send(buffer);
                Console.WriteLine("Request Sent");

                byte[] receivedBuf = new byte[128];
                int rec = _clientSocket.Receive(receivedBuf);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuf, data, rec);

                switch (data[0]) {
                    case (byte)'$':             // >> Notifications
                        ClientMeta.ParseIn(ref data);
                        break; 
                    case (byte)'#': break;      // >> Map data
                    case (byte)'%': break;      // >> Entity/player stats
                    default:
                        Console.WriteLine("Odd packet received: " + Encoding.ASCII.GetString(data));
                        break;
                    
                }

                Console.WriteLine("Received: " + Encoding.ASCII.GetString(data));
                

            }
        }

        private static void LoopConnect()
        {
            int attempts = 0;

            //IPAddress[] IPs = Dns.GetHostAddresses("107.191.103.148");
            IPAddress[] IPs = Dns.GetHostAddresses("127.0.0.1");

            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPs[0], 100);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine("Connection attempts: " + attempts.ToString());
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }
    }
}
