using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWaterClient
{
    class MetaMenu
    {
        private string user = "";

        public MetaMenu(){


        }

        public byte[] Prompt()
        {
            Console.WriteLine(
                "Welcome to the DirtyWater sub-prealpha client main menu.\n" +
                "Please type one of the commands below to begin.\n" +
                " * register\n" +
                " * login\n"
            );

            bool valid = false;
            string input;

            while (!valid)
            {
                Console.WriteLine("{0}> ", user);
                input = Console.ReadLine();
                SecureString pass;

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
                        while (true) {
                            Console.WriteLine("Please enter your Email address: ");
                            email = Console.ReadLine();
                            Console.WriteLine("Create a username: ");
                            user = Console.ReadLine();
                            Console.WriteLine("Create a password: ");
                            pass = GetPassword();
                            Console.WriteLine("Repeat Password: ");
                            if (GetPassword() == pass) break;
                        }
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
            return password;
        }


    }
}
