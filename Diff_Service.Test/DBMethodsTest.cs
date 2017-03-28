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
            DifferContext testContext = new DifferContext();
            testContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");
            testContext = new DifferContext();

            DBMethods sut = new DBMethods(testContext);

            testContext.Differs.Add(new Differ() { LeftInput = "left", RightInput = "right" });
            testContext.SaveChanges();
            Differ result = sut.GetDiffer(1);

            // Correct Differ
            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo("left")
                & Has.Property("RightInput").EqualTo("right"));

            // Non-existing Differ
            result = sut.GetDiffer(2);
            Assert.That(result, Is.EqualTo(null));

            // Incomplete Differ
            testContext.Differs.Add(new Differ() { LeftInput = null, RightInput = "right" });
            testContext.SaveChanges();
            result = sut.GetDiffer(2);
            Assert.That(result, Is.EqualTo(null));
        }


        [Test]
        public void AddOrUpdate_Test()
        {
            DifferContext testContext = new DifferContext();
            testContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");
            testContext = new DifferContext();

            DBMethods sut = new DBMethods(testContext);

            // Record with only Right data filled
            sut.AddOrUpdate(1, null, "right!");
            Differ result = testContext.Differs.FirstOrDefault(d => d.Id == 1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo(null) & Has.Property("RightInput").EqualTo("right!"));

            // Record with Left and Right data filled
            sut.AddOrUpdate(1, "left!");
            result = testContext.Differs.FirstOrDefault(d => d.Id == 1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo("left!") & Has.Property("RightInput").EqualTo("right!"));

            // Record with Left and Right data filled
            sut.AddOrUpdate(2, "left!");
            result = testContext.Differs.FirstOrDefault(d => d.Id == 2);

            Assert.That(result, Has.Property("Id").EqualTo(2)
                & Has.Property("LeftInput").EqualTo("left!") & Has.Property("RightInput").EqualTo(null));
        }
    }
}

