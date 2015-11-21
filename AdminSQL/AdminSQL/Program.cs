using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSQL
{
    class Program
    {
        static DBConnection conn = new DBConnection();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Dirty Database Admin Experience...\n" +
                "Please choose a path or type help for more options");
            Input();
            Console.Read();
        }

        public static void Input()
        {
            Console.Write(">");
            string action = Console.ReadLine();
            switch (action.ToLower())
            {
                default:
                    Console.WriteLine("Invalid Command... For assistance type \"help\"");
                    break;
                case ("help"):
                    Console.WriteLine("register - Register a new user in the Dirty Database");
                    Console.WriteLine("login - Login with a registered user");
                    Console.WriteLine("read - Read the entire login table");
                    break;
                case ("register"):
                    CreateUser();
                    break;
                case ("login"):
                    Login();
                    break;
                case ("read"):
                    ReadDB();
                    break;

             }
             Input();
            
        }

        public static void Login()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            if (conn.AttemptLogin(username, password))
            {
                Console.WriteLine("Successful Login");
            }
            else
            {
            }
            Input();
        }

        public static void CreateUser()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            if (conn.CreateUser(username, password))
            {
                Console.WriteLine("User Created");
            }
            else
            {
                Console.WriteLine("Creation Failed");
            }
            Input();
        }

        public static void ReadDB()
        {
            if (conn.ReadDB())
            {
            }
            else
            {
                Console.WriteLine("Read Failed");
            }
            Input();
        }
    }
}
