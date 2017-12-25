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
    class RequestProcessor
    {
        public enum DMLRequests { IsAlive = 0, Register, GetLastUpdate, GetFile, SendFile }
        
        private string[] package;

        private byte[] responce = default(byte[]);

        public RequestProcessor(string[] package)
        {
            try
            {

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


        public byte[] GetResponce()
        {
            try
            {
                return this.responce;
            }
            catch
            {
                return null;
            }
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

            bool flags = false;

            ResponceProcessor responce = new ResponceProcessor();


            //trying to send responce
            try
            {

                using (var db = new ClientContext())
                {
                    //list of clients in db
                    IQueryable<ClientModel.Client> queryable = db.Clients;

                    //trying to find our new client
                    foreach (var tmp in queryable)
                    {
                        if (tmp.Login == package[2] && tmp.Password == package[3])//if we is already exist
                        {
                            flags = true;
                            break;
                        }
                    }

                    
                }


                this.responce = responce.IsAlive(flags);

            }
            catch//if there is an exception
            {

                this.responce = ResponceProcessor.BadRequest();

            }

        }

      

        //Processing Register responce
        private void Register()
        {
            ResponceProcessor responce = new ResponceProcessor();

            try
            {

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
                            flags = true;
                            break;
                        }
                    }

                    //if client does not exist resister hom
                    if(!flags)
                    {
                        ClientModel.Client client = new ClientModel.Client(package[2], package[3]);

                        db.Clients.Add(client);


                        CloudConfigs.WorkingDirectoryConfig.CreateDirectory(CloudConfigs.WorkingDirectoryConfig.GetWorkingDirectory(), client.Login);


                        db.SaveChanges();
                    }


                    //Send a responce

                    this.responce = responce.Register(flags);
                }

            }


                
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.responce = ResponceProcessor.BadRequest();
            }

        }

        private void GetLastUpdate()
        {

            ResponceProcessor responce = new ResponceProcessor();

            try
            {

                //if user was logged
                bool flags = false;

                //user last update
                DateTime dateTime = default(DateTime);
                string path = default(string);
                string clietnDir = package[2];//Login = Working DIrectory

                //open a db
                using (var db = new ClientContext())
                {
                    //list of clients in db
                    IQueryable<ClientModel.Client> queryable = db.Clients;
                    

                    //trying to find our new client
                    foreach (var tmp in queryable)
                    {
                        if (tmp.Login == package[2] && tmp.Password == package[3])//if we is already exist
                        {
                            path = tmp.WorkingDirectory;
                            dateTime = tmp.LastUpdate;
                            flags = true;
                            break;
                        }
                    }

                    

                }


                this.responce = responce.GetLastUpdate(flags,dateTime,path,clietnDir);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.responce = ResponceProcessor.BadRequest();
            }

        }

        private void GetFile()
        {
            ResponceProcessor responce = new ResponceProcessor();

            try
            {

                //if user was logged
                bool flags = false;

                string path = default(string);
                

                //open a db
                using (var db = new ClientContext())
                {
                    //list of clients in db
                    IQueryable<ClientModel.Client> queryable = db.Clients;

                   

                    //trying to find our new client
                    foreach (var tmp in queryable)
                    {
                        if (tmp.Login == package[2] && tmp.Password == package[3])//if we is already exist
                        {
                            path = tmp.WorkingDirectory;
                            flags = true;
                            break;
                        }
                    }

                    path += "\\";
                    path += package[4];

                    //if client exists send all dates to him him
                    this.responce = responce.GetFile(path, flags);


                }
            }
            catch
            {
                this.responce = ResponceProcessor.BadRequest();
            }
        }    

        private void SendFile()
        {

        }

    }
}
