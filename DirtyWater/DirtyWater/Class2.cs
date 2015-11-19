using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWater
{
    class World
    {
        
        enum Req : uint {

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

            uint request = ((uint)input[1]<<8) | input[2]; // create a word to represent the two bytes used for the command


            switch (request) {
                case (uint)Req.MOVE: break;
                case (uint)Req.MOVE_JUMP: break;
                case (uint)Req.MOVE_RUN: break;

                case (uint)Req.SET_LOCATION: break;

                case (uint)Req.ATT_MELEE: break;
                case (uint)Req.ATT_AOE: break;
                case (uint)Req.ATT_PROJ: break;
                case (uint)Req.ATT_BEAM: break;
                case (uint)Req.ATT_CONE: break;
                case (uint)Req.ATT_DIRECT: break;



                case (uint)Req.TALK: break;
                case (uint)Req.USE: break;
                
                default:
                    Console.WriteLine("ERROR: INVALID REQUEST");
                    break;

            }


        }

    }
}
