using System.Data.Entity;

namespace Diff_Service.Data
{
    public class DifferContext : DbContext
    {
        public DbSet<Differ> Differs { get; set; }
    }
}
