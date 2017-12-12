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
                endPoint = new IPEndPoint(IPAddress.Parse(ServerConfig.GetIp()), Convert.ToInt32(ServerConfig.GetPort()));
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
                Console.WriteLine("Server have been sucesfully started\nAdress:{0}:{1}", endPoint.Address,endPoint.Port);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //Different ways to bind a server
        public void Initiallize(IPEndPoint endPoint)
        {
            try
            {
                listener = new TcpListener(endPoint);
                Console.WriteLine("Server have been sucesfully started\nAdress:{0}", endPoint.ToString());
            }
            catch (Exception ex)
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
                Console.WriteLine("Starting to Listen:");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //Run server
            Run();
        }

        private void Run()
        {
            while(true)
            {

                try
                {
                    //Our client
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine();
                    Console.WriteLine("Client have been connected:{0}", client.Client.RemoteEndPoint);
                    Thread thread = null;

                    thread = new Thread(ClientHandler);


                    thread.Start(client);

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        //To procces a client in a personal thread
        private static void ClientHandler(object o)
        {
            TcpClient client = o as TcpClient;
            string package = String.Empty;

            //Error falgs
            bool Eflags = false;

            //package = "admin,qwerty12345";

            //if we cannot implement o as TcpClient
            if (o == null)
            {
                return;
            }
            else
            {
                try
                {
                    package = Unpackage(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Eflags = true;
                }
                

                Console.WriteLine();
                Console.WriteLine();
               // Console.WriteLine("*****************************************************************************");
                Console.WriteLine("Recieved data:\n{0}", package);
                //Console.WriteLine("Count of connected clients:{0}", this.Lenth);
                Console.WriteLine(); 
                Console.WriteLine();
                Console.WriteLine();

            }

            if (!Eflags)
            {
                //Entity to make a desicion what type of request is we have
                Processor processor = new Solution();

                //Process it
                processor.FindSolution(client, package);

            }
            else
            {
                try
                {
                    client.Close();
                }
                catch
                { }
            }

        }


        //Make it ~ integrated into a NetworkStream
        private static string Unpackage(TcpClient client)
        {
            //amoun of bytes and package buffer

            int rcvBytes = 65535;
            //Buffer
            byte[] rcvBuffer = new byte[rcvBytes];
            
            //Client stream
            NetworkStream networkStream = null;
            try
            {
                networkStream = client.GetStream();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            //Result
            string package = String.Empty;

            while (true)
            {

                try
                {
                    networkStream.Read(rcvBuffer, 0, rcvBytes);
                    package += Encoding.ASCII.GetString(rcvBuffer, 0, rcvBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }


                //processing a package and adding it to the end of string value

                try
                {
                    //Check if it is the end of the package
                    if (!networkStream.DataAvailable)
                    {
                        //Find the end of the package
                        if (package.Contains("\0"))
                        {
                            package = package.Substring(0, package.IndexOf("\0"));
                        }
                        break;
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        networkStream.Close();
                        throw ex;
                    }
                    catch
                    { }
                }
                
            }
            return package;
        }
    }
}
