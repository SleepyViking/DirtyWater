using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace DirtyWaterClient
{
    public partial class GUI : Form
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public GUI()
        {
            InitializeComponent();
            //LoopConnect();
        }

        private static bool LoopConnect()
        {
            int attempts = 0;

            IPAddress[] IPs = Dns.GetHostAddresses("107.191.103.148");
            //IPAddress[] IPs = Dns.GetHostAddresses("127.0.0.1");

            while (!_clientSocket.Connected && attempts <10)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPs[0], 100);
                }
                catch (SocketException e)
                {

                }
            }
            if (_clientSocket.Connected)
            {
                return true;
            }
            return false;
        }

        private void RegisterButton_MouseUp(object sender, MouseEventArgs e)
        {
            //Commence Register here
        }

        private void LoginButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (LoopConnect()){
                _clientSocket.Send(ClientMeta.Login(UsernameBox.Text, PasswordBox.Text));
                // Make this more effecient ----------------
                byte[] receivedBuf = new byte[128];
                int rec = _clientSocket.Receive(receivedBuf);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuf, data, rec);
                // -----------------------------------------
                if (data[0] == '$' && data[1] == 'L' && data[2] == '\0')
                {
                    switch (data[6])
                    {
                        default:
                            _clientSocket.Disconnect(true);
                            break;

                        case (byte)'0': //Sucessful Login
                            LoginPane.Visible = false;
                            //Start Game XOXOXOXOXOXOXXOXOXOXOXOXOXOXOXXXXXXXXXXXXXXXXXXXXXXX
                            break;

                        case (byte)'1': //Invalid Username or Password
                            _clientSocket.Disconnect(true);
                            break;
                    }
                }
                _clientSocket.Disconnect(true);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RegisterPageButton_MouseEnter(object sender, EventArgs e)
        {
            RegisterPageButton.ForeColor = Color.White; 
        }

        private void RegisterPageButton_MouseLeave(object sender, EventArgs e)
        {
            RegisterPageButton.ForeColor = Color.Lime;
        }

        private void LoginButton_MouseEnter(object sender, EventArgs e)
        {
            LoginButton.BackColor = Color.FromArgb(32, 32, 32);
        }

        private void LoginButton_MouseLeave(object sender, EventArgs e)
        {
            LoginButton.BackColor = Color.DimGray;
        }

        private void RegisterPageButton_MouseUp(object sender, MouseEventArgs e)
        {
            RegisterPanel.Visible = true;
            LoginPane.Visible = false;
        }

        private void LoginPageButton_MouseEnter(object sender, EventArgs e)
        {
            LoginPageButton.ForeColor = Color.White;
        }

        private void LoginPageButton_MouseLeave(object sender, EventArgs e)
        {
            LoginPageButton.ForeColor = Color.Lime;
        }

        private void LoginPageButton_MouseUp(object sender, MouseEventArgs e)
        {
            LoginPane.Visible = true;
            RegisterPanel.Visible = false;
            
        }

        private void RegisterButton_MouseEnter(object sender, EventArgs e)
        {
            RegisterButton.BackColor = Color.FromArgb(32, 32, 32);
        }

        private void RegisterButton_MouseLeave(object sender, EventArgs e)
        {
            RegisterButton.BackColor = Color.DimGray;
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_clientSocket.Connected)
            {
                if (MessageBox.Show("You are still logged in... Are you sure you want to quit?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    //Log out here
                }
            }
        }
    }
}
