using NUnit.Framework;
using JesseRussell.Numerics;
using System;

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

        [Test]
        public void Floor_Ceiling_Truncate()
        {
            Fraction f;
            f= new Fraction(25, 3);

            if (f.Floor() != 8) Assert.Fail("floor");
            if (f.Ceiling() != 9) Assert.Fail("ceiling");
            if (f.Truncate() != 8) Assert.Fail("truncate");

            f = new Fraction(-25, 3);

            if (f.Floor() != -9) Assert.Fail("floor");
            if (f.Ceiling() != -8) Assert.Fail("ceiling");
            if (f.Truncate() != -8) Assert.Fail("truncate");
        }

        [Test]
        public void Remainder()
        {
            Fraction f1, f2;

            f1 = new Fraction(50, 1);
            f2 = new Fraction(9, 1);

            if (f1 % f2 != 5) Assert.Fail();

            f1 = new Fraction(-50, 1);
            f2 = new Fraction(9, 1);
            if (f1 % f2 != -5) Assert.Fail();

            f1 = new Fraction(50, 1);
            f2 = new Fraction(-9, 1);
            if (f1 % f2 != 5) Assert.Fail();

            f1 = new Fraction(-50, 1);
            f2 = new Fraction(-9, 1);
            if (f1 % f2 != -5) Assert.Fail();
        }

        [Test]
        public void NaiveSum()
        {
            int lim = 10;
            // Big O(n^4). hey, If it's for a unit test, bad code is good code. this only takes 125ms by the way. 
            for (int n1 = -lim; n1 <= lim; ++n1)
                for (int d1 = -lim; d1 <= lim; ++d1)
                    for (int n2 = -lim; n2 <= lim; ++n2)
                        for (int d2 = -lim; d2 <= lim; ++d2)
                            if (new Fraction(n1, d1).NaiveSum(new Fraction(n2, d2)) != new Fraction(n1 + n2, d1 + d2))
                                Assert.Fail($"{n1}/{d1} ~+~ {n2}/{d2}");
        }

        [Test]
        public void EqualizeDenominators()
        {
            int lim = 10;
            for (int n1 = -lim; n1 <= lim; ++n1)
                for (int d1 = -lim; d1 <= lim; ++d1)
                    for (int n2 = -lim; n2 <= lim; ++n2)
                        for (int d2 = -lim; d2 <= lim; ++d2)
                        {
                            if (d1 == 0 || d2 == 0) continue;
                            Fraction f1, f2;
                            (f1, f2) = Fraction.EqualizeDenominators(new Fraction(n1, d1), new Fraction(n2, d2));
                            if (f1 != new Fraction(n1, d1)) Assert.Fail($"{f1} != {n1}/{d1}");
                            if (f2 != new Fraction(n2, d2)) Assert.Fail($"{f2} != {n2}/{d2}");
                            if (f1.AbsoluteValue.Denominator != Math.Abs(d1 * d2)) Assert.Fail($"denominator {f1.AbsoluteValue.Denominator} != {Math.Abs(d1 * d2)}");
                            if (f2.AbsoluteValue.Denominator != Math.Abs(d1 * d2)) Assert.Fail($"denominator {f2.AbsoluteValue.Denominator} != {Math.Abs(d1 * d2)}");
                        }
                            
        }
    }
}