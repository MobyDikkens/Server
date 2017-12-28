using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    //A high level Entity to provide communication with different types of protocols
    interface Processor
    {
        void FindSolution(TcpClient client,byte[] request);
    }
}
