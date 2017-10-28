using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Server.Models;

namespace Server
{
    //Entity Server - our Server and methods to connect with it
    class EServer
    {
        //our server info - IP and Port
        private static IPEndPoint endPoint;

        //our server (~socket)
        private TcpListener listener;

        //constructor
        public EServer()
        {
            try
            {
                endPoint = new IPEndPoint(IPAddress.Parse(InitiallizeServer.GetIp()), Convert.ToInt32(InitiallizeServer.GetPort()));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //Bind server with IP and Port
        public void Initiallize()
        {
            try
            {
                listener = new TcpListener(endPoint);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //Start our server
        public void Start()
        {
            try
            {
                listener.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Handler()
        {
            while(true)
            {
                //Our client
                TcpClient client = listener.AcceptTcpClient();

                string package = null;

                try
                {
                    while (true)
                    {
                        //stream to work with buffer
                        NetworkStream networkStream = client.GetStream();

                        //processing a package and adding it to the end of string value
                        package += Unpackage(networkStream);
                        
                        //Check if it is the end of the package
                        if (package.IndexOf("\0") > -1)
                        {
                            //Find the end of the package
                            package = package.Substring(0, package.IndexOf("\0"));

                            break;
                        }

                    }

                    Console.WriteLine(package);

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        private string Unpackage(NetworkStream networkStream)
        {
            //amoun of bytes and package buffer
            int rcvBytes = 65535;
            byte[] rcvBuffer = new byte[rcvBytes];

            try
            {
                networkStream.Read(rcvBuffer, 0, rcvBytes);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Encoding.ASCII.GetString(rcvBuffer, 0, rcvBytes);
        }
    }
}
