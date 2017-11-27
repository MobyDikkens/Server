using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ClientModel
{
    class Client
    {
        private int ID;

        //To store a client working directory
        public ClientDirectory WorkingDirectory { get; private set; }

        //To save a client last update
        public DateTime LastUpdate { get; set; }

        public  string Login { get; private set; }

        public string Password { get; private set; }

        //Add a non-required parametr
        public Client(string login,string password,ClientDirectory workingDirectory = null)
        {
            Login = login;
            Password = password;
            WorkingDirectory = workingDirectory;
        }
    }
}
