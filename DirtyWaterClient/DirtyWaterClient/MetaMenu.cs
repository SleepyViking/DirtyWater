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

        string user;

        public MetaMenu(){

            user = "";

        }

        private void Start() {
            bool running = true;
            while (running)
            {
                Console.WriteLine(
                    "Welcome to the DirtyWater sub-prealpha menu.\n" +
                    "Please type one of the commands below to begin.\n" +
                    " * register\n" +
                    " * login"
                );

                Prompt(user);
            }
        }

        private void Prompt(string user)
        {
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
                        Console.WriteLine("Username: ");
                        user = Console.ReadLine();
                        Console.WriteLine("Password: ");
                        pass = GetPassword();
                        ClientMeta.Login(user, pass);
                        break;

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
                        ClientMeta.Register(user, pass, email);
                        break;

                    default:
                        Console.WriteLine("Sorry, that command is not recognized.");
                        break;
                }
            }

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
