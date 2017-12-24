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
                        assembly += "Ok\r\n\r\n";
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
                default:
                    {
                        assembly += "BadRequest\r\n\r\n";
                        break;
                    }
            }
        }

        public byte[] Assemble()
        {
            return Encoding.ASCII.GetBytes(this.assembly);
        }



    }
}
