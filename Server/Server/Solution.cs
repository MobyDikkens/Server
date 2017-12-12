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

        private const int keyLenth = 24;


        /*HTTP/1.1 101 Switching Protocols
        Upgrade: websocket
        Connection: Upgrade
        Sec-WebSocket-Accept: s3pPLMBiTxaQ9kYGzzhZRbK+xOo=
        Sec-WebSocket-Protocol: chat*/


        string WebResponce = "HTTP/1.1 101 Switching Protocols\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: ";

        //Realize what what type of protocol is we working with
        public void FindSolution(TcpClient client,string request)
        {
            this.client = client;

            string clientKey = default(string);

            try
            {
                if (request.IndexOf("HTTP") > -1)
                {
                    if (request.IndexOf("Sec - WebSocket - Key: ") > -1)
                    {
                        string separator = "Sec - WebSocket - Key: ";

                        clientKey = request.Substring(request.IndexOf(separator) + separator.Length, keyLenth);

                        //Send a responce "HADSHAKE"
                        WebSocketProcessor(clientKey);
                    }
                    else
                    {
                        PackageProcessor.ResponceProcessor.UnknownPakage(client);
                    }


                }
                else
                {
                    DMLProcessor(request);
                }
            }
            catch
            {
                try
                {
                    PackageProcessor.ResponceProcessor.BadRequest(client);
                }
                catch
                { }
            }
        }

        //Make a processing of a WebSockets requests
        private void WebSocketProcessor(string key)
        {
            NetworkStream stream = default(NetworkStream);

            try
            {
                //Calculate a responce key
                key += "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

                var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(key));
                WebResponce += string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
                WebResponce += "\r\nSec-WebSocket-Protocol: chat\r\n\r\n";

                //Send a responce
                stream = client.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(WebResponce);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                try
                {
                    stream.Close();
                    client.Close();
                }
                catch
                { }
            }
        }

        //Make a processing of DML Requests
        private void DMLProcessor(string request)
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
                processor = new PackageProcessor.RequestProcessor(client, unpack);
            }
            catch (PackageComposer.UnknownPakageException)//if unkn pckg
            {
                Console.WriteLine("UnknownPackage");
                PackageProcessor.ResponceProcessor.UnknownPakage(client);
            }
            catch//other
            {
                Console.WriteLine("BadRequest");
                PackageProcessor.ResponceProcessor.BadRequest(client);
            }
        }

    }
}
