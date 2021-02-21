using NUnit.Framework;
using JesseRussell.Numerics;
using System;
using System.Collections.Generic;

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

        [Test]
        public void Sum()
        {
            // Hard coded values:
            Fraction
                f1 = new Fraction(9, 3),
                f2 = new Fraction(8, -9),
                f3 = new Fraction(4, 7),
                f4 = new Fraction(0, 11),
                f5 = new Fraction(456435, 5652),
                f6 = new Fraction(456435, -2532),
                f7 = new Fraction(42, 42),
                f8 = new Fraction(420, 69),
                f9 = new Fraction(256, 65536),
                f10 = new Fraction(128, 32768),
                f11 = new Fraction(1420, 69),
                f12 = new Fraction(1256, 65536),
                f13 = new Fraction(1128, 32768),
                f14 = new Fraction(1420, -69),
                f15 = new Fraction(1256, -65536),
                f16 = new Fraction(1128, -32768),
                f17 = new Fraction(1, 3),
                f18 = new Fraction(1, 4),
                f19 = new Fraction(1, 5),
                f20 = new Fraction(1, 6),
                f21 = new Fraction(1, 7),
                f22 = new Fraction(1, 8),
                f23 = new Fraction(1, 9);

            if (
            new[] {
            f1,f2,f3,f4,f5,
            f6,f7,f8,f9,f10,
            f11,f12,f13,f14,
            f15,f16,f17,f18,
            f19,f20,f21,f22,
            f23,
            }.Sum() !=
            f1 + f2 + f3 + f4 + f5 +
            f6 + f7 + f8 + f9 + f10 +
            f11 + f12 + f13 + f14 +
            f15 + f16 + f17 + f18 +
            f19 + f20 + f21 + f22 +
            f23
            )
                Assert.Fail("Hard coded values");

            // Random values
            Random rand = new Random();
            List<Fraction> fracs = new List<Fraction>();
            Fraction total = new Fraction(0);

            for(int i = 0; i < 1000; ++i)
            {
                Fraction next = rand.NextFraction(int.MinValue, int.MaxValue, 1, int.MaxValue);
                
                fracs.Add(next);
                total += next;
            }
            if (fracs.Sum() != total) Assert.Fail("Random values");

            // single item:
            if (new[] { new Fraction(1, 2) }.Sum() != new Fraction(1, 2)) Assert.Fail("single item");
            if (new Fraction[0].Sum() != new Fraction(0)) Assert.Fail("no items");

        }
    }
}