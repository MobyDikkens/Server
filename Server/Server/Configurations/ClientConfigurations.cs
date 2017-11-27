using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;


namespace Server.Configurations
{
    class ClientConfigurations:EntityTypeConfiguration<ClientModel.Client>
    {
        public ClientConfigurations()
        {
            Property(d => d.Login).IsRequired().HasMaxLength(20);
            Property(d => d.Password).IsRequired();
            Property(d => d.LastUpdate).IsRequired();
        }
    }
}
