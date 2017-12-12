using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ClientModel
{
    class Client
    {
       public int Id { get; set; }

        //To store a client working directory
        public string WorkingDirectory { get; private set; }

        //To save a client last update
        public DateTime LastUpdate { get; set; }

        public  string Login { get; private set; }

        public string Password { get; private set; }

        //Add a non-required parametr
        public Client(string login,string password,string workingDirectory = default(string))
        {
            Login = login;
            Password = password;
            WorkingDirectory = CloudConfigs.WorkingDirectoryConfig.GetWorkingDirectory() + login;
            
            LastUpdate = DateTime.Now;
        }

        public Client()
        {
            Login = default(string);
            Password = default(string);
            WorkingDirectory = default(string);
            LastUpdate = DateTime.Now;
        }

    }
}
