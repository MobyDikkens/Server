using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server.Models
{
    static class ServerConfig
    {
        public static string GetIp()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\MobyDi\\Desktop\\Server\\Server\\Server\\Resources\\ip.ini"))
                {
                    String ip = sr.ReadToEnd();
                    return ip;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public static string GetPort()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\MobyDi\\Desktop\\Server\\Server\\Server\\Resources\\port.ini"))
                {
                    String port = sr.ReadToEnd();
                    return port;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}
