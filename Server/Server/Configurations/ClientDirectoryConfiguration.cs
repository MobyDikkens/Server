using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.Data;


namespace Server.Configurations
{
    class ClientDirectoryConfiguration:EntityTypeConfiguration<ClientModel.ClientDirectory>
    {
        public ClientDirectoryConfiguration()
        {
            Property(d => d.WorkingDirectory).IsRequired();
           // DbModelBuilder dbModelBuilder = new DbModelBuilder();
           // dbModelBuilder.Entity<ClientModel.ClientDirectory>().HasKey(p => p.ID);
        }

    }
}
