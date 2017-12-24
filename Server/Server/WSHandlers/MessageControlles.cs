using System;
using WebSocketSharp;
using WebSocketSharp.Server;
namespace Server.WSHandlers
{
    class MessageControlles:WebSocketBehavior
    {


        protected override void OnMessage(MessageEventArgs e)
        {
            string request = e.Data;
            
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

                this.Send(responce);
                

            }
            catch (PackageComposer.UnknownPakageException)//if unkn pckg
            {
                Console.WriteLine("UnknownPackage");
                PackageProcessor.ResponceProcessor.UnknownPakage();
                byte[] responce = processor.GetResponce();

                this.Send(responce);

            }
            catch//other
            {
                Console.WriteLine("BadRequest");
                PackageProcessor.ResponceProcessor.BadRequest();
                byte[] responce = processor.GetResponce();

                this.Send(responce);
            }
        }
    }
}
