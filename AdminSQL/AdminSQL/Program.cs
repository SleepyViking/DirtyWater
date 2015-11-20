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
            Login();
            Console.Read();
        }

        public static void Login() {
            if (conn.AttemptLogin("mime490", "Deidre<3"))
            {
                Console.WriteLine("Successful Login");
            }
            else
            {
                Console.WriteLine("Unsuccessful Login");
            }
        }
    }
}
