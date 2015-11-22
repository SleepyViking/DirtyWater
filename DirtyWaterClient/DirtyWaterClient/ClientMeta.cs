using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWaterClient
{
    class ClientMeta
    {

        public static string request = "";

        public static void Login(string username, SecureString password) {
            if (Validate(username, password) == 0)
            {
                request = "@L\x00" + username.PadRight(16, '\0') + PassHash(password).PadRight(16, '\0');  

            }
        }

        public static void Logout(string username) {
            if (Validate(username) == 0)
            {
                request = "@LO";
            }
            

        }

        public static void Register(string username, SecureString password, string email) {
            if (Validate(username, password) == 0)
            {
                request = "@R\x00";
            }
        }

        public static void Unregister(string username, SecureString password) {
            if (Validate(username, password) == 0)
            {
                request = "@U\x00";
            }
        }

        public static byte Validate(string username, SecureString password) {
            //salt and hash the password HERE before sending it along to the 
            //string assembly methods
            return 0;
        }

        public static byte Validate(string username)
        {
            //Check to see if Username exists, and if it does, if the password matches.
            //done before any other processing.
            //Should check their flags to see if they are banned, temporarily or permanently.

            return 0;
        }

        public static string PassHash(SecureString s) {


            s.Dispose();
            return "";
        }

        public static byte[] Serialize(string input) {
            byte[] bytes = new byte[128];
            return bytes;
        }


    }
}
