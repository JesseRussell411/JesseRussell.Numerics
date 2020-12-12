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
            IntFloat f1 = new BigInteger(2);
            IntFloat f2 = new BigInteger(600);

            if (
                IntFloat.Pow(f1, f2) !=
                BigInteger.Parse("41495155688809929585124078636911611510124462322424368999956573296906528114129" +
                "08146399707048947103794288197886611300789182395151075411775307886874834113963687061181803401509523685376")
                ) Assert.Fail();
        }
    }
}
