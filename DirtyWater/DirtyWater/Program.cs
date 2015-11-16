using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWater
{
    class Program
    {
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Client Connected");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            Console.WriteLine("Waiting on Client...");
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Console.WriteLine("Data Recieved from Client");
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuf = new byte[received];
            Console.WriteLine("Processing data...");
            Array.Copy(_buffer, dataBuf, received);

            string text = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine("Text received: " + text);

            //string response = string.Empty;

            byte[] data;

            if (text[0] == '@')
            {
                data = Encoding.ASCII.GetBytes(ParseInput(text));
            }
            else {
                data = Encoding.ASCII.GetBytes(text);
            }

            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }


        private static String ParseInput(String s) {
            String report = "";
            String name = "";
            String pass = "";

            int i = 1;
            switch(s[i++]){
                case 'L':
                    report += "Login initiated, ";
                    break;
                case 'N':
                    report += "New account created, ";
                    break;
                case 'U':
                    report += "Account Unregistered, ";
                    break;
                default:
                    report += "Undefined";
                    break;                    
            }

            if (s[i] == '/') {
                while(s[++i] != '/')
                {
                    name += s[i];
                }
                while (s[++i] != '?') // ADD A THING FOR SAFETY
                {
                    pass += s[i];
                }
            }

            report += "\nN: " + name + "\nP: " + pass;
            Console.WriteLine(report);

            if (pass == "Hella")
            {
                return "Affirmative";
            }
            else {
                return "Negative";
            }

        }


    }
}
