using NUnit.Framework;
using JesseRussell.Numerics;

namespace JesseRussell.Numerics_test
{
    public class Fraction_test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MinAndMaxValue()
        {
            if (Fraction.Min(new Fraction(9, 3), new Fraction(3, 1)).Numerator != 9) Assert.Fail("Either Fraction.Min simplified the parameters or returned b by default.");
            if (Fraction.Max(new Fraction(9, 3), new Fraction(3, 1)).Numerator != 9) Assert.Fail("Either Fraction.Max simplified the parameters or returned b by default.");

            if (Fraction.Min(new Fraction(1, 2), new Fraction(1, 3)) != new Fraction(1, 3)) Assert.Fail("Fractin.Min did not return the smallest parameter.");
            if (Fraction.Max(new Fraction(1, 2), new Fraction(1, 3)) != new Fraction(1, 2)) Assert.Fail("Fractin.Max did not return the largest parameter.");
        }

        [Test]
        public void Simplify()
        {
            Fraction f, fs;

            f = new Fraction(4, 2);
            fs = f.Simplify();

            if (fs.Numerator != 2 || fs.Denominator != 1) Assert.Fail();


            f = new Fraction(2, 4);
            fs = f.Simplify();

            if (fs.Numerator != 1 || fs.Denominator != 2) Assert.Fail();

            f = new Fraction(4, -2);
            fs = f.Simplify();

            if (fs.Numerator != -2 || fs.Denominator != 1) Assert.Fail();


            f = new Fraction(2, -4);
            fs = f.Simplify();

            if (fs.Numerator != -1 || fs.Denominator != 2) Assert.Fail();
        }
    }
}