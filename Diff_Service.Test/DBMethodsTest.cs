using Diff_Service.Data;
using NUnit.Framework;
using System.Linq;

namespace Diff_Service.Test
{
    [TestFixture]
    public class DBMethodsTest
    {
        [Test]
        public void GetDiffer_Test()
        {
            DifferContext dbContext = new DifferContext();
            // Clear table so each test has the same starting point.
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");

            dbContext.Differs.Add(new Differ() { LeftInput = "left", RightInput = "right" });
            dbContext.SaveChanges();
            Differ result = DBMethods.GetDiffer(dbContext, 1);

            Assert.That(result, Has.Property("Id").EqualTo(1) 
                & Has.Property("LeftInput").EqualTo("left")
                & Has.Property("RightInput").EqualTo("right"));

            // Non-existing Differ
            result = DBMethods.GetDiffer(dbContext, 2);
            Assert.That(result, Is.EqualTo(null));

            // Incomplete Differ
            dbContext.Differs.Add(new Differ() { LeftInput = null, RightInput = "right" });
            dbContext.SaveChanges();
            result = DBMethods.GetDiffer(dbContext, 2);
            Assert.That(result, Is.EqualTo(null));
        }


        [Test]
        public void AddOrUpdate_Test()
        {
            DifferContext dbContext = new DifferContext();
            // Clear table so each test has the same starting point.
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");

            // Record with only Right data filled
            DBMethods.AddOrUpdate(dbContext, 1, null, "right!");
            Differ result = dbContext.Differs.FirstOrDefault(d => d.Id == 1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo(null) & Has.Property("RightInput").EqualTo("right!"));

            // Record with Left and Right data filled
            DBMethods.AddOrUpdate(dbContext, 1, "left!");
            result = dbContext.Differs.FirstOrDefault(d => d.Id == 1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo("left!") & Has.Property("RightInput").EqualTo("right!"));

            // Record with Left and Right data filled
            DBMethods.AddOrUpdate(dbContext, 2, "left!");
            result = dbContext.Differs.FirstOrDefault(d => d.Id == 2);

            Assert.That(result, Has.Property("Id").EqualTo(2)
                & Has.Property("LeftInput").EqualTo("left!") & Has.Property("RightInput").EqualTo(null));

        }
    }
}

