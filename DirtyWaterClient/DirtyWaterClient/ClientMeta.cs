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

        public enum Notify : int
        {
            //EXAMPLE:
            LOGIN = 'L' << 8,               
            LOGOUT = ('L' << 8) | 'O',      

            REGISTER = 'R' << 8,            

            BAN = 'B' << 8,                 
            KICK = 'K' << 8                 

        }

        /// <summary>
        /// OUTBOUND SECTION
        /// </summary>

        public static string request = "";

        public static byte[] Login(string username, SecureString password){
            byte[] pass = ToBytes(password);    //pass is now the unsecure bytes in password
            password.Dispose();                 //password is destroyed
            HashPassword(ref pass, username);   //pass is transformed to the hashed bytes

            request = "@L\x00" + username.PadRight(16, '\0') + Encoding.ASCII.GetString(pass) + '?';
            return Serialize(request);
        }

        public static byte[] Logout(string username) {
            request = "@LO" + username.PadRight(16, '\0') +'?';

            return Serialize(request);
        }

        public static byte[] Register(string username, SecureString password, string email) {
            byte[] pass = ToBytes(password);    //pass is now the unsecure bytes in password
            password.Dispose();                 //password is destroyed
            HashPassword(ref pass, username);   //pass is transformed to the hashed bytes

            request = "@R\x00" + username.PadRight(16, '\0') + Encoding.ASCII.GetString(pass) + email.PadRight(64, '\0') + '?';
            return Serialize(request);

        }

        public static byte[] Unregister(string username, SecureString password) {
            byte[] pass = ToBytes(password);    //pass is now the unsecure bytes in password
            password.Dispose();                 //password is destroyed
            HashPassword(ref pass, username);   //pass is transformed to the hashed bytes

            request = "@U\x00" + username.PadRight(16, '\0') + Encoding.ASCII.GetString(pass) + '?'; 
            return Serialize(request);
        }

        public static byte[] Serialize(string input) {
            byte[] bytes = new byte[128];
            Array.Copy(Encoding.ASCII.GetBytes(input.PadRight(128, '\0')), bytes, 128);
            return bytes;
        }

        public static void HashPassword(ref byte[] password, string username)
        {
            byte[] salt = new byte[32];
            salt = Encoding.ASCII.GetBytes("DNGNLORDGODPUNCH" + username);

            password = PBKDF2(ref password, salt, 1024, 16);
        }



        public static byte[] ToBytes(SecureString securePassword)
        {
            Encoding encoding = Encoding.ASCII;

            if (securePassword == null) {
                throw new ArgumentException(nameof(SecureString));
            }

            IntPtr umString = IntPtr.Zero;

            try
            {
                umString = Marshal.SecureStringToGlobalAllocAnsi(securePassword);
                return encoding.GetBytes(Marshal.PtrToStringAnsi(umString));
            }
            finally {
                if(umString != IntPtr.Zero) {
                    Marshal.ZeroFreeGlobalAllocAnsi(umString);
                }

            }

        }

        private static byte[] PBKDF2(ref byte[] password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            //pbkdf2.IterationCount = iterations;
            for (int i = 0; i < password.Length; i++) {
                password[i] = 0; //Manually destroy the plaintext password in bytes
            }

            return pbkdf2.GetBytes(outputBytes); //at the call location, password is transformed into these bytes
        }


    }
}
