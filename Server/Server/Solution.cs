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
        public void FindSolution(TcpClient client,string request)
        {
            this.client = client;

            try
            {
                if (request.IndexOf("HTTP") > -1)
                {
                    if (request.IndexOf("Sec-WebSocket-Key: ") > -1)
                    {

                        //Send a responce "HADSHAKE"
                        WebSocketHandler(request);
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
                NetworkStream networkStream = client.GetStream();

                networkStream.Write(responce, 0, responce.Length);

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
        private void DMLHandler(string request)
        {
            //to process our requests
            PackageProcessor.RequestProcessor processor = default(PackageProcessor.RequestProcessor);
            //networkStream.Close();
            try
            {
                PackageComposer.PakageDisassembly disassembly = new PackageComposer.PakageDisassembly(request);

                //unpack array of DML request
                string[] unpack = disassembly.Unpack();

                //initialize processor
                processor = new PackageProcessor.RequestProcessor(unpack);

                byte[] responce = processor.GetResponce();


                Responce(responce);

            }
            catch (PackageComposer.UnknownPakageException)//if unkn pckg
            {
                Console.WriteLine("UnknownPackage");
                PackageProcessor.ResponceProcessor.UnknownPakage();
                byte[] responce = processor.GetResponce();
            }
            catch//other
            {
                Console.WriteLine("BadRequest");
                PackageProcessor.ResponceProcessor.BadRequest();
                byte[] responce = processor.GetResponce();
            }
        }

    }
}
