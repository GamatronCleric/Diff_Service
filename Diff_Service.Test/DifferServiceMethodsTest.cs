﻿using Diff_Service.Data;
using NUnit.Framework;
using System.Net;
using System.ServiceModel.Web;

namespace Diff_Service.Test
{
    public class DifferServiceMethodsTest
    {
        [Test]
        public void AddInput_Test()
        {
            DifferContext dbContext = new DifferContext();
            // Clear table so each test has the same starting point.
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");

            // Faulty Id
            var exception = Assert.Catch(() => DifferServiceMethods.AddInput(null, new InputData()
            { Data = "Q2FyZHNPZkhvdXNl" }, true, dbContext));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException<string>>(exception);

            // Correct Id, No Data.
            exception = Assert.Catch(() => DifferServiceMethods.AddInput("1", new InputData()
            { }, true, dbContext));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException<string>>(exception);

            // Correct Info.
            var result = DifferServiceMethods.AddInput("1", new InputData() { Data = "Q2FyZHNPZkhvdXNl" }, true, dbContext);
            Assert.That(result, Is.EqualTo(HttpStatusCode.Created));
        }


        [Test]
        public void CheckInput_Test_Exceptions()
        {
            DifferContext dbContext = new DifferContext();
            // Clear table so each test has the same starting point.
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");

            // Faulty Id
            var exception = Assert.Catch(() => DifferServiceMethods.CheckInput("A", dbContext));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException>(exception);

            // No Differ found.
            exception = Assert.Catch(() => DifferServiceMethods.CheckInput("1", dbContext));
            Assert.NotNull(exception);
            Assert.IsInstanceOf<WebFaultException>(exception);
        }

        [Test]
        public void CheckInput_Test()
        {
            DifferContext dbContext = new DifferContext();
            // Clear table so each test has the same starting point.
            dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [Differs]");

            //Correct Info (Equals).
            dbContext.Differs.Add(new Differ() { Id = 1, LeftInput = "AAAAAA==", RightInput = "AAAAAA==" });
            dbContext.SaveChanges();
            var result = DifferServiceMethods.CheckInput("1", dbContext);
            Assert.That(result, Has.Property("ResultType").EqualTo("Equals")& Has.Property("Diffs").EqualTo(null));

            //Correct Info (Size does not match).
            dbContext.Differs.Add(new Differ() { Id = 2, LeftInput = "AAAAAA==", RightInput = "AAA=" });
            dbContext.SaveChanges();
            result = DifferServiceMethods.CheckInput("2", dbContext);
            Assert.That(result, Has.Property("ResultType").EqualTo("SizeDoesNotMatch") & Has.Property("Diffs").EqualTo(null));

            //Correct Info (Size does not match).
            dbContext.Differs.Add(new Differ() { Id = 3, LeftInput = "AAAAAA==", RightInput = "AQABAQ==" });
            dbContext.SaveChanges();
            result = DifferServiceMethods.CheckInput("3", dbContext);
            Assert.That(result, Has.Property("ResultType").EqualTo("ContentDoesNotMatch") 
                & Has.Property("Diffs").EqualTo(new System.Collections.Generic.List<Diff>()
            {
                new Diff() { offset = 0, length = 1},
                new Diff() { offset = 2, length = 2},
            }));
        }
    }
}