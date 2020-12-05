using NUnit.Framework;
using JesseRussell.Numerics;

namespace JesseRussell.Numerics_test
{
    public class Doudec_test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DefaultValue()
        {
            Doudec d1 = default;
            Assert.AreEqual(d1, 0.0);
            Assert.AreEqual(d1, 0.0M);
            Assert.AreEqual(d1, 0);
            Assert.IsFalse(d1.IsDouble);
            Assert.IsTrue(d1.IsDecimal);
            Assert.AreEqual(d1.Value, 0.0);
            Assert.AreEqual(d1.Double, 0.0);
            Assert.AreEqual(d1.Decimal, 0.0M);
        }

        [Test]
        public void BasicOperations()
        {
            Doudec d1 = .1;
            Doudec d2 = .2;

            Assert.AreEqual(d1 + d2, .3);
            Assert.AreEqual(d1 - d2, -.1);
            Assert.AreEqual(d1 * d2, .02);
            Assert.AreEqual(d1 / d2, .5);
            Assert.AreEqual(d1 % d2, .1);
        }
    }
}