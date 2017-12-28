using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{

    class FileWatcher
    {
        private List<FileSystemWatcher> fswList;
        private List<IPAddress> ipList;


        public FileWatcher()
        {
            fswList = new List<FileSystemWatcher>();
            ipList = new List<IPAddress>();
        }

        public void AddClient(IPAddress ip,string path)
        {

            bool flags = false;

            foreach(var tmp in ipList)
            {
                if(tmp == ip)
                {
                    flags = true;
                    break;
                }
            }

            if (!flags)
            {
                FileSystemWatcher fsw = new FileSystemWatcher(path);
                fswList.Add(fsw);
                ipList.Add(ip);
            }
        }


        public void Start()
        {
            foreach (var tmp in fswList)
            {
                tmp.Changed += this.Handler;
                tmp.Renamed += this.Handler;
                tmp.Deleted += this.Handler;
                tmp.Created += this.Handler;

                tmp.EnableRaisingEvents = true;
                tmp.IncludeSubdirectories = true;
            }
        }

        public void End()
        {
            foreach(var tmp in fswList)
            {
                tmp.EnableRaisingEvents = false;
            }
        }

        protected void Handler(object sender,FileSystemEventArgs e)
        {
            UdpClient client = default(UdpClient);

            try
            {

                byte[] buffer = BitConverter.GetBytes(1);

                foreach(var tmp in ipList)
                {
                    client.Send(buffer, buffer.Length, new IPEndPoint(tmp, 8045));
                }

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
        

    }
}
