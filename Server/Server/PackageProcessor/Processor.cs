using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server.PackageProcessor
{
    class Processor
    {
        public enum DMLRequests { IsAlive = 0, Register, GetLastUpdate, GetFile, SendFile }

        private TcpClient client;

        private string[] package;

        public Processor(TcpClient client,string[] package)
        {
            try
            {
                this.client = client;
                this.package = package;
            }
            catch(Exception)
            {
                PackageComposer.UnknownPakageException ex = new PackageComposer.UnknownPakageException();

                throw ex;
            }

            //Call request type processing
            RequestType(package[1]);
            

        }

        private void RequestType(string request)
        {
            switch(request)
            {
                case "IsAlive":
                    IsAlive();
                    break;

                case "Register":
                    Register();
                    break;

                case "GetLastUpdate":
                    GetLastUpdate();
                    break;

                case "GetFile":
                    GetFile();
                    break;

                case "SendFile":
                    SendFile();
                    break;

                default:
                    PackageComposer.UnknownPakageException ex = new PackageComposer.UnknownPakageException();
                    throw ex;
                    //break;
            }
        }

        //Is Alive Request
        private void IsAlive()
        {
            NetworkStream networkStream = default(NetworkStream);

            //trying to send responce
            try
            {
                networkStream = client.GetStream();

                byte[] responce = Encoding.ASCII.GetBytes("DML\r\nAlive\r\n\r\n");

                networkStream.Write(responce, 0, responce.Length);

                networkStream.Close();
                client.Close();
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
                    networkStream.Close();
                }

            }

        }

        //send badrequest responce
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
                networkStream.Close();
                client.Close();
            }

        }

        public static void UnknownPakage(TcpClient client)
        {
            NetworkStream networkStream = default(NetworkStream);

            try
            {
                byte[] responce = Encoding.ASCII.GetBytes("DML\r\nUnknownPackage\r\n\r\n");

                networkStream = client.GetStream();

                networkStream.Write(responce, 0, responce.Length);

                networkStream.Close();
                client.Close();

            }
            catch
            {
                networkStream.Close();
                client.Close();
            }

        }

        //Processing Register responce
        private void Register()
        {
            //stream to deal with client
            NetworkStream networkStream = default(NetworkStream);

            try
            {
                string message = "DML\r\nRegistrationOk\r\n\r\n";

                bool flags = false;

                //open a db
                using (var db = new ClientContext())
                {
                    //list of clients in db
                    IQueryable<ClientModel.Client> queryable = db.Clients;

                    //trying to find our new client
                    foreach (var tmp in queryable)
                    {
                        if(tmp.Login == package[2])//if we is already exist
                        {
                            message = "DML\r\nUserIsAlreadyLogined\r\n\r\n";
                            flags = true;
                            break;
                        }
                    }

                    //if client does not exist resister hom
                    if(!flags)
                    {
                        ClientModel.Client client = new ClientModel.Client(package[2], package[3]);

                        db.Clients.Add(client);
                        db.SaveChanges();
                    }

                }
                

                //Send a responce

               byte[] responce = Encoding.ASCII.GetBytes(message);

                networkStream = client.GetStream();

                networkStream.Write(responce, 0, responce.Length);

                networkStream.Close();
                client.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                BadRequest(client);
            }

        }

        private void GetLastUpdate()
        {
           

        }

        private void GetFile()
        {

        }

        private void SendFile()
        {

        }

    }
}
