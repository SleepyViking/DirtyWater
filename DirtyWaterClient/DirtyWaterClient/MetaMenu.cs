using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWaterClient
{
    class MetaMenu
    {
        private string user = "";

        public MetaMenu(){

            Console.WriteLine(
            "Welcome to the DirtyWater sub-prealpha client main menu.\n" +
            "Please type one of the commands below to begin.\n" +
            " * register\n" +
            " * login\n"
            );

        }

        public void ParseReply(byte[] data)
        {
            int reply = (data[1] << 8) | data[2];

            switch (reply)
            {
                case (int)ClientMeta.Notify.LOGIN:
                    if (data[6] == 0)
                    {
                        Console.WriteLine("You have successfully logged in. Welcome!");
                    }
                    else
                    {
                        Console.WriteLine("Your Login is incorrect.");
                        user = "";
                    }
                    break;

                case (int)ClientMeta.Notify.LOGOUT:
                    if (data[6] == 0)
                    {
                        Console.WriteLine("You have successfully logged out. Gday.");
                        user = "";
                    }
                    break;

                case (int)ClientMeta.Notify.REGISTER:
                    if (data[6] == 0)
                    {
                        Console.WriteLine("You have successfully Registered. Welcome to Dungeonlord Godpunch.");
                    }
                    else if (data[6] == 2)
                    {
                        Console.WriteLine("That account name is taken, please try again.");
                    }
                    else if (data[6] == 3)
                    {
                        Console.WriteLine("That account name is either too long or has disallowed characters. Try again.");
                    }
                    else
                    {
                        Console.WriteLine("Your registration failed for uncertain reasons. Please inform the Developers, and try again.");
                    }
                    user = "";
                    break;

                case (int)ClientMeta.Notify.BAN:
                    user = "";
                    break;

                case (int)ClientMeta.Notify.KICK:
                    user = "";
                    break;

                default: break;


            }

        }


        public byte[] Prompt()
        {

            bool valid = false;
            string input;

            while (!valid)
            {
                Console.Write("{0}> ", user);
                input = Console.ReadLine();
                SecureString pass;
                //string pass = "";

                switch (input)
                {

                    
                    case "login":
                        Console.Write("Username: ");
                        user = Console.ReadLine();
                        Console.Write("Password: ");
                        pass = GetPassword();
                        return ClientMeta.Login(user, pass);

                    case "register":
                        string email = "";
                        do {
                            Console.Write("Please enter your Email address: ");
                            email = Console.ReadLine();
                            Console.Write("Create a username: ");
                            user = Console.ReadLine();
                            Console.Write("Create a password: ");
                            pass = GetPassword();
                            Console.Write("Repeat Password: ");
                        } while (!IsEqualTo(GetPassword(), pass));

                        return ClientMeta.Register(user, pass, email);

                    default:
                        Console.WriteLine("Sorry, that command is not recognized.");
                        break;
                }

                return new byte[128];
            }
            return new byte[128];

        }

        private static SecureString GetPassword() {
            SecureString password = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter) break;
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.RemoveAt(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else {
                    password.AppendChar(i.KeyChar);
                    Console.Write("*");
                }

            }
            Console.Write("\n");
            return password;
        }

        public static bool IsEqualTo(SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }









    }
}
