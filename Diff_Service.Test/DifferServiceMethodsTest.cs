using Diff_Service.Data;
using Diff_Service.Data.Models;
using Diff_Service.Models;
using NUnit.Framework;
using System.Net;
using System.ServiceModel.Web;

namespace Diff_Service.Test
{
    public class DifferServiceMethodsTest
    {
        private readonly DifferServiceMethods _sut = new DifferServiceMethods();
        DifferContext _testContext = new DifferContext();

        [SetUp]
        public void RunBeforeAnyTests()
        {
            _testContext = new DifferContext();
            // Clear table so each test has the same starting point.
            _testContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");
            _testContext = new DifferContext();
        }


        [Test]
        public void FaultyId_Exception_AddInput()
        {
            var exception = Assert.Catch(() => _sut.AddInput(null, new InputData()
            { Data = "Q2FyZHNPZkhvdXNl" }, true));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException<string>>(exception);
        }

        [Test]
        public void NoData_Exception_AddInput()
        {
            var exception = Assert.Catch(() => _sut.AddInput("1", new InputData(), true));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException<string>>(exception);
        }

        [Test]
        public void CorrectInput_AddInput()
        {
            var result = _sut.AddInput("1", new InputData() { Data = "Q2FyZHNPZkhvdXNl" }, true);
            Assert.That(result, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void FaultyId_Exception_CheckInput()
        {
            var exception = Assert.Catch(() => _sut.CheckInput("A"));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException>(exception);
        }

        [Test]
        public void NoDifferFound_Exception_CheckInput()
        {
            var exception = Assert.Catch(() => _sut.CheckInput("1"));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException>(exception);
        }

        [Test]
        public void EqualsInfo_CheckInput()
        {
            _testContext.Differs.Add(new Differ() { Id = 1, LeftInput = "AAAAAA==", RightInput = "AAAAAA==" });
            _testContext.SaveChanges();
            var result = _sut.CheckInput("1");
            Assert.That(result, Has.Property("ResultType").EqualTo("Equals")& Has.Property("Diffs").EqualTo(null));
        }

        [Test]
        public void SizeDoesNotMatchInfo_CheckInput()
        {
            _testContext.Differs.Add(new Differ() { Id = 1, LeftInput = "AAAAAA==", RightInput = "AAA=" });
            _testContext.SaveChanges();
            var result = _sut.CheckInput("1");
            Assert.That(result, Has.Property("ResultType").EqualTo("SizeDoesNotMatch") & Has.Property("Diffs").EqualTo(null));
        }

        [Test]
        public void ContentDoesNotMatch_CheckInput()
        {
            _testContext.Differs.Add(new Differ() { Id = 1, LeftInput = "AAAAAA==", RightInput = "AQABAQ==" });
            _testContext.SaveChanges();
            var result = _sut.CheckInput("1");
            Assert.That(result, Has.Property("ResultType").EqualTo("ContentDoesNotMatch")
                & Has.Property("Diffs").EqualTo(new System.Collections.Generic.List<Diff>()
            {
                new Diff() { Offset = 0, Length = 1},
                new Diff() { Offset = 2, Length = 2},
            }));
        }
    }
}
