using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using JesseRussell.Numerics;

namespace JesseRussell.Numerics_test
{
    class IntFloatFrac_test
    {
        [Test]
        public void Parsing()
        {
            if (IntFloatFrac.Parse("1/2") != new IntFloatFrac(new Fraction(1, 2))) Assert.Fail("fraction parse");
            if (IntFloatFrac.Parse("-1/2") != new IntFloatFrac(new Fraction(-1, 2))) Assert.Fail("fraction parse");
            if (IntFloatFrac.Parse(new Fraction(1,2).ToString()) != new IntFloatFrac(new Fraction(1, 2))) Assert.Fail("fraction parse");
            if (IntFloatFrac.Parse("1.3") != new IntFloatFrac(new IntFloat(new Doudec(1.3)))) Assert.Fail("fraction parse");
            if (IntFloatFrac.Parse(new IntFloat(new Doudec(1.3)).ToString()) != new IntFloatFrac(new IntFloat(new Doudec(1.3)))) Assert.Fail("fraction parse");
            
        }
    }
}
