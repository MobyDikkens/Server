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

            try
            {
                PackageComposer.PackageAssembly assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UnknownPackage);
                byte[] responce = assembly.Assemble();

                return responce;

            }
            catch
            {
                return null;
            }

        }


        public static byte[] BadRequest()
        {
            

            try
            {
                PackageComposer.PackageAssembly assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);
                byte[] responce = assembly.Assemble();

                return responce;

            }
            catch
            {
                return null;
            }

        }


        public byte[] IsAlive(bool flags)
        {
            byte[] responce = default(byte[]);

            //trying to send responce
            try
            {

                if (flags)
                {
                    PackageComposer.PackageAssembly assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserExists);
                    responce = assembly.Assemble();
                }
                else
                {

                    PackageComposer.PackageAssembly assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserNotFound);
                    responce = assembly.Assemble();
                }


                return responce;

            }
            catch//if there is an exception
            {
                //trying to send badrequest responce
                return null;

            }

        }



        public byte[] Register(bool flags)
        {

            byte[] responce = default(byte[]);

            try
            {

                if(!flags)//if already registered
                {
                    PackageComposer.PackageAssembly assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.RegistrationOk);
                    responce = assembly.Assemble();
                }
                else
                {
                    PackageComposer.PackageAssembly assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserIsAlreadyExists);
                    responce = assembly.Assemble();
                }

                //Send a responce


                return responce;
            }
            catch (Exception)
            {
               return null;
            }

        }



        public byte[] GetLastUpdate(string message = default(string))
        {

            PackageComposer.PackageAssembly assembly;

            byte[] responce = default(byte[]);

            if (message == default(string))
            {
                assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.UserNotFound);

                responce = assembly.Assemble();
            }

            try
            {

                //Send a responce

                Console.WriteLine();

                Console.WriteLine(message);

                Console.WriteLine();

                

                responce = Encoding.ASCII.GetBytes(message.Replace(@"\","/"));

                return responce;
 

            }
            catch (Exception)
            {
                return null;
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
                        assembly.AddFile(file);
                        message = assembly.Assemble();

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


    }
}
