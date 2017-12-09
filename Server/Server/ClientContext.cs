using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Server
{
    class ClientContext:DbContext
    {
        public DbSet<ClientModel.Client> Clients { get; set; }



       /* public IQueryable<ClientModel.Client> GetClients()
        {
            using (var context = new ClientContext())
            {

                return context.Set<ClientModel.Client>();
            }
        }*/

    }
}
