using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWaterClient
{
    class ClientMeta
    {
        /// <summary>
        /// INBOUND SECTION
        /// </summary>

        enum Notify : int
        {
            //EXAMPLE:
            LOGIN = 'L' << 8,               
            LOGOUT = ('L' << 8) | 'O',      

            REGISTER = 'R' << 8,            

            BAN = 'B' << 8,                 
            KICK = 'K' << 8                 

        }

        public static void ParseIn(ref byte[] data)
        {
            int reply = (data[0] << 8) | data[1];

            switch (reply)
            {
                case (int)Notify.LOGIN:
                    if (data[6] == 0) {
                        Console.WriteLine("You have successfully logged in. Welcome.");
                    }
                    break;

                case (int)Notify.LOGOUT:
                    if (data[6] == 0)
                    {
                        Console.WriteLine("You have successfully logged in. Welcome.");
                    }
                    break;

                case (int)Notify.REGISTER:
                    if (data[6] == 0)
                    {
                        Console.WriteLine("You have successfully logged in. Welcome.");
                    }
                    break;

                case (int)Notify.BAN:

                    break;

                case (int)Notify.KICK:

                    break;

                default: break;


            }

        }




        /// <summary>
        /// OUTBOUND SECTION
        /// </summary>

        public static string request = "";

        public static byte[] Login(string username, SecureString password) {
            request = "@L\x00" + username.PadRight(16, '\0') + ToUnsecureString(password).PadRight(16, '\0') + '?';  
            return Serialize(request);
        }

        public static byte[] Login(string username, string password)
        {
            request = "@L\x00" + username.PadRight(16, '\0') + password.PadRight(16, '\0') + '?';
            return Serialize(request);
        }

        public static byte[] Logout(string username) {

            request = "@LO" + username.PadRight(16, '\0') +'?';
            return Serialize(request);
        }


        public static byte[] Register(string username, SecureString password, string email) {

            request = "@R\x00" + username.PadRight(16, '\0') + ToUnsecureString(password).PadRight(16, '\0') + email.PadRight(64, '\0') + '?';
            return Serialize(request);

        }

        public static byte[] Register(string username, string password, string email)
        {

            request = "@R\x00" + username.PadRight(16, '\0') + password.PadRight(16, '\0') + email.PadRight(64, '\0') + '?';
            return Serialize(request);
        }


        public static byte[] Unregister(string username, SecureString password) {

            request = "@U\x00" + username.PadRight(16, '\0') + ToUnsecureString(password).PadRight(16, '\0') + '?'; 
            return Serialize(request);
        }

        public static byte[] Serialize(string input) {
            byte[] bytes = new byte[128];
            Array.Copy(Encoding.ASCII.GetBytes(input.PadRight(128, '\0')), bytes, 128);
            return bytes;
        }

        public static string ToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static string SaltNHash(string s) {
            return s;
        }

    }
}
