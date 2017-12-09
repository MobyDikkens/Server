using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PackageComposer
{
    class PackageAssembly
    {
        //enum of all supported commands
        public enum DMLRequests { IsAlive = 0 , Register , GetLastUpdate, GetFile, SendFile }

        public enum DMLResponce { IsAlive = 0 , RegisterOk , RegisterFail , SendLastUpdate , SendFile }

        private DMLRequests request;


        public PackageAssembly(DMLRequests request)
        {
            this.request = request;
        }


    }
}
