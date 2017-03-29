using Diff_Service.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace Diff_Service.Test
{
    [TestFixture]
    public class DifferMethodsTest
    {
        private readonly DifferMethods _sut = new DifferMethods();

        [TestCase("AAAAAA==", "AAAAAA==", DiffResultType.Equals)]
        [TestCase("AAAAAA==", "AAA=", DiffResultType.SizeDoesNotMatch)]
        [TestCase("AAAAAA==", "AQABAQ==", DiffResultType.ContentDoesNotMatch)]
        public void AreInputsEqual_Test(string left, string right, DiffResultType resultType)
        {
            var result = _sut.AreInputsEqual(left, right);
            Assert.That(result, Is.EqualTo(resultType));
        }

        [Test]
        public void ReportDiffs_Test()
        {
            List<Diff> expected = new List<Diff>()
            {
                new Diff() { Offset = 0, Length = 1},
                new Diff() { Offset = 2, Length = 2},
            };

            var result = _sut.ReportDiffs("AAAAAA==","AQABAQ==");
            Assert.That(result, Is.EqualTo(expected));
        }     
    }
}
