using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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

                try
                {
                    //Our client
                    TcpClient client = listener.AcceptTcpClient();
                    Thread thread = null;

                    /*
                     * string package = null;

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
                    */

                    thread = new Thread(ClientHandler);

                    thread.Start(client);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }


        private static void ClientHandler(object o)
        {
            TcpClient client = o as TcpClient;
            string package = "";


            if (o == null)
            {
                return;
            }
            else
            {

                    while (true)
                    {
                        //stream to work with buffer
                        NetworkStream networkStream = null;

                        try
                        {
                            networkStream = client.GetStream();
                            package += Unpackage(networkStream);
                        }
                        catch
                        {
                            Console.WriteLine("NetworkStream ERROR");
                        }
                        finally
                        {

                            try
                            {
                                client.Close();
                                networkStream.Close();
                            }
                            catch
                            {

                                Console.WriteLine("Closing ERROR");

                            }

                        }

                        //processing a package and adding it to the end of string value


                        //Check if it is the end of the package
                        if (package.IndexOf("\0") > -1)
                        {
                            //Find the end of the package
                            package = package.Substring(0, package.IndexOf("\0"));

                            break;
                        }


                    }


                    Console.WriteLine();
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine("Recieved data:{0}", package);
                    //Console.WriteLine("Count of connected clients:{0}", this.Lenth);
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine();

            }
        }



        private static string Unpackage(NetworkStream networkStream)
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
