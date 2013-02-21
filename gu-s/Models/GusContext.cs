using System.Data.Entity;

namespace gu_s.Models
{
    public class GusContext : DbContext
    {
        public GusContext() : base("GusDatabase"){ }

        public DbSet<Country> Countries { get; set; }
    }
}