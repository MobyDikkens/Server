using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PackageComposer
{
    class PakageDisassembly
    {
        private string package;

        public PakageDisassembly(string package)
        {
            this.package = package;
        }

        public string GetLogin()
        {
            string login = String.Empty;

            login = package.Split(',')[0];

            return login;
        }

        public string GetPassword()
        {
            string password = String.Empty;

            int a = this.GetLogin().Length + 1;
            int b = package.Length - this.GetLogin().Length - 1;

            password = package.Substring(a,b);

            return password;
        }
    }
}
