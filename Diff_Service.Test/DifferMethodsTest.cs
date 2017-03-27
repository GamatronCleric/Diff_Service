using NUnit.Framework;
using System.Collections.Generic;

namespace Diff_Service.Test
{
    [TestFixture]
    public class DifferMethodsTest
    {
        [TestCase("AAAAAA==", "AAAAAA==", DiffResultType.Equals)]
        [TestCase("AAAAAA==", "AAA=", DiffResultType.SizeDoesNotMatch)]
        [TestCase("AAAAAA==", "AQABAQ==", DiffResultType.ContentDoesNotMatch)]
        public void AreInputsEqual_Test(string left, string right, DiffResultType resultType)
        {
            var result = DifferMethods.AreInputsEqual(left, right);
            Assert.That(result, Is.EqualTo(resultType));
        }

        [Test]
        public void ReportDiffs_Test()
        {
            List<Diff> expected = new List<Diff>()
            {
                new Diff() { offset = 0, length = 1},
                new Diff() { offset = 2, length = 2},
            };

            var result = DifferMethods.ReportDiffs("AAAAAA==","AQABAQ==");
            Assert.That(result, Is.EqualTo(expected));
        }     
    }
}
