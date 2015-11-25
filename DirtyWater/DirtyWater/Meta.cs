using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;


/*
*/

namespace DirtyWater
{
    class Meta
    {

        const int SALT_BYTE_SIZE = 16;
        const int HASH_BYTE_SIZE = 16;
        const int PBKDF2_ITERATIONS = 1024;
        const int ITERATION_INDEX = 0;
        const int SALT_INDEX = 1;
        const int PBKDF2_INDEX = 2;

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

        enum Err : int {
            


        }



        enum Req : int{
                                            //EXAMPLE:
            LOGIN = 'L' << 8,               //@L.NIKOLITO........PASSWORD........   
            LOGOUT = ('L' << 8) | 'O',      //@LONIKOLITO........................

            REGISTER = 'R' << 8,            //@R.NIKOLITO........................

            BAN = 'B' << 8,                 //@B.NIKOLITO........................
            KICK = 'K' << 8                 //@K.NIKOLITO......0xTTTTTTTT........
            
        }

        public static void ParseIn(ref byte[] input){
            if (Connect()) //if we connect to the database successfully
            { 

                int request = ((int)input[1] << 8) | input[2];
                int reply;

                string[] data = new string[7]; // for now, parses the entire login
                                               // packet as 7 16-character strings
                                               // 0 - username
                                               // 1 - password hash
                                               // 2-6 - email address

                for (int i = 0; i < (input.Length/16)-1; i++)
                {
                    data[i] = Encoding.ASCII.GetString(input, (i * 16) + 3, 16).Trim('\0');
                    Console.WriteLine(i + " - " + data[i] +" "+ data[i].Length);
                }



                switch (request)
                {
                    

                    case (int)Req.LOGIN:
                        Console.WriteLine("Logging in user {0} with password {1}...", data[0], data[1]);
                        reply = Login(data[0], data[1]); //Recall that data[0] is the 16byte username, 
                        
                        Console.WriteLine("Logged in.");
                        break;                   //and data[1] is the 16byte password hash.

                    case (int)Req.LOGOUT:  //Again, data[0] is the username
                        reply = Logout(data[0]);
                        break;

                    case (int)Req.REGISTER:
                        Console.WriteLine("Registering user {0}...", data[0], data[1]);
                        reply = Register(data[0], data[1], data[2] + data[3] + data[4] + data[5]); //username, password, email
                        Console.WriteLine("Registered.");
                        break;

                    default:
                        reply = -1;
                        break;
                }

                try
                {
                    conn.Close();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\n" + e.Number);
                }

                input[0] = (byte)'$';
                Array.Copy(new byte[125], 0, input, 3, 125); // White out the rest of the input

                input[3] = (byte)((reply & 0xFF000000) >> 24);
                input[4] = (byte)((reply & 0x00FF0000) >> 16);
                input[5] = (byte)((reply & 0x0000FF00) >> 8);
                input[6] = (byte)((reply & 0x000000FF) >> 0); // login error block

                

            }

        }


        public static int Login(string username, string passhash) {
            Console.WriteLine("> LOGIN: " + passhash + " " + passhash.Length);

            if (Validate(username, passhash))
            {

                time = string.Format("{0:yyyy.MM.dd 0:HH:mm:ss tt}", DateTime.Now);

                rdr = new MySqlCommand("SELECT lastin FROM login WHERE username='" + username + "';", conn).ExecuteReader();
                while (rdr.Read()) {
                    Console.WriteLine("User's last sign in was: " + rdr["lastin"]);
                }
                rdr.Close();

                new MySqlCommand("UPDATE login SET lastin='" + time + "' WHERE username='" + username + "';", conn).ExecuteNonQuery();

                return 0; //Login succeeds
            }
            return 1; //Validation failed

            //TODO:
            //Set the current player to active
            //forward/bind socket to control an Object?
            //Note the timestamp and IP of the login in the database
            //Send all of the necessary information back to the client:

        }

        public static int Logout(string username)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "UPDATE login VALUES (@username, @email, @hash, @salt, 'NEW USER', 'NEW USER')";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
                return 0;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e + "\n\n" + e.Number);
                return e.Number;
            }
            
            //close the current user's connection with the server
            //
            //Processes a MySQL query to log out the user
            //Log the logout timestamp
            //Save the time they were signed in for?

        }

        public static int Register(string username, string passhash, string email) {
            Console.WriteLine(">> REGISTER: " + passhash + " " + passhash.Length);
            //Create a new SQL entry for username
            //As with login, mark time and IP of registration
            //Also mark their email address, for use in sending out mass emails and all that junk
            // IF THE USER EXISTS, RETURN THAT THE USER EXISTS
            //Return a prompt to login, as well as verifying the success of registration


            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();

            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            

            byte[] hash = PBKDF2(passhash, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
                       
            try {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO login VALUES (@username, @email, @hash, @salt, 'NEW USER', 'NEW USER')";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@hash", hash);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.ExecuteNonQuery();
                return 0;
            } catch (MySqlException e)
            {
                Console.WriteLine(e + "\n\n" + e.Number);
                return 1;
            }

        }

        public static bool Validate(string username, string testhash) {
            // A block to do the validation step common with most of the
            // meta requests. Checks the username and password against a 
            // the database, and also checks to see if 
            Console.WriteLine("> VALIDATING " + username);


            query = "SELECT * FROM login WHERE (username = '" + username + "')";
            cmd = new MySqlCommand(query, conn);
            rdr = cmd.ExecuteReader();

            byte[] testHashBytes = new byte[HASH_BYTE_SIZE];
            byte[] correctHashBytes = new byte[HASH_BYTE_SIZE];
            byte[] saltBytes = new byte[SALT_BYTE_SIZE];
            try {
                while (rdr.Read())
                {
                    //Console.WriteLine("Reading...");
                    correctHashBytes = (byte[])rdr["hash"];

                    //Console.WriteLine("Correct Hash: " + Encoding.ASCII.GetString(correctHashBytes));
                    saltBytes = (byte[])rdr["salt"];

                    //Console.WriteLine("User's Salt: " + Encoding.ASCII.GetString(saltBytes));

                }
            }
            catch (MySqlException e) {
                Console.WriteLine(e + "\n\n " + e.Number);
            }

            testHashBytes = PBKDF2(testhash, saltBytes, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);


            rdr.Close();

            if (SlowEquals(correctHashBytes, testHashBytes) && correctHashBytes.Length > 0)
            {
                return true;
            }
            else if (correctHashBytes.Length < 0)
            {
                Console.WriteLine("User not found");
            }
            else
            {
                Console.WriteLine("Invalid Username or Password");
            }
            return false;

        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }


    }

}
