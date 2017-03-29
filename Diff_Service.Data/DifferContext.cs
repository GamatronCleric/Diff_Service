using Diff_Service.Data.Models;
using System.Data.Entity;

namespace Diff_Service.Data
{
    public class DifferContext : DbContext, IDifferContext
    {
        public DbSet<Differ> Differs { get; set; }
    }
}
