using System;
using WebSocketSharp;
using WebSocketSharp.Server;
namespace Server.WSHandlers
{
    class MessageControlles:WebSocketBehavior
    {


        protected override void OnMessage(MessageEventArgs e)
        {
            byte[] request = e.RawData;
            
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

                this.Send(responce);
                

            }
            catch (PackageComposer.UnknownPakageException)//if unkn pckg
            {
                try
                {
                    byte[] responce = PackageProcessor.ResponceProcessor.UnknownPakage();

                    this.Send(responce);
                }
                catch
                { }

            }
            catch//other
            {
                try
                {

                    byte[] responce = PackageProcessor.ResponceProcessor.BadRequest();

                    this.Send(responce);
                }
                catch
                { }
            }
        }
    }
}
