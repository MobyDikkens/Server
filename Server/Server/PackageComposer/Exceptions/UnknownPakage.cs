using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PackageComposer
{
    class UnknownPakageException:Exception
    {
        private static string message = "Unknown Pakage";

        public UnknownPakageException() : base() { }

        public override string ToString()
        {
            return message;
        }
    }
}
