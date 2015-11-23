using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWaterClient
{
    class ClientMeta
    {

        public static string request = "";


        public static byte[] Login(string username, SecureString password) {
            request = "@L\x00" + username.PadRight(16, '\0') + ToUnsecureString(password).PadRight(16, '\0');  
            return Serialize(request);
        }


        public static byte[] Logout(string username) {

            request = "@LO";
            return Serialize(request);
        }


        public static byte[] Register(string username, SecureString password, string email) {

            request = "@R\x00";
            return Serialize(request);
        }


        public static byte[] Unregister(string username, SecureString password) {

            request = "@U\x00";
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

    }
}
