using Diff_Service.Data;
using Diff_Service.Data.Models;
using NUnit.Framework;
using System.Linq;

namespace Diff_Service.Test
{
    [TestFixture]
    public class DbMethodsTest
    {
        private DbMethods _sut;
        private DifferContext _testContext;

        [SetUp]
        public void RunBeforeAnyTests()
        {
            _testContext = new DifferContext();
            // Clear table so each test has the same starting point.
            _testContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");
            _testContext = new DifferContext();
            _sut = new DbMethods(_testContext);
        }

        [Test]
        public void CorrectDiffer_GetDiffer()
        {
            _testContext.Differs.Add(new Differ() { LeftInput = "left", RightInput = "right" });
            _testContext.SaveChanges();
            var result = _sut.GetDiffer(1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo("left")
                & Has.Property("RightInput").EqualTo("right"));
        }

        [Test]
        public void NonExistentDiffer_GetDiffer()
        {
            var result = _sut.GetDiffer(1);
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void IncompleteDiffer_GetDiffer()
        {
            _testContext.Differs.Add(new Differ() { LeftInput = null, RightInput = "right" });
            _testContext.SaveChanges();
            var result = _sut.GetDiffer(1);
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void RightInputOnly_AddOrUpdate()
        {
            _sut.AddOrUpdate(1, null, "right!");
            var result = _testContext.Differs.FirstOrDefault(d => d.Id == 1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo(null) & Has.Property("RightInput").EqualTo("right!"));
        }

        [Test]
        public void LeftInputOnly_AddOrUpdate()
        {
            _sut.AddOrUpdate(1, "left!");
            var result = _testContext.Differs.FirstOrDefault(d => d.Id == 1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo("left!") & Has.Property("RightInput").EqualTo(null));
        }

        [Test]
        public void LeftAndRightInput_AddOrUpdate()
        {
            _sut.AddOrUpdate(1, null, "right!");
            _sut.AddOrUpdate(1, "left!");
            var result = _testContext.Differs.FirstOrDefault(d => d.Id == 1);

            Assert.That(result, Has.Property("Id").EqualTo(1)
                & Has.Property("LeftInput").EqualTo("left!") & Has.Property("RightInput").EqualTo("right!"));
        }
    }
}

