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
        private TcpClient client = default(TcpClient);

        public ResponceProcessor(TcpClient client)
        {
            this.client = client;
        }

        public static void UnknownPakage(TcpClient client)
        {
            NetworkStream networkStream = default(NetworkStream);

            try
            {

                byte[] responce = Encoding.ASCII.GetBytes("DML\r\nUnknownPackage\r\n\r\n");

                networkStream = client.GetStream();

                Console.WriteLine(responce.Length);
                networkStream.Write(responce, 0, responce.Length);

                networkStream.Close();
                client.Close();

            }
            catch
            {
                try
                {
                    networkStream.Close();
                    client.Close();
                }
                catch
                { }
            }

        }


        public static void BadRequest(TcpClient client)
        {
            NetworkStream networkStream = default(NetworkStream);

            try
            {
                byte[] responce = Encoding.ASCII.GetBytes("DML\r\nBadRequest\r\n\r\n");

                networkStream = client.GetStream();

                networkStream.Write(responce, 0, responce.Length);

                networkStream.Close();
                client.Close();

            }
            catch
            {
                try
                {
                    networkStream.Close();
                    client.Close();
                }
                catch
                { }
            }

        }


        public void IsAlive(bool flags)
        {
            NetworkStream networkStream = default(NetworkStream);

            //trying to send responce
            try
            {
                networkStream = client.GetStream();

                if (flags)
                {

                    byte[] responce = Encoding.ASCII.GetBytes("DML\r\nUserExists\r\n\r\n");

                    networkStream.Write(responce, 0, responce.Length);

                    networkStream.Close();
                    client.Close();
                }
                else
                {

                    byte[] responce = Encoding.ASCII.GetBytes("DML\r\nUnknownUser\r\n\r\n");

                    networkStream.Write(responce, 0, responce.Length);

                    networkStream.Close();
                    client.Close();
                }
            }
            catch//if there is an exception
            {
                //trying to send badrequest responce
                try
                {
                    BadRequest(this.client);

                    networkStream.Close();

                }
                catch//close all opened
                {
                    try
                    {
                        networkStream.Close();
                    }
                    catch
                    { }
                }

            }

        }



        public void Register(bool flags)
        {
            //stream to deal with client
            NetworkStream networkStream = default(NetworkStream);

            try
            {
                string message = default(string);

                if(!flags)//if already registered
                {
                    message = "DML\r\nRegistrationOk\r\n\r\n";
                }
                else
                {
                    message = "DML\r\nUserIsAlreadyLoggined";
                }

                //Send a responce

                byte[] responce = Encoding.ASCII.GetBytes(message);

                networkStream = client.GetStream();

                networkStream.Write(responce, 0, responce.Length);

                networkStream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                BadRequest(client);
            }

        }



        public void GetLastUpdate(string message = default(string))
        {

            //stream to deal with client
            NetworkStream networkStream = default(NetworkStream);

            if (message == default(string))
            {
                message = "DML\r\nUserIsNotFond\r\n\r\n";
            }

            try
            {

                //Send a responce

                Console.WriteLine();

                Console.WriteLine(message);

                Console.WriteLine();

                byte[] responce = Encoding.ASCII.GetBytes(message);

                networkStream = client.GetStream();

                networkStream.Write(responce, 0, responce.Length);

                networkStream.Close();
                client.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                BadRequest(client);
            }

        }


    }
}
