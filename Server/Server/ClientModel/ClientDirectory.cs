using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server.ClientModel
{
    class ClientDirectory
    {
        public int Id { get; set; }

        public string WorkingDirectory { get; private set; }

        public ClientDirectory(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;
        }
    }
}
