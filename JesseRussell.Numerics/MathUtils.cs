using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace JesseRussell.Numerics
{
    public static class MathUtils
    {
        /// <summary>
        /// Tries to convert the double to a decimal (result) in a controlled manor.
        /// </summary>
        /// <param name="d">The double to be converted.</param>
        /// <param name="result">The result of the conversion.</param>
        /// <returns>If the conversion was successful.</returns>
        public static bool TryToDecimal(double d, out decimal result)
        {
            // Simplest and quickest check first for efficiency...
            if (d > DecimalMaxValue_double || d < DecimalMinValue_double)
            {
                result = default;
                return false;
            }

            // Then actually try the conversion...
            try
            {
                result = Convert.ToDecimal(d);
                return true;
            }
            catch (OverflowException e)
            {
                result = default;
                return false;
            }
        }
        /// <summary>
        /// Tries to convert the double to a decimal (result) in a controlled manor. In this case, the end result is converted back to a double and checked against the original. If they do not match, the conversion fails. This conversion guaranties that the original and the result are considered equal.
        /// </summary>
        /// <param name="d">The double to be converted.</param>
        /// <param name="result">The result of the conversion.</param>
        /// <returns>If the conversion was successful.</returns>
        public static bool TryToDecimalStrictly(double d, out decimal result)
        {
            // Simplest and quickest check first for efficiency...
            if (d > DecimalMaxValue_double || d < DecimalMinValue_double)
            {
                result = default;
                return false;
            }

            // Then actually try the conversion...
            try
            {
                result = Convert.ToDecimal(d);
            }
            catch (OverflowException e)
            {
                result = default;
                return false;
            }

            // This is the part that makes this strict. The end result must be equal to the original double value.
            // If the conversion was successful, make sure that it still equals the original...
            if (Convert.ToDouble(result) == d)
            {
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        public static bool TryToDecimal(BigInteger bi, out decimal result)
        {
            if (DecimalMinValue_BigInteger <= bi && bi <= DecimalMaxValue_BigInteger)
            {
                try
                {
                    result = (decimal)bi;
                    return true;
                }
                finally { }
            }

            result = default;
            return false;
        }


        public static bool NextBool(this Random rand) => rand.NextDouble() >= 0.5;

        private const decimal DecimalEpsilon = 1e-28M; // *from https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
        private const double DecimalMaxValue_double = 7.922816251426433E+28; // *from Console.WriteLine(Convert.ToDouble(decimal.MaxValue)); *actual result was 7.922816251426434E+28 (4 vs 3 at end), but that 4 at the end was rounded up, so I floored it back down to 3. This is the actual largest double, which can be cast to decimal.
        private const double DecimalMinValue_double = -7.922816251426433E+28;
        private static readonly BigInteger DecimalMaxValue_BigInteger = (BigInteger)decimal.MaxValue;
        private static readonly BigInteger DecimalMinValue_BigInteger = (BigInteger)decimal.MinValue;
    }
}
