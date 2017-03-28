using System.Data.Entity;

namespace Diff_Service.Data
{
    public interface IDifferContext
    {
        DbSet<Differ> Differs { get; set; }
        int SaveChanges();
    }
}