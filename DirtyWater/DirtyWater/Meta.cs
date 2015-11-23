using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


/*
*/

namespace DirtyWater
{
    class Meta
    {

        static MySqlConnection conn;
        static MySqlCommand cmd;
        static MySqlDataReader rdr;
    
        static string time;
        static string query;
        static string connectionString =
                "server=127.0.0.1;" +
                "port=3306;"+
                "user=DirtyWater;"+
                "password=M3t4th351515k1n9;"+
                "database=meta;";

        public static bool Connect()
        {
            try
            {

                conn = new MySqlConnection(connectionString);
                conn.Open();
                return true;
            }
            catch (MySqlException e)
            {

                Console.WriteLine("ERROR: " + e.Number + "\n" + e);
                return false;

            }
        }
        

        enum Req : int{
                                            //EXAMPLE:
            LOGIN = 'L' << 8,               //@L.NIKOLITO........PASSWORD........   
            LOGOUT = ('L' << 8) | 'O',      //@LONIKOLITO........................

            REGISTER = 'R' << 8,            //@R.NIKOLITO........................

            BAN = 'B' << 8,                 //@B.NIKOLITO........................
            KICK = 'K' << 8                 //@K.NIKOLITO......0xTTTTTTTT........
            
        }

        public static void ParseIn(byte[] input){
            if (Connect()) //if we connect to the database successfully
            { 

                int request = ((int)input[1] << 8) | input[2];

                string[] data = new string[7]; // for now, parses the entire login
                                               // packet as 7 16-character strings
                                               // 0 - username
                                               // 1 - password hash
                                               // 2-6 - email address, other data fields
                                               //

                for (int i = 0; i < (input.Length/16)-1; i++)
                {
                    Console.WriteLine(i);
                    data[i] = Encoding.ASCII.GetString(input, (i*16)+3, 16).Trim('\0');
                    Console.WriteLine(data[i]);
                }

                switch (request)
                {
                    case (int)Req.LOGIN:
                        Console.WriteLine("Logging in user {0} with password {1}", data[0], data[1]);
                        Login(data[0], data[1]); //Recall that data[0] is the 16byte username, 
                        break;                   //and data[1] is the 16byte password hash.

                    case (int)Req.LOGOUT:  //Again, data[0] is the username
                        Logout(data[0]);
                        break;

                    case (int)Req.REGISTER:
                        Register(data[0], data[1], data[2] + data[3] + data[4] + data[5]); //username, password, email
                        break;

                    default: break;
                }

                try
                {
                    conn.Close();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\n" + e.Number);
                }

            }

        }


        public static void Login(string username, string passhash) {
            if (Validate(username, passhash))
            {
                time = string.Format("{0:yyyy.MM.dd 0:HH:mm:ss tt}", DateTime.Now);

                rdr = new MySqlCommand("SELECT lastin FROM login WHERE username='" + username + "';", conn).ExecuteReader();
                while (rdr.Read()) {
                    Console.WriteLine("User's last sign in was: " + rdr["lastin"]);
                }
                rdr.Close();

                new MySqlCommand("UPDATE login SET lastin='" + time + "' WHERE username='" + username + "';", conn).ExecuteNonQuery();
                
            }
            Console.WriteLine("HHHHhhhh");
         


            


            //TODO:
            //
            //If the username exists, check that the password hash matches.
            //
            //If the username does not exist, be like "No user found".
            //
            //Else, If the password hash matches, Sign in.
            //Set the current player to active
            //forward/bind socket to control an Object?
            //Note the timestamp and IP of the login in the database
            //Send all of the necessary information back to the client:

        }

        public static void Logout(string username)
        {

            //close the user's connection with the server
            //
            //Processes a MySQL query to log out the user
            //Log the logout timestamp
            //Save the time they were signed in for?

        }

        public static void Register(string username, string passhash, string email) {

            //Create a new SQL entry for username
            //As with login, mark time and IP of registration
            //Also mark their email address, for use in sending out mass emails and all that junk
            //
            //
            //
            //Return a prompt to login, as well as verifying the success of registration

        }

        public static bool Validate(string username, string passhash) {
            // A block to do the validation step common with most of the
            // meta requests. Checks the username and password against a 
            // the database, and also checks to see if 

            query = "SELECT * FROM login WHERE (username = '" + username + "')";
            cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            string loginInfo = "";

            while (rdr.Read())
            {
                Console.WriteLine("FUCK");
                loginInfo = rdr["password"]+"";
            }

            rdr.Close();

            if (passhash == loginInfo && loginInfo != "")
            {
                return true;
            }
            else if (loginInfo == "")
            {
                Console.WriteLine("User not found");
            }
            else
            {
                Console.WriteLine("Invalid Username or Password");
            }
            return false;

        }

        public static bool Validate(string Username) {

            //validates w/o the passhash, for logouts and all

            return true;
        }



    }

}
