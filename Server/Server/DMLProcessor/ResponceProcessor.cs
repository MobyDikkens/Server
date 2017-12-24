using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;



namespace Server.PackageProcessor
{
    class ResponceProcessor
    {

        public ResponceProcessor()
        {
            
        }

        public static byte[] UnknownPakage()
        {

            try
            {

                byte[] responce = Encoding.ASCII.GetBytes("DML\r\nUnknownPackage\r\n\r\n");

                return responce;

            }
            catch
            {
                return null;
            }

        }


        public static byte[] BadRequest()
        {
            

            try
            {
                byte[] responce = Encoding.ASCII.GetBytes("DML\r\nBadRequest\r\n\r\n");

                return responce;

            }
            catch
            {
                return null;
            }

        }


        public byte[] IsAlive(bool flags)
        {

            //trying to send responce
            try
            {

                if (flags)
                {

                    byte[] responce = Encoding.ASCII.GetBytes("DML\r\nUserExists\r\n\r\n");

                   return responce;
                }
                else
                {

                    byte[] responce = Encoding.ASCII.GetBytes("DML\r\nUnknownUser\r\n\r\n");

                    return responce;
                }
            }
            catch//if there is an exception
            {
                //trying to send badrequest responce
                return null;

            }

        }



        public byte[] Register(bool flags)
        {

            try
            {
                string message = default(string);

                if(!flags)//if already registered
                {
                    message = "DML\r\nRegistrationOk\r\n\r\n";
                }
                else
                {
                    message = "DML\r\nUserIsAlreadyExists\r\n\r\n";
                }

                //Send a responce

                byte[] responce = Encoding.ASCII.GetBytes(message);

                return responce;
            }
            catch (Exception)
            {
               return null;
            }

        }



        public byte[] GetLastUpdate(string message = default(string))
        {

        

            if (message == default(string))
            {
                message = "DML\r\nUserNotFond\r\n\r\n";
            }

            try
            {

                //Send a responce

                Console.WriteLine();

                Console.WriteLine(message);

                Console.WriteLine();

                byte[] responce = Encoding.ASCII.GetBytes(message);

                return responce;
 

            }
            catch (Exception)
            {
                return null;
            }

        }


    }
}
