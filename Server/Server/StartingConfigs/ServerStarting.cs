using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    interface ServerStarting
    {
        void StartServer();
        void ServerController(EServer server);
    }
}
