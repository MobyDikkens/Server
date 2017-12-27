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
        
        private byte[][] package;

        private byte[] responce = default(byte[]);

        public RequestProcessor(byte[][] package)
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
            RequestType(Encoding.UTF8.GetString(package[1]));
            

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
                    {
                        Console.WriteLine("IsAlive");
                        IsAlive();
                        break;
                    }

                case "Register":
                    {
                        Console.WriteLine("Register");
                        Register();
                        break;
                    }

                case "GetLastUpdate":
                    {
                        Console.WriteLine("GetLastUpdate");
                        GetLastUpdate();
                        break;
                    }

                case "GetFile":
                    {
                        Console.WriteLine("GetFile");
                        GetFile();
                        break;
                    }

                case "SendFile":
                    {
                        Console.WriteLine("SendFile");
                        SendFile();
                        break;
                    }

                case "SendDirectory":
                    {
                        Console.WriteLine("SendDirectory");
                        SendDirectory();
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Default");
                        PackageComposer.UnknownPakageException ex = new PackageComposer.UnknownPakageException();
                        throw ex;
                    }
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
                        if (tmp.Login == Encoding.UTF8.GetString(package[2]) && tmp.Password == Encoding.UTF8.GetString(package[3]))//if we is already exist
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
                        if(tmp.Login == Encoding.UTF8.GetString(package[2]))//if we is already exist
                        {
                            flags = true;
                            break;
                        }
                    }

                    //if client does not exist resister hom
                    if(!flags)
                    {
                        ClientModel.Client client = new ClientModel.Client(Encoding.UTF8.GetString(package[2]), Encoding.UTF8.GetString(package[3]));

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
                string clietnDir = Encoding.UTF8.GetString(package[2]);//Login = Working DIrectory

                //open a db
                using (var db = new ClientContext())
                {
                    //list of clients in db
                    IQueryable<ClientModel.Client> queryable = db.Clients;
                    

                    //trying to find our new client
                    foreach (var tmp in queryable)
                    {
                        if (tmp.Login == Encoding.UTF8.GetString(package[2]) && tmp.Password == Encoding.UTF8.GetString(package[3]))//if we is already exist
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


        //DB Requests
        private ClientModel.Client DBSearch(string login,string password)
        {
            ClientModel.Client client = default(ClientModel.Client);

            try
            {
                using (var db = new ClientContext())
                {
                    //list of clients in db
                    IQueryable<ClientModel.Client> queryable = db.Clients;

                    string path = default(string);
                    
                    bool flags = false;

                    DateTime dateTime = default(DateTime);

                    //trying to find our new client
                    foreach (var tmp in queryable)
                    {
                        if (tmp.Login == Encoding.UTF8.GetString(package[2]) && tmp.Password == Encoding.UTF8.GetString(package[3]))//if we is already exist
                        {
                            path = tmp.WorkingDirectory;
                            dateTime = tmp.LastUpdate;
                            flags = true;
                            break;
                        }
                    }



                    path += "\\";
                    path += Encoding.UTF8.GetString(package[4]);

                    client = new ClientModel.Client(login, password, path);
                    client.LastUpdate = dateTime;

                    if(flags)
                    {
                        return client;
                    }
                    else
                    {
                        return default(ClientModel.Client);
                    }



                }
            }
            catch
            {
                return default(ClientModel.Client);
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
                        if (tmp.Login == Encoding.UTF8.GetString(package[2]) && tmp.Password == Encoding.UTF8.GetString(package[3]))//if we is already exist
                        {
                            path = tmp.WorkingDirectory;
                            flags = true;
                            break;
                        }
                    }



                    path += "\\";
                    path += Encoding.UTF8.GetString(package[4]);

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
                        if (tmp.Login == Encoding.UTF8.GetString(package[2]) && tmp.Password == Encoding.UTF8.GetString(package[3]))//if we is already exist
                        {
                            path = tmp.WorkingDirectory;
                            tmp.LastUpdate = DateTime.Now;
                            flags = true;
                            break;
                        }
                    }



                    path += "\\";
                    path += Encoding.UTF8.GetString(package[4]);

                    bool success = CloudConfigs.WorkingDirectoryConfig.CreateFile(path,package[5]);

                    //if client exists send all dates to him him
                    this.responce = responce.SendFile(flags,success);


                }
            }
            catch
            {
                this.responce = ResponceProcessor.BadRequest();
            }
        }

        private void SendDirectory()
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
                        if (tmp.Login == Encoding.UTF8.GetString(package[2]) && tmp.Password == Encoding.UTF8.GetString(package[3]))//if we is already exist
                        {
                            path = tmp.WorkingDirectory;
                            tmp.LastUpdate = DateTime.Now;
                            flags = true;
                            break;
                        }
                    }

                    path += "\\";
                    path += Encoding.UTF8.GetString(package[4]);

                    bool success = CloudConfigs.WorkingDirectoryConfig.CreateDirectory(path);

                    //if client exists send all dates to him him
                    this.responce = responce.SendDirectory(flags,success);


                }
            }
            catch
            {
                this.responce = ResponceProcessor.BadRequest();
            }
        }


    }
}
