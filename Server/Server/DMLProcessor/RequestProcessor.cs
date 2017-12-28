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

        private static ClientModel.Client client = default(ClientModel.Client);


        public ClientModel.Client GetClient()
        {
            return RequestProcessor.client;
        }

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


            bool flags = true;

            ResponceProcessor responce = new ResponceProcessor();


            //trying to send responce
            try
            {

                ClientModel.Client client = DBSearch(Encoding.UTF8.GetString(package[2]), Encoding.UTF8.GetString(package[3]));

                
                if(client == default(ClientModel.Client))
                {
                    flags = false;
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
                bool flags = true;

                ClientModel.Client client = DBSearch(Encoding.UTF8.GetString(package[2]), Encoding.UTF8.GetString(package[3]));


                if (client == default(ClientModel.Client))
                {
                    flags = false;
                    this.responce = ResponceProcessor.BadRequest();
                }
                else
                {

                    this.responce = responce.GetLastUpdate(flags, client.LastUpdate, client.WorkingDirectory, client.Login);//clientDir == Login
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.responce = ResponceProcessor.BadRequest();
            }

        }


        //DB Requests
        private static ClientModel.Client DBSearch(string login,string password)
        {
            ClientModel.Client client = default(ClientModel.Client);

            try
            {
                using (var db = new ClientContext())
                {
                    //list of clients in db
                    IQueryable<ClientModel.Client> queryable = from cl in db.Clients
                                                               where cl.Login == login
                                                               where cl.Password == password
                                                               select cl;

                    string path = default(string);
                    
                    bool flags = false;

                    DateTime dateTime = default(DateTime);

                    //trying to find our new client
                    foreach (var tmp in queryable)
                    {
                        if (tmp.Login == login && tmp.Password == password )//if we is already exist
                        {
                            path = tmp.WorkingDirectory;
                            dateTime = tmp.LastUpdate;
                            flags = true;
                            break;
                        }
                    }


                    client = new ClientModel.Client(login, password, path);
                    client.LastUpdate = dateTime;

                    if(flags)
                    {
                        RequestProcessor.client = client;
                        return client;
                    }
                    else
                    {
                        RequestProcessor.client = client;
                        return default(ClientModel.Client);
                    }



                }
            }
            catch
            {
                RequestProcessor.client = client;
                return default(ClientModel.Client);
            }

        }

        private void GetFile()
        {
            ResponceProcessor responce = new ResponceProcessor();

            try
            {

                //if user was logged
                bool flags = true;

                string path = default(string);



                ClientModel.Client client = DBSearch(Encoding.UTF8.GetString(package[2]), Encoding.UTF8.GetString(package[3]));


                if (client == default(ClientModel.Client))
                {
                    flags = false;
                    this.responce = ResponceProcessor.BadRequest();
                }
                else
                {

                    path = client.WorkingDirectory;
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
                bool flags = true;

                string path = default(string);


                ClientModel.Client client = DBSearch(Encoding.UTF8.GetString(package[2]), Encoding.UTF8.GetString(package[3]));


                if (client == default(ClientModel.Client))
                {
                    flags = false;
                    this.responce = ResponceProcessor.BadRequest();
                }
                else
                {

                    //get requested path + datetime
                    string clientPath = Encoding.UTF8.GetString(package[4]);
                    //delete the date
                    string correctedPath = clientPath.Substring(0, clientPath.IndexOf('\n'));

                    //index and lenth of data
                    int index = clientPath.IndexOf('\n');
                    int lenth = clientPath.Length - 1;

                    string date = clientPath.Substring(index + 1 , lenth - index);

                    DateTime dateTime = DateTime.Parse(date);

                    client.LastUpdate = dateTime;

                    path = client.WorkingDirectory;
                    path += "\\";
                    path += correctedPath;

                    bool success = CloudConfigs.WorkingDirectoryConfig.CreateFile(path, package[5]);

                    //if client exists send all dates to him him
                    this.responce = responce.SendFile(flags, success);
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
                bool flags = true;

                string path = default(string);


                ClientModel.Client client = DBSearch(Encoding.UTF8.GetString(package[2]), Encoding.UTF8.GetString(package[3]));




                if (client == default(ClientModel.Client))
                {
                    flags = false;
                    this.responce = ResponceProcessor.BadRequest();
                }
                else
                {

                    

                    //add path to cloud
                    path += client.WorkingDirectory;
                    //add login
                    path += client.Login;
                    path += "\\";
                    path += Encoding.UTF8.GetString(package[4]);

                    bool success = CloudConfigs.WorkingDirectoryConfig.CreateDirectory(path);

                    //if client exists send all dates to him him
                    this.responce = responce.SendDirectory(flags, success);

                }



            }
            catch
            {
                this.responce = ResponceProcessor.BadRequest();
            }
        }


    }
}
