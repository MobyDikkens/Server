using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static class MessageVizualizer
    {
        public static void Show(byte[] responce)
        {

           /* //Console.WriteLine("\n\n\n**************************************************");
            //Console.WriteLine("Send a responce:");
            //Console.WriteLine(Encoding.UTF8.GetString(responce));*/

        }

        public static void Show(string responce)
        {

            /*//Console.WriteLine("\n\n\n**************************************************");
            //Console.WriteLine("Send a responce:");
            //Console.WriteLine(responce);*/

        }


        public static void Hex(byte[] responce)
        {
            /*//Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            for (int i = 0; i < responce.Length; i++)
            {
                //Console.Write("{0:X} ", responce[i]);
            }
            //Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");*/
        }
    }
}
