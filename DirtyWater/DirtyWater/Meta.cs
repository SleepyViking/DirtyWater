using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWater
{
    class Meta
    {
        //Sql sql = new Sql();

        enum Req : int{

            LOGIN = 'L' << 8,           
            LOGOUT = ('L' << 8) | 'O',

            REGISTER = 'R' << 8,

            BAN = 'B' << 8,
            KICK = 'K' << 8

        }

        public static void ParseIn(byte[] input){
            int request = ((int)input[1] << 8) | input[2];
            string[] data = new string[4]; // for now, parses the entire login
                                           // packet as 4 32-character strings

            for (int i = 0; i < data.Length; i++) {
                data[i] = input.ToString().Substring((i*32) + 3); 
            }

            
            //TODO: parse the bytes as char[] 
            //Parse the strings out of it ????

            switch (request) {
                case (int)Req.LOGIN:

                    Login();


                    break;
                case (int)Req.LOGOUT: break;
                case (int)Req.REGISTER: break;
                default: break;
            }

        }


        public static void Login(string username, string passhash) {

            //TODO:

            //Set the current player to active
            //forward/bind socket to an Object?
            //Note the timestamp and IP of the login in the database
            //Send all of the necessary information back to the client:
        
        }













    }

}
