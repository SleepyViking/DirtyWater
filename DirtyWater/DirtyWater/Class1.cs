using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWater
{
    class Meta
    {
        enum Req : uint
        {

            LOGIN = 'L' << 8,
            LOGOUT = ('L' << 8) | 'O',

        }

        public static void ParseIn(byte[] input){
            uint request = ((uint)input[1] << 8) | input[2];



        }
















    }



}
