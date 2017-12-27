using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;

namespace Server.WebSocketProcessor
{
    class ResponceProcessor
    {
        private TcpClient client = default(TcpClient);

        private string WebResponce = default(string);

        private string requestKey = default(string);


        public ResponceProcessor(TcpClient client, string requestKey)
        {
            try
            {
                this.client = client;
                WebResponce = "HTTP/1.1 101 Switching Protocols\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: ";
                this.requestKey = requestKey;

                Handshake();
            }
            catch
            {
                try
                {
                    client.Close();
                }
                catch
                { }
            }
        }

        //Make a processing of a WebSockets requests
        private void Handshake()
        {
            NetworkStream stream = default(NetworkStream);



            try
            {
                //Calculate a responce key
                //responceKey += "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

                string key = string.Concat(requestKey, "258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(key));
                    key = Convert.ToBase64String(hash);
                }

                // byte[] hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(responceKey));
                // WebResponce += Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(hash.ToString()));

                WebResponce += key;

                WebResponce += "\r\nSec-WebSocket-Protocol: chat\r\n\r\n";
                


                //Send a responce
                stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(WebResponce);
                stream.Write(buffer, 0, buffer.Length);
                
                while(true)
                {
                    if(stream.DataAvailable)
                    {
                        stream.Read(buffer, 0, buffer.Length);
                        {
                            //Console.WriteLine(Encoding.UTF8.GetString(buffer));
                        }
                    }
                }

                stream.Close();
                client.Close();

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


    }
}
