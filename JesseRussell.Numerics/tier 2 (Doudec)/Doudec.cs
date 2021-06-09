using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Linq;

namespace JesseRussell.Numerics
{
    /// <summary>
    /// Combination of decimal and double. This is a method for reducing the impact of floating point rounding errors, with a large trade-off in performance (unfortunately); It generally tries to store the number as a decimal for as long as possible, only switching to double on an overflow of either size or precision.
    /// </summary>
    /// 
    /// <author>
    /// Jesse Russell
    /// </author>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct Doudec : IComparable<Doudec>, IComparable<double>, IComparable<decimal>, IEquatable<Doudec>, IEquatable<double>, IEquatable<decimal>
    {
        /// <summary>
        /// If the wrapped value is a double.
        /// </summary>
        [FieldOffset(0)]
        public readonly bool IsDouble;
        [FieldOffset(1)]
        private readonly double floatingPoint;
        [FieldOffset(1)]
        private readonly decimal fixedPoint;

        /// <summary>
        /// If the wrapped value is a decimal.
        /// </summary>
        public bool IsDecimal => !IsDouble;

        /// <summary>
        /// The Doudec as a Double. Is a direct conversion if IsDouble is true.
        /// </summary>
        public double Double => IsDouble ? floatingPoint : Convert.ToDouble(fixedPoint);

        /// <summary>
        /// The Doudec as a decimal. Is a direct conversion if IsDecimal is true.
        /// </summary>
        public decimal Decimal => IsDouble ? Convert.ToDecimal(floatingPoint) : fixedPoint;

        /// <summary>
        /// The wrapped value as an object.
        /// </summary>
        public object Value => IsDouble ? (object)floatingPoint : fixedPoint;

        /// <summary>
        /// If the value of the Doudec is a whole number.
        /// </summary>
        public bool IsWhole => IsDouble ? floatingPoint % 1.0 == 0.0 : fixedPoint % 1.0M == 0.0M;

        // o---------------o
        // | Constructors: |
        // o---------------o
        public Doudec(double d)
        {
            if (MathUtils.TryToDecimalStrictly(d, out decimal dec))
            {
                IsDouble = false;
                floatingPoint = 0;
                fixedPoint = dec;
            }
            else
            {
                IsDouble = true;
                fixedPoint = default;
                floatingPoint = d;
            }
        }
        public Doudec(decimal dec)
        {
            IsDouble = false;
            floatingPoint = default;
            fixedPoint = dec;
        }

        public Doudec(sbyte i) : this((decimal)i) { }
        public Doudec(short i) : this((decimal)i) { }
        public Doudec(int i) : this((decimal)i) { }
        public Doudec(long i) : this((decimal)i) { }

        public Doudec(byte i) : this((decimal)i) { }
        public Doudec(ushort i) : this((decimal)i) { }
        public Doudec(uint i) : this((decimal)i) { }
        public Doudec(ulong i) : this((decimal)i) { }

        // o------------o
        // | Operations |
        // o------------o
        /// <summary>
        /// Returns the calling Doudec plus 1.0
        /// </summary>
        public Doudec Increment() => IsDouble ? (Doudec)floatingPoint + 1.0 : fixedPoint + 1M;
        /// <summary>
        /// Returns the calling Doudec minus 1.0
        /// </summary>
        public Doudec Decrement() => IsDouble ? (Doudec)floatingPoint - 1.0 : fixedPoint - 1M;
        #region public Methods
        #region Math
        #endregion

        #region Comparison
        /// <summary>
        /// Returns a comparison of the calling Doudec and the given Doudec.
        /// </summary>
        /// <returns>
        /// (-1): calling &lt; given
        /// (0): calling == given
        /// (1): calling &gt; given
        /// </returns>
        public int CompareTo(Doudec other)
        {
            if (IsDouble || other.IsDouble) return Double.CompareTo(other.Double);
            else return fixedPoint.CompareTo(other.fixedPoint);
        }

        /// <summary>
        /// Returns a comparison of the calling Doudec and the given double.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>
        /// (-1): calling &lt; given
        /// (0): calling == given
        /// (1): calling &gt; given
        /// </returns>
        public int CompareTo(double d) => Double.CompareTo(d);

        /// <summary>
        /// Returns a comparison of the calling Doudec and the given decimal.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>
        /// (-1): calling &lt; given
        /// (0): calling == given
        /// (1): calling &gt; given
        /// </returns>
        public int CompareTo(decimal dec) => Decimal.CompareTo(dec);

        /// <summary>
        /// Returns whether the calling Doudec is equal to the given Doudec.
        /// </summary>
        public bool Equals(Doudec other)
        {
            if (IsDecimal)
            {
                if (other.TryToDecimal(out decimal other_dec))
                {
                    return fixedPoint.Equals(other_dec);
                }
                else
                {
                    return Double.Equals(other.Double);
                }
            }
            else if (!other.IsDouble)
            {
                if (TryToDecimal(out decimal this_dec))
                {
                    return this_dec.Equals(other.fixedPoint);
                }
                else
                {
                    return Double.Equals(other.Double);
                }
            }
            else
            {
                return floatingPoint.Equals(other.floatingPoint);
            }
        }

        /// <summary>
        /// Returns whether the calling Doudec is equal to the given double.
        /// </summary>
        public bool Equals(double d) => Equals((Doudec)d);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given decimal.
        /// </summary>
        public bool Equals(decimal dec) => Equals((Doudec)dec);

        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(BigInteger i)
        {
            if (!IsWhole) return false;

            if (IsDouble)
            {
                if (double_minValue_BigInteger <= i && i <= double_maxValue_BigInteger)
                {
                    return (BigInteger)floatingPoint == i;
                }
                else return false;
            }
            else
            {
                if (decimal_minValue_BigInteger <= i && i <= decimal_maxValue_BigInteger)
                {
                    return (BigInteger)fixedPoint == i;
                }
                else return false;
            }
        }
        private static readonly BigInteger double_maxValue_BigInteger = (BigInteger)Math.Truncate(double.MaxValue);
        private static readonly BigInteger double_minValue_BigInteger = (BigInteger)Math.Truncate(double.MinValue);

        private static readonly BigInteger decimal_maxValue_BigInteger = (BigInteger)Math.Truncate(decimal.MaxValue);
        private static readonly BigInteger decimal_minValue_BigInteger = (BigInteger)Math.Truncate(decimal.MinValue);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(byte i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(ushort i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(uint i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(ulong i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(UBigInteger i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(sbyte i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(short i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(int i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given integer.
        /// </summary>
        public bool Equals(long i) => Equals((BigInteger)i);
        /// <summary>
        /// Returns whether the calling Doudec is equal to the given Object. 
        /// </summary>
        public override bool Equals(object obj) => obj switch
        {
            Doudec dd => Equals(dd),
            double d => Equals(d),
            float f => Equals(f),
            decimal dec => Equals(dec),

            BigInteger i => Equals(i),

            long i => Equals(i),
            int i => Equals(i),
            short i => Equals(i),
            sbyte i => Equals(i),

            UBigInteger i => Equals(i),

            ulong i => Equals(i),
            uint i => Equals(i),
            ushort i => Equals(i),
            byte i => Equals(i),
            _ => throw new ArgumentException("Argument must be a Doudec, double, float, decimal, BigInteger, long, int, short, sbyte, UBigInteger, ulong, uint, ushort, or byte"),
        };

        /// <summary>
        /// Returns a HashCode representing the Doudec.
        /// </summary>
        public override int GetHashCode() => IsDouble ? floatingPoint.GetHashCode() : fixedPoint.GetHashCode();

        #endregion

        /// <summary>
        /// Returns a string encoding of the Doudec.
        /// </summary>
        public override string ToString() => IsDouble ? floatingPoint.ToString() : fixedPoint.ToString();
        
        /// <summary>
        /// Tries to convert to a decimal.
        /// </summary>
        /// <param name="result">The resulting decimal if the conversion was successful</param>
        /// <returns>Whether the conversion was successful or not.</returns>
        public bool TryToDecimal(out decimal result)
        {
            if (IsDecimal)
            {
                result = fixedPoint;
                return true;
            }
            else
            {
                return MathUtils.TryToDecimalStrictly(floatingPoint, out result);
            }
        }
        #endregion
        #region public static Methods

        // o----------o
        // | casting: |
        // o----------o
        #region Casts
        #region from
        // int -> Doudec
        public static implicit operator Doudec(sbyte i) => new Doudec((decimal) i);
        public static implicit operator Doudec(short i) => new Doudec((decimal) i);
        public static implicit operator Doudec(int i) => new Doudec((decimal) i);
        public static implicit operator Doudec(long i) => new Doudec((decimal) i);
        public static explicit operator Doudec(BigInteger i) => FromBigInteger(i);

        public static implicit operator Doudec(byte i) => new Doudec((decimal) i);
        public static implicit operator Doudec(ushort i) => new Doudec((decimal) i);
        public static implicit operator Doudec(uint i) => new Doudec((decimal) i);
        public static implicit operator Doudec(ulong i) => new Doudec((decimal) i);
        public static explicit operator Doudec(UBigInteger i) => FromBigInteger(i);

        // floating point -> Doudec
        public static implicit operator Doudec(float f) => new Doudec(f);
        public static implicit operator Doudec(double f) => new Doudec(f);
        public static implicit operator Doudec(decimal dec) => new Doudec(dec);
        #endregion

        #region to
        // Doudec -> int
        public static explicit operator sbyte(Doudec d) => d.IsDouble ? (sbyte)d.floatingPoint : (sbyte)d.fixedPoint;
        public static explicit operator short(Doudec d) => d.IsDouble ? (short)d.floatingPoint : (short)d.fixedPoint;
        public static explicit operator int(Doudec d) => d.IsDouble ? (int)d.floatingPoint : (int)d.fixedPoint;
        public static explicit operator long(Doudec d) => d.IsDouble ? (long)d.floatingPoint : (long)d.fixedPoint;
        public static explicit operator BigInteger(Doudec d) => d.IsDouble ? (BigInteger)d.floatingPoint : (BigInteger)d.fixedPoint;

        public static explicit operator byte(Doudec d) => d.IsDouble ? (byte)d.floatingPoint : (byte)d.fixedPoint;
        public static explicit operator ushort(Doudec d) => d.IsDouble ? (ushort)d.floatingPoint : (ushort)d.fixedPoint;
        public static explicit operator uint(Doudec d) => d.IsDouble ? (uint)d.floatingPoint : (uint)d.fixedPoint;
        public static explicit operator ulong(Doudec d) => d.IsDouble ? (ulong)d.floatingPoint : (ulong)d.fixedPoint;
        public static explicit operator UBigInteger(Doudec d) => d.IsDouble ? (UBigInteger)d.floatingPoint : (UBigInteger)d.fixedPoint;

        // Doudec -> floating point
        public static explicit operator float(Doudec d) => (float)d.Double;
        public static explicit operator double(Doudec d) => (double)d.Double;
        public static explicit operator decimal(Doudec d) => d.Decimal;
        #endregion
        #endregion

        #region double Pass-through
        /// <summary>
        /// If the Doudec is not a number.
        /// </summary>
        public static bool IsNaN(Doudec d) => d.IsDouble && double.IsNaN(d.floatingPoint);
        /// <summary>
        /// If the Doudec is infinity.
        /// </summary>
        public static bool IsInfinity(Doudec d) => d.IsDouble && double.IsInfinity(d.floatingPoint);

        /// <summary>
        /// If the Doudec is finite.
        /// </summary>
        public static bool IsFinite(Doudec d) => d.IsDecimal || double.IsFinite(d.floatingPoint);

        /// <summary>
        /// If the Doudec is Negative.
        /// </summary>
        public static bool IsNegative(Doudec d) => d.IsDouble ? double.IsNegative(d.floatingPoint) : d.fixedPoint < 0.0M;

        /// <summary>
        /// If the Doudec is positive infinity
        /// </summary>
        public static bool IsPositiveInfinity(Doudec d) => d.IsDouble && double.IsPositiveInfinity(d.floatingPoint);

        /// <summary>
        /// If the Doudec is negative infinity
        /// </summary>
        public static bool IsNegativeInfinity(Doudec d) => d.IsDouble && double.IsNegativeInfinity(d.floatingPoint);
        #endregion

        #region Parse
        /// <summary>
        /// Tries to parse the given string to a Doudec.
        /// </summary>
        /// <param name="s">the string to parse</param>
        /// <param name="result">The resulting Doudec if the parse was successful.</param>
        /// <returns>Whether the parse was successful.</returns>
        public static bool TryParse(string s, out Doudec result)
        {
            if (decimal.TryParse(s, out decimal dec))
            {
                result = new Doudec(dec);
                return true;
            }
            else if (double.TryParse(s, out double d))
            {
                result = new Doudec(d);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Parses the given string to a Doudec.
        /// </summary>
        public static Doudec Parse(string s)
        {
            if (TryParse(s, out Doudec result))
            {
                return result;
            }
            else
            {
                throw new FormatException("The string could not be parsed as either a double or a decimal.");
            }
        }
        #endregion

        #region Operators
        public static Doudec operator -(Doudec dd) => Neg(dd);
        public static Doudec operator +(Doudec dd) => dd;

        public static Doudec operator +(Doudec left, Doudec right) => Add(left, right);
        public static Doudec operator -(Doudec left, Doudec right) => Subtract(left, right);
        public static Doudec operator *(Doudec left, Doudec right) => Multiply(left, right);
        public static Doudec operator /(Doudec left, Doudec right) => Divide(left, right);
        public static Doudec operator %(Doudec left, Doudec right) => Remainder(left, right);

        public static Doudec operator ++(Doudec dd) => dd.Increment();
        public static Doudec operator --(Doudec dd) => dd.Decrement();

        public static bool operator ==(Doudec left, Doudec right) => left.Equals(right);
        public static bool operator !=(Doudec left, Doudec right) => !left.Equals(right);

        public static bool operator >(Doudec left, Doudec right) => left.CompareTo(right) > 0;
        public static bool operator >=(Doudec left, Doudec right) => left.CompareTo(right) > 0 || left.Equals(right);
        public static bool operator <(Doudec left, Doudec right) => left.CompareTo(right) < 0;
        public static bool operator <=(Doudec left, Doudec right) => left.CompareTo(right) < 0 || left.Equals(right);
        #endregion

        #region Math
        /// <summary>
        /// Returns the sum of the two given Doudecs.
        /// </summary>
        public static Doudec Add(Doudec left, Doudec right)
        {
            if (left.IsDouble || right.IsDouble)
            {
                return left.Double + right.Double;
            }
            else
            {
                try
                {
                    return left.fixedPoint + right.fixedPoint;
                }
                catch (OverflowException)
                {
                    return left.Double + right.Double;
                }
            }
        }

        /// <summary>
        /// Returns the difference of the two given Doudecs.
        /// </summary>
        public static Doudec Subtract(Doudec left, Doudec right)
        {
            if (left.IsDouble || right.IsDouble)
            {
                return left.Double - right.Double;
            }
            else
            {
                try
                {
                    return left.fixedPoint - right.fixedPoint;
                }
                catch (OverflowException)
                {
                    return left.Double - right.Double;
                }
            }
        }

        /// <summary>
        /// Returns the product of the two given Doudecs.
        /// </summary>
        public static Doudec Multiply(Doudec left, Doudec right)
        {
            if (left.IsDouble || right.IsDouble)
            {
                return left.Double * right.Double;
            }
            else
            {
                try
                {
                    return left.fixedPoint * right.fixedPoint;
                }
                catch (OverflowException)
                {
                    return left.Double * right.Double;
                }
            }
        }

        /// <summary>
        /// Returns the quotient of the two given Doudecs.
        /// </summary>
        public static Doudec Divide(Doudec left, Doudec right)
        {
            if (left.IsDouble || right.IsDouble)
            {
                return left.Double / right.Double;
            }
            else
            {
                try
                {
                    return left.fixedPoint / right.fixedPoint;
                }
                catch (OverflowException)
                {
                    return left.Double / right.Double;
                }
            }
        }

        /// <summary>
        /// Returns the remainder of the two given Doudecs.
        /// </summary>
        public static Doudec Remainder(Doudec left, Doudec right)
        {
            if (left.IsDouble || right.IsDouble)
            {
                return left.Double % right.Double;
            }
            else
            {
                try
                {
                    return left.fixedPoint % right.fixedPoint;
                }
                catch (OverflowException)
                {
                    return left.Double % right.Double;
                }
            }
        }

        /// <summary>
        /// Returns the square root of the given Doudec.
        /// </summary>
        public static Doudec Sqrt(Doudec x) => Math.Sqrt(x.Double);
        /// <summary>
        /// Returns the power of the given Doudec raised to the given double.
        /// </summary>
        public static Doudec Pow(Doudec x, double y) => Math.Pow(x.Double, y);

        /// <summary>
        /// Returns the Given Doudec rounded down to the nearest whole number.
        /// </summary>
        public static Doudec Floor(Doudec dd) => dd.IsDouble ? (Doudec) Math.Floor(dd.Double) : Math.Floor(dd.Decimal);
        /// <summary>
        /// Returns the Given Doudec rounded up to the nearest whole number.
        /// </summary>
        public static Doudec Ceiling(Doudec dd) => dd.IsDouble ? (Doudec) Math.Ceiling(dd.Double) : Math.Ceiling(dd.Decimal);
        /// <summary>
        /// Returns whole number portion of the given Doudec.
        /// </summary>
        public static Doudec Truncate(Doudec dd) => dd.IsDouble ? (Doudec) Math.Truncate(dd.Double) : Math.Truncate(dd.Decimal);

        /// <summary>
        /// Returns the absolute value of the given Doudec.
        /// </summary>
        public static Doudec Abs(Doudec dd) => dd.IsDouble ? (Doudec) Math.Abs(dd.Double) : Math.Abs(dd.Decimal);


        /// <summary>
        /// Returns the natural (base e) logarithm of the given Doudec.
        /// </summary>
        public static Doudec Log(Doudec x) => Math.Log(x.Double);
        /// <summary>
        /// Returns the largarithm of the specified Doudec in the specified base.
        /// </summary>
        public static Doudec Log(Doudec x, Doudec newbase) => Math.Log(x.Double, newbase.Double);
        /// <summary>
        /// Returns the base 10 logarithm of the given Doudec.
        /// </summary>
        public static Doudec Log10(Doudec x) => Math.Log10(x.Double);

        /// <summary>
        /// Returns an integer that represents the sign of hte given Doudec.
        /// </summary>
        public static int Sign(Doudec x) => x.IsDouble ? Math.Sign(x.floatingPoint) : Math.Sign(x.fixedPoint);
        #endregion

        #region Conversion
        /// <summary>
        /// Returns a Doudec converted from the given BigInteger.
        /// </summary>
        public static Doudec FromBigInteger(BigInteger bi)
        {
            if (MathUtils.TryToDecimal(bi, out decimal dec))
            {
                return dec;
            }
            else
            {
                return new Doudec((double)bi);
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Doudec that is not a number.
        /// </summary>
        public static readonly Doudec NaN = new Doudec(double.NaN);
        /// <summary>
        /// Doudec equal to positive infinity.
        /// </summary>
        public static readonly Doudec PositiveInfinity = new Doudec(double.PositiveInfinity);
        /// <summary>
        /// Doudec equal to negative infinity.
        /// </summary>
        public static readonly Doudec NegativeInfinity = new Doudec(double.NegativeInfinity);
        /// <summary>
        /// Doudec equal to the smallest value of double that is greater than 0.
        /// </summary>
        public static readonly Doudec Epsilon = new Doudec(double.Epsilon);
        
    }

    /// <summary>
    /// Extension methods for Doudec.
    /// </summary>
    public static class DoudecExtensions
    {
        /// <summary>
        /// Returns the total value of all the fractions in the calling enumerable.
        /// </summary>
        public static Doudec Sum(this IEnumerable<Doudec> items) => items.Aggregate((total, next) => total + next);
    }
}
