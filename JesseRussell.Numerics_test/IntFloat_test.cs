using NUnit.Framework;
using System.Numerics;

using JesseRussell.Numerics;

namespace JesseRussell.Numerics_test
{
    class IntFloat_test
    {
        [Test]
        public void Pow()
        {
            IntFloat x = new BigInteger(2);
            IntFloat y = new BigInteger(600);

            if (
                IntFloat.Pow(x, y) !=
                BigInteger.Parse("41495155688809929585124078636911611510124462322424368999956573296906528114129" +
                "08146399707048947103794288197886611300789182395151075411775307886874834113963687061181803401509523685376")
                ) Assert.Fail();

            x = 2.3;
            y = 333.4;

            IntFloat p = IntFloat.Pow(x, y);
            if (!p.IsFloat)                  Assert.Fail();
            if (p != 3.981626592064194E+120) Assert.Fail();
        }
    }
}
