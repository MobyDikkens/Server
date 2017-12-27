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
            PackageComposer.PackageAssembly assembly = default(PackageComposer.PackageAssembly);

            try
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UnknownPackage);
                byte[] responce = assembly.Assemble();

                return responce;

            }
            catch
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();
            }

        }


        public static byte[] BadRequest()
        {
            PackageComposer.PackageAssembly assembly = default(PackageComposer.PackageAssembly);

            try
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);
                byte[] responce = assembly.Assemble();

                return responce;

            }
            catch
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();
            }

        }


        public byte[] IsAlive(bool flags)
        {
            PackageComposer.PackageAssembly assembly = default(PackageComposer.PackageAssembly);

            byte[] responce = default(byte[]);

            //trying to send responce
            try
            {

                if (flags)
                {
                   assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserExists);
                    responce = assembly.Assemble();
                }
                else
                {

                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserNotFound);
                    responce = assembly.Assemble();
                }


                return responce;

            }
            catch//if there is an exception
            {
                //trying to send badrequest responce
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();

            }

        }



        public byte[] Register(bool flags)
        {
            PackageComposer.PackageAssembly assembly = default(PackageComposer.PackageAssembly);
            byte[] responce = default(byte[]);

            try
            {

                if(!flags)//if already registered
                {
                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.RegistrationOk);
                    responce = assembly.Assemble();
                }
                else
                {
                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserIsAlreadyExists);
                    responce = assembly.Assemble();
                }

                //Send a responce


                return responce;
            }
            catch
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();
            }

        }



        public byte[] GetLastUpdate(bool flags,DateTime dateTime,string path,string clientDirectory)
        {

            PackageComposer.PackageAssembly assembly;

            
            try
            { 



                if (flags)
                {
                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.LastUpdates);
                    assembly.AddUpdates(dateTime, path, clientDirectory);
                }
                else
                {
                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserNotFound);
                }
            
            



                //Send a responce

               // //Console.WriteLine();

                ////Console.WriteLine(assembly);

                ////Console.WriteLine();


                return assembly.Assemble();
 

            }
            catch
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();
            }

        }


        public byte[] GetFile(string path,bool flags)
        {

            byte[] message = default(byte[]);

            PackageComposer.PackageAssembly assembly;

            try
            {


               

                //if client exists send all dates to him him
                if (flags)
                {
                    //out file
                    byte[] file = CloudConfigs.WorkingDirectoryConfig.GetFile(path);

                    if (file != default(byte[]))
                    {
                        assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.Ok);
                        message = assembly.AddFile(file);
                        //message = assembly.Assemble();

                    }

                }
                else
                {
                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.FileNotFound);
                    message = assembly.Assemble();
                }

                return message;


            }
            catch
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();
            }

        }


        public byte[] SendFile(bool flags,bool success)
        {

            byte[] message = default(byte[]);

            PackageComposer.PackageAssembly assembly;

            try
            {
                if (flags)
                {
                    if (success)
                    {
                        assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.Sucess);
                        message = assembly.Assemble();
                    }
                    else
                    {
                        assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);
                        message = assembly.Assemble();
                    }

                }
                else
                {
                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserNotFound);
                    message = assembly.Assemble();
                }


                return message;

            }
            catch
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();
            }
        }

        public byte[] SendDirectory(bool flags,bool success)
        {
            byte[] message = default(byte[]);

            PackageComposer.PackageAssembly assembly;

            try
            {
                if (flags)
                {
                    if (success)
                    {
                        assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.Sucess);
                        message = assembly.Assemble();
                    }
                    else
                    {
                        assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);
                        message = assembly.Assemble();
                    }

                }
                else
                {
                    assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserNotFound);
                    message = assembly.Assemble();
                }


                return message;

            }
            catch
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);

                return assembly.Assemble();
            }
        }


    }
}
