using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PackageComposer
{
    class PackageAssembly
    {
        //enum of all supported commands

        string assembly = "DML\r\n";

        Enums.DMLResponce type;

        public PackageAssembly(Enums.DMLResponce responce)
        {
            type = responce;

            switch(responce)
            {
                case Enums.DMLResponce.BadRequest:
                    {
                        assembly += "BadRequest\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.FileNotFound:
                    {
                        assembly += "FileNotFound\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.Ok:
                    {
                        assembly += "Ok\r\n";
                        break;
                    }
                case Enums.DMLResponce.RegistrationOk:
                    {
                        assembly += "RegistrationOk\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.UnknownPackage:
                    {
                        assembly += "UnknownPackage\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.UnknownUser:
                    {
                        assembly += "UnknownUser\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.UserExists:
                    {
                        assembly += "UserExists\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.UserIsAlreadyExists:
                    {
                        assembly += "UserIsAlreadyExists\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.UserNotFound:
                    {
                        assembly += "UserNotFound\r\n\r\n";
                        break;
                    }
                case Enums.DMLResponce.LastUpdates:
                    {
                        assembly += "LastUpdates\r\n";
                        break;
                    }
                default:
                    {
                        assembly += "BadRequest\r\n\r\n";
                        break;
                    }
            }
        }

        public byte[] Assemble()
        {
            return Encoding.UTF8.GetBytes(this.assembly);
        }

        public override string ToString()
        {
            return assembly;
        }

        public byte[] AddFile(byte[] file)
        {

            //DML\r\nOk\r\n
            byte[] first = Encoding.UTF8.GetBytes(assembly);

            //\r\n\r\n
            byte[] end = Encoding.UTF8.GetBytes("\r\n\r\n");

            //our responce
            byte[] result = new byte[file.Length + first.Length + end.Length];

            Buffer.BlockCopy(first, 0, result, 0, first.Length);
            Buffer.BlockCopy(file, 0 , result, first.Length,file.Length);
            Buffer.BlockCopy(end, 0, result, first.Length+file.Length, end.Length);



            /*//add the first part to the result
            for(int i=0;i<first.Length;i++)
            {
                result[i] = first[i];
            }

            //offset
            int ptr = first.Length;

            //add file part using offset
            for (int i = 0;i<file.Length;i++)
            {
                result[i + ptr] = file[i];
            }

            //inc the offset
            ptr += file.Length;

            //add \r\n\r\n
            for(int i=0;i<end.Length;i++)
            {
                result[i + ptr] = end[i];
            }
            */
            assembly = Encoding.UTF8.GetString(result);

            return result;

        }

        public void AddUpdates(DateTime dateTime, string path, string clientDirectory)
        {

            assembly += Convert.ToString(dateTime);

            //assembly += "\r\n";

            string[][] allUpdates = CloudConfigs.WorkingDirectoryConfig.GetLastUpdate(path, clientDirectory);


            for (int i = 0; i < allUpdates.Length; i++)
            {
                for (int j = 0; j < allUpdates[i].Length; j++)
                {
                    assembly += "\r\n" + allUpdates[i][j];
                }
            }

            assembly += "\r\n\r\n";


            assembly.Replace('\\','/');
        }

    }
}
