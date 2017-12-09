using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PackageComposer
{
    class PakageDisassembly
    {

        //DML package commands
        public enum DMLCommands { Autorization = 0, GetLastUpdate, GetFile, SendFile }

        //Package
        private string package;

        private string[] unpack;

        public PakageDisassembly(string package)
        {
            if (package != String.Empty)
            {
                //Check if we have a DML protocol if not throw the exception
                if (package.Substring(0, 3) != "DML")
                {
                    UnknownPakageException exception = new UnknownPakageException();

                    throw exception;
                }
                else
                {
                    this.package = package;
                }
            }
            else
            {
                UnknownPakageException exception = new UnknownPakageException();

                throw exception;

            }

            //To check if it is a \r in the end of string[]
            bool flags = false;

            string[] pack = package.Split('\n');

            Console.WriteLine();

            Console.WriteLine("Split the package:");

            for (int i = 0; i < pack.Length; i++)
            {
                //Check if it is the end + write out pack in unpack array with corrections
                if (pack[i] == "\r")
                {
                    int lenth = i;

                    unpack = new string[lenth];

                    for (int j = 0; j < lenth; j++)
                    {
                        unpack[j] = pack[j];
                    }

                    flags = true;

                    break;

                }

                //remove /r
                try
                {
                    pack[i] = pack[i].Remove(pack[i].IndexOf('\r'));
                }
                catch (IndexOutOfRangeException)
                {
                    UnknownPakageException ex = new UnknownPakageException();

                    throw ex;

                }

                Console.Write("{0}:",i+1);
                Console.WriteLine(pack[i]);
                
            }


            Console.WriteLine("*****************************************************************************");

            //If package structure is inccorect
            if (!flags)
            {
                UnknownPakageException ex = new UnknownPakageException();

                throw ex;
            }
            else
            {
                //Inittialize a package array field
                unpack = pack;
            }



        }

        //To get an unpack package array
        public string[] Unpack()
        {
            return unpack;
        }

    }
}
