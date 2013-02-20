using System.Data.Entity;

namespace gu_s.Models
{
    public class GusContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
    }
}