using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Server.WebSocketProcessor
{
    class RequestProcessor
    {
        private const int keyLenth = 24;//????

        private string requestKey = default(string);


        TcpClient client = default(TcpClient);

        public RequestProcessor(TcpClient client, string request)
        {
            try
            {
                this.client = client;

                string separator = "Sec-WebSocket-Key: ";
                requestKey = request.Substring(request.IndexOf(separator) + separator.Length, keyLenth);


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
        public void Handshake()
        {
            ResponceProcessor responce = new ResponceProcessor(client, requestKey);
        }

    }
}
