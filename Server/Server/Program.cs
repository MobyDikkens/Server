using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Server.Models;

namespace Server
{
    class Program:ServerStarting
    {

        static void Main(string[] args)
        {
            /*Program server = new Program();

            server.StartServer();*/

            string[][] catalogTree = CloudConfigs.CloudConfig.GetCatalogTree("C:\\Users\\MobyDi\\Desktop\\Пан Горох фото");

            for(int i=0;i<catalogTree.Length;i++)
            {

                Console.WriteLine(catalogTree[i][0]);

                for(int j=1;j<catalogTree[i].Length;j++)
                {
                    Console.WriteLine("                {0}", catalogTree[i][j]);
                }
            }

            Console.ReadKey();
        }


        public void ServerController(EServer server)
        {
            //Bind server
            server.Initiallize();

            //Start server
            server.Start();

        }

        //Initiallize Sockets(.ini files) + go to ServerController
        public void StartServer()
        {
            EServer server = new EServer();

            ServerController(server);
        }
    }
}
