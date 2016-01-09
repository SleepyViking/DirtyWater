using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DirtyWater
{
    class Program
    {
        private static byte[] _buffer = new byte[128];
        private static List<Socket> _clientSockets = new List<Socket>();

        private static List<User> _users = new List<User>();

        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        MySqlConnection conn = new MySqlConnection();

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
            try
            {

                Socket socket = _serverSocket.EndAccept(AR);
                _clientSockets.Add(socket);
                Console.WriteLine("Client Connected");
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                Console.WriteLine("Waiting on Client...");
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            }
            catch (SocketException e)
            {

                Console.WriteLine("!!!... C L I E N T   D I S C O N N E C T E D...!!!");
                Console.WriteLine(e);

            }
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Console.WriteLine("Data Recieved from Client");
                Socket socket = (Socket)AR.AsyncState;
                int received = socket.EndReceive(AR);
                byte[] dataBuf = new byte[received];

                Console.WriteLine("Processing data...");

                Array.Copy(_buffer, dataBuf, received);

                string text = Encoding.ASCII.GetString(dataBuf);

                Console.WriteLine("Text received: " + text);

                switch (dataBuf[0])
                {

                    case (byte)'@':
                        Meta.ParseIn(ref dataBuf);
                        break;

                    case (byte)'!':
                        World.ParseIn(dataBuf);
                        break;

                    default:
                        Console.WriteLine(Encoding.ASCII.GetString(dataBuf));
                        break;

                }

                if (IsConnected(socket))
                {

                    socket.BeginSend(dataBuf, 0, dataBuf.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);

                }

            }

            catch (SocketException e)
            {
                Console.WriteLine("!!!...C L I E N T   D I S C O N N E C T E D...!!!");
                Console.WriteLine(e);
                Console.WriteLine("Logging out user....\nLogout" + Meta.Logout("PLACEHOLDER")); 

            }

        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

        public static bool IsConnected(Socket socket)
        {
            try
            {
                if (!(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0))
                {
                    return true;
                }


            }
            catch (SocketException e) {

                Console.WriteLine(e);

            }

            _clientSockets.Remove(socket);
            return false;

        }

    }

}