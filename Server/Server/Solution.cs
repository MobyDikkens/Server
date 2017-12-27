using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Security.Cryptography;


namespace Server
{
    //A class that implements Processor functions
    class Solution:Processor
    {
        TcpClient client = default(TcpClient);
        
            

        //Realize what what type of protocol is we working with
        public void FindSolution(TcpClient client,byte[] request)
        {
            this.client = client;

            string strRequest = Encoding.UTF8.GetString(request);

            try
            {
                if (strRequest.IndexOf("HTTP") > -1)
                {
                    if (strRequest.IndexOf("Sec-WebSocket-Key: ") > -1)
                    {

                        //Send a responce "HADSHAKE"
                        WebSocketHandler(strRequest);
                    }
                    else
                    {
                        Responce(PackageProcessor.ResponceProcessor.UnknownPakage());
                    }


                }
                else
                {
                    DMLHandler(request);
                }
            }
            catch
            {
                try
                {
                    Responce(PackageProcessor.ResponceProcessor.BadRequest());
                }
                catch
                { }
            }
        }



        private void Responce(byte[] responce)
        {
            try
            {
                var changedResponce = AddPackageSize(responce);



                MessageVizualizer.Hex(changedResponce);

                NetworkStream networkStream = client.GetStream();

                networkStream.Write(changedResponce, 0, changedResponce.Length);

                try
                {
                    client.Close();
                }
                catch
                { }

           }
            catch
            {
                try
                {
                    client.Close();
                }
                catch
                {

                }
            }
        }


        private byte[] AddPackageSize(byte[] responce)
        {
            try
            {
                //Special for Aleksei add 4 
                byte[] size = BitConverter.GetBytes(responce.Length);

                byte[] result = new byte[size.Length + responce.Length];

                Buffer.BlockCopy(size, 0, result, 0, size.Length);
                Buffer.BlockCopy(responce, 0, result, size.Length, responce.Length);
                /*
                for (int i = 0; i < size.Length; i++)
                {
                    result[i] = size[i];
                }

                int ptr = size.Length;

                for (int i = 0; i < responce.Length; i++)
                {
                    result[i + ptr] = responce[i];
                }*/

                return result;
            }
            catch
            {
                PackageComposer.PackageAssembly assembly = new PackageComposer.PackageAssembly(Enums.DMLResponce.BadRequest);
                return assembly.Assemble();
            }
        }


        //Make a processing of a WebSockets requests
        private void WebSocketHandler(string request)
        {

            try
            {
                WebSocketProcessor.RequestProcessor processor = new WebSocketProcessor.RequestProcessor(client, request);

            }
            catch
            { }
        }

        //Make a processing of DML Requests
        private void DMLHandler(byte[] request)
        {
            //to process our requests
            PackageProcessor.RequestProcessor processor = default(PackageProcessor.RequestProcessor);
            //networkStream.Close();
            try
            {
                PackageComposer.PakageDisassembly disassembly = new PackageComposer.PakageDisassembly(request);

                
                //unpack array of DML request
                byte[][] unpack = disassembly.Unpack();

                

                //initialize processor
                processor = new PackageProcessor.RequestProcessor(unpack);

                byte[] responce = processor.GetResponce();


                Responce(responce);

            }
            catch (PackageComposer.UnknownPakageException)//if unkn pckg
            {
                ////Console.WriteLine("UnknownPackage");
                
                byte[] responce = PackageProcessor.ResponceProcessor.UnknownPakage();
                Responce(responce);
            }
            catch//other
            {
                ////Console.WriteLine("BadRequest");
               
                byte[] responce = PackageProcessor.ResponceProcessor.BadRequest();
                Responce(responce);
            }
        }

    }
}
