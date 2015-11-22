using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWater
{
    class World
    {

        static int x = 0;
        static int y = 0;


        
        enum Req : int {

            MOVE = 'M'<<8,                  //move the sender's entity
            MOVE_JUMP = ('M' << 8) | 'J',   //jump (movement over objects, with respect to player's jump-height and 
            MOVE_RUN = ('M' << 8) | 'R',    //sprint, increasing range per tick, expending energy

            SET_LOCATION = 'S' << 8,         //Set the player's absolute location

            ATT_MELEE = ('A' << 8) |'M',
            ATT_AOE = ('A' << 8) | 'A',
            ATT_PROJ = ('A' << 8) | 'P',
            ATT_BEAM = ('A' << 8) | 'B',
            ATT_DIRECT = ('A' << 8) | 'D',
            ATT_CONE = ('A' << 8) | 'C',

            GET_CHUNKS = 'G' << 8,          //returns the sender's Neighborhood terrain chunks
            GET_CHUNK = ('G' << 8) | 'C',   //returns the specified realative chunk
            GET_VISMASK = ('G' << 8) | 'V', //

            INSP_TILE = ('I'<<8) | 'T',     //return info about the Tile X, Y relative to player including contents
            INSP_ENT = ('I' << 8) | 'E',    //return info about entity #ID on tile X, Y 

            TALK = ('T' << 8),              
            TALK_ENT = ('T' << 8) | 'E',

            USE = ('U' << 8),               //Use top item on a Tile
            USE_ENT = ('U' << 8) | 'E',     //Use entity on a tile by index


        }

        

        public static void ParseIn(byte[] input) {

            int request = ((int)input[1]<<8) | input[2];  // create a word to represent the two bytes used for the command

            

            int[] data = new int[15];   //creates an array of numbers which consists of the 4byte data segments in the
            for (int i = 0, j = 3; i < data.Length; i++) {
                data[i] = Conversions.BytesToSInt(new byte[] { input[j++], input[j++], input[j++], input[j++] });
            }
            
            ////XXXX NOTE: DECREASED EFFICIENCY(?) ABOVE BUT LESS CODE THAN USING XXXX
            ////XXXX THE BELOW, AND EXPLICIT CONVERSIONS WITHIN EACH REQ'S CASE   XXXX

            //byte[][] data = new byte[15][];
            /*
            for( int i = 0; i < data[0].Length; i++) {      // disect the data section of the packet into 15 4-byte numbers
                for (int j = 3; j < data[i].Length;){       // Starting at input[3], which is the beginning of our request's data segment,
                    data[i] = new byte[]{input[j++], input[j++], input[j++], input[j++]};   // Assign each value in the second dim of the array to  
                }
            }
            */


            switch (request) {
                case (int)Req.MOVE:
                    Move( data[0], data[1] );
                    break;
                case (int)Req.MOVE_JUMP: break;
                case (int)Req.MOVE_RUN: break;

                case (int)Req.SET_LOCATION: break;

                case (int)Req.ATT_MELEE: break;
                case (int)Req.ATT_AOE: break;
                case (int)Req.ATT_PROJ: break;
                case (int)Req.ATT_BEAM: break;
                case (int)Req.ATT_CONE: break;
                case (int)Req.ATT_DIRECT: break;

                

                case (int)Req.TALK: break;
                case (int)Req.USE: break;
                
                default:
                    Console.WriteLine("ERROR: INVALID WORLD REQUEST {0}{1}", input[1], input[2]);
                    break;

            }


        }

        public static void Move(int inX, int inY) {
            x += inX;   //NOTE: TO BE EXPANDED - test case of the player's x and y coords
            y += inY;   //     
        }

    }
}
