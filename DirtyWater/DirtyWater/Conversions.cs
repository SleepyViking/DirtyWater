using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWater
{
    class Conversions
    {
        public static int BytesToSInt(byte[] input) {
            return ((((int)(sbyte)input[0]) << 24) | (input[1] << 16) | (input[2] << 8) | input[3]);
        }


        public static uint BytesToUInt(byte[] input) {
            return (((uint)input[0] << 24) | (uint)(input[1] << 16) | (uint)(input[2] << 8) | input[3]);
        }


        public static byte[] UIntToBytes(uint input) {
            byte[] outb = new byte[4];

            outb[0] = (byte)(input & 0xFF000000 >> 24);
            outb[1] = (byte)(input & 0x00FF0000 >> 16);
            outb[2] = (byte)(input & 0x0000FF00 >> 8);
            outb[3] = (byte)(input & 0x000000FF);

            return outb;
                
        }

    }
}
