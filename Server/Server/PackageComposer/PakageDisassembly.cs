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
        private byte[] package;

        private byte[][] unpack;



        public PakageDisassembly(byte[] package)
        {
            if (package != default(byte[]) && package.Length > 3)
            {
                //need to check whether it is a DML package
                byte[] dml = new byte[3];
                Buffer.BlockCopy(package, 0, dml, 0, 3);

                //Check if we have a DML protocol if not throw the exception
                if (dml != Encoding.UTF8.GetBytes("DML"))
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
            bool flags = true;

            byte[][] pack = SpliteRN(package);

            Console.WriteLine();

            Console.WriteLine("Split the package:");

            for(int i=0;i<pack.Length;i++)
            {
                Console.WriteLine(Encoding.UTF8.GetString(pack[i]));
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


        private byte[][] SpliteRN(byte[] array)
        {
            try
            {
                //the result of caling the function
                byte[][] result = default(byte[][]);


                //the \r\n separator
                byte[] separator = Encoding.UTF8.GetBytes("\r\n");


                //list that contains the positions of each \r in the sequence
                List<int> positions = new List<int>();

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == separator[0])
                    {
                        if (i < array.Length - 1 && array[i + 1] == separator[1])
                        {
                            positions.Add(i);
                        }
                    }

                }

                //Initialize the result
                result = new byte[positions.Count - 1][];

                byte[] tmp = default(byte[]);

                int offset = 0;

                //sub?byte the sequence
                for (int i = 0; i < result.Length; i++)
                {
                    tmp = new byte[positions[i] - offset];

                    for (int j = 0; j < tmp.Length; j++)
                    {
                        tmp[j] = array[j + offset];

                    }

                    offset = positions[i] + 2;
                    result[i] = new byte[tmp.Length];
                    result[i] = tmp;

                }


                return result;

            }
            catch
            {
                return default(byte[][]);
            }
        }

        //To get an unpack package array
        public byte[][] Unpack()
        {
            return unpack;
        }

    }
}
