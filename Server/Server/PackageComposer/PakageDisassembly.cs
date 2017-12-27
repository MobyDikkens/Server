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

                string DML = Encoding.UTF8.GetString(dml);

                //Check if we have a DML protocol if not throw the exception
                if (DML != "DML")
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

            //Console.WriteLine();

            //Console.WriteLine("Split the package:");

           /* for(int i=0;i<pack.Length;i++)
            {
                //Console.WriteLine("{0:x}",Encoding.UTF8.GetString(pack[i]));
            }*/
            

            

            //Console.WriteLine("*****************************************************************************");

            //If package structure is inccorect
            if (!flags && pack == null)
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
                string s = Encoding.UTF8.GetString(array);

                //the result of caling the function
                byte[][] result = default(byte[][]);

                //Separator 
                byte[] separator = Encoding.UTF8.GetBytes("\r\n");


                List<int> positions = new List<int>();
                int count = 0;

                for (int i = 0; i < array.Length; i++)
                {

                    if (count == 5)
                    {
                        //check if it is a directory
                        if (i != array.Length - 4)
                        {
                            positions.Add(array.Length - 1);// - \r\n\r\n  -  1
                        }
                        break;
                    }

                    if (array[i] == separator[0])
                    {
                        if (i < array.Length - 1 && array[i + 1] == separator[1])
                        {
                            positions.Add(i);
                            count++;

                            if(count == 4)
                            {
                                if(i == array.Length - 4)
                                {
                                    break;
                                }
                            }

                        }
                    }
                }


                result = new byte[positions.Count][];

                int currPosition = 0;

                int offset = 0;

                for(int i=0;i<positions.Count;i++)
                {
                    //current position
                    currPosition = positions[i];
                    //initialize each array
                    result[i] = new byte[currPosition - offset];

                    for(int j = 0;j<result[i].Length;j++)
                    {
                        result[i][j] = array[j + offset];
                    }

                    //add \r\n
                    offset = currPosition + 2;

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
