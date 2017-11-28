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

            //package = "admin,qwerty12345";

            //if we cannot implement o as TcpClient
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
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        finally
                        {

                            try
                            {
                                client.Close();
                                networkStream.Close();
                            }
                            catch(Exception ex)
                            {

                                Console.WriteLine(ex.ToString());

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

           /* PackageComposer.PakageDisassembly disassembly = new PackageComposer.PakageDisassembly(package);

            ClientModel.Client clientmodel= new ClientModel.Client(disassembly.GetLogin(),disassembly.GetPassword());

            using (var db = new ClientContext())
            {
                db.Clients.Add(clientmodel);
                db.SaveChanges();
            }*/

        }


        //Make it ~ integrated into a NetworkStream
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
