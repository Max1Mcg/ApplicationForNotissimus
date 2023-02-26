using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
//using System.Configuration;
using Microsoft.Extensions.Configuration;
    
namespace ApplicationForNotissimus
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(string connectionString) : base(connectionString)
        {
        }
        public DbSet<Offer> Offers { get; set; }
    }
}
