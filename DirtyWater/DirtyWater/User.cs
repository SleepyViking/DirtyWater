using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DirtyWater
{
    class User
    {
        private string username;
        private Socket socket;

        public User(string username) {
            this.username = username;


        }



        public void Disconnect() {
            Logout();
        }

        public void Logout() {
            Meta.Logout(username);
        }
        

    }
}
