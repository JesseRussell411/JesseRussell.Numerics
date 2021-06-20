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

        // o-------------o
        // | Properties: |
        // o-------------o

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

        /// <summary>
        /// If the Doudec is not a number.
        /// </summary>
        public bool IsNaN=> IsDouble && double.IsNaN(floatingPoint);
        /// <summary>
        /// If the Doudec is infinity.
        /// </summary>
        public bool IsInfinity => IsDouble && double.IsInfinity(floatingPoint);

        /// <summary>
        /// If the Doudec is finite.
        /// </summary>
        public bool IsFinite => IsDecimal || double.IsFinite(floatingPoint);

        /// <summary>
        /// If the Doudec is Negative.
        /// </summary>
        public bool IsNegative => IsDouble ? double.IsNegative(floatingPoint) : fixedPoint < 0.0M;

        /// <summary>
        /// If the Doudec is positive infinity
        /// </summary>
        public bool IsPositiveInfinity => IsDouble && double.IsPositiveInfinity(floatingPoint);

        /// <summary>
        /// If the Doudec is negative infinity
        /// </summary>
        public bool IsNegativeInfinity => IsDouble && double.IsNegativeInfinity(floatingPoint);



        // o---------------o
        // | Constructors: |
        // o---------------o
        public Doudec(double dbl)
        {
            IsDouble = true;
            fixedPoint = default;
            floatingPoint = dbl;
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
        /// <summary>
        /// Returns the total of the current Doudec and other.
        /// </summary>
        public Doudec Add(Doudec other)
        {
            if (IsDecimal && other.IsDecimal)
            {
                try
                {
                    return fixedPoint + other.fixedPoint;
                }
                catch (OverflowException) { }
            }
            return Double + other.Double;
        }
        /// <summary>
        /// Returns the difference between the current Doudec and the given Doudec.
        /// </summary>
        public Doudec Subract(Doudec other)
        {
            if (IsDecimal && other.IsDecimal)
            {
                try
                {
                    return fixedPoint - other.fixedPoint;
                }
                catch (OverflowException) { }
            }
            return Double - other.Double;
        }
        /// <summary>
        /// Returns the product of the current Doudec and the given Doudec.
        /// </summary>
        public Doudec Multiply(Doudec other)
        {
            if (IsDecimal && other.IsDecimal)
            {
                try
                {
                    return fixedPoint * other.fixedPoint;
                }
                catch (OverflowException) { }
            }
            return Double * other.Double;
        }
        /// <summary>
        /// Returns the total of the current Doudec and the given Doudec.
        /// </summary>
        public Doudec Divide(Doudec other)
        {
            if (IsDecimal && other.IsDecimal)
            {
                try
                {
                    return fixedPoint / other.fixedPoint;
                }
                catch (OverflowException) { }
            }
            return Double / other.Double;
        }
        /// <summary>
        /// Returns the remainder between the current Doudec and the given Doudec.
        /// </summary>
        public Doudec Remainder(Doudec other)
        {
            if (IsDecimal && other.IsDecimal)
            {
                try
                {
                    return fixedPoint % other.fixedPoint;
                }
                catch (OverflowException) { }
            }
            return Double % other.Double;
        }

        
        #region public Methods
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

        /// <summary>
        /// Returns the given double converted to Doudec.
        /// Conversion to decimal is attempted, but aborted if data changes due to rounding errors, as well as any other errors such as OverflowException.
        /// </summary>
        public static Doudec FromDouble(double dbl)
        {
            if (MathUtils.TryToDecimalStrictly(dbl, out decimal dec))
            {
                return new Doudec(dec);
            }
            else
            {
                return new Doudec(dbl);
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
        public static implicit operator Doudec(sbyte i) => new Doudec((decimal)i);
        public static implicit operator Doudec(short i) => new Doudec((decimal)i);
        public static implicit operator Doudec(int i) => new Doudec((decimal)i);
        public static implicit operator Doudec(long i) => new Doudec((decimal)i);
        public static explicit operator Doudec(BigInteger i) => FromBigInteger(i);

        public static implicit operator Doudec(byte i) => new Doudec((decimal)i);
        public static implicit operator Doudec(ushort i) => new Doudec((decimal)i);
        public static implicit operator Doudec(uint i) => new Doudec((decimal)i);
        public static implicit operator Doudec(ulong i) => new Doudec((decimal)i);
        public static explicit operator Doudec(UBigInteger i) => FromBigInteger(i);

        // floating point -> Doudec
        public static implicit operator Doudec(float flt) => FromDouble(flt);
        public static implicit operator Doudec(double dbl) => FromDouble(dbl);
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
        public static explicit operator double(Doudec d) => d.Double;
        public static explicit operator decimal(Doudec d) => d.Decimal;
        #endregion
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
            // Try Parsing both double and decimal.
            bool isDecimal = decimal.TryParse(s, out decimal dec);
            bool isDouble = double.TryParse(s, out double dbl);
            //

            if (isDecimal)
            {
                if (isDouble)
                {
                    if (Convert.ToDouble(dec) == dbl)
                    {
                        result = new Doudec(dec);
                    }
                    else
                    {
                        result = new Doudec(dbl);
                    }
                }
                else
                {
                    result = new Doudec(dec);
                }
            }
            else if (isDouble)
            {
                result = new Doudec(dbl);
            }
            else
            {
                result = default;
                return false;
            }

            return true;
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
        public static Doudec operator -(Doudec dd) => dd.IsDouble ? new Doudec(-dd.floatingPoint) : new Doudec(-dd.fixedPoint);
        public static Doudec operator +(Doudec dd) => dd;

        public static Doudec operator +(Doudec left, Doudec right) => left.Add(right);
        public static Doudec operator -(Doudec left, Doudec right) => left.Subract(right);
        public static Doudec operator *(Doudec left, Doudec right) => left.Multiply(right);
        public static Doudec operator /(Doudec left, Doudec right) => left.Divide(right);
        public static Doudec operator %(Doudec left, Doudec right) => left.Remainder(right);

        public static Doudec operator ++(Doudec dd) => dd.Increment();
        public static Doudec operator --(Doudec dd) => dd.Decrement();

        public static bool operator ==(Doudec left, Doudec right) => left.Equals(right);
        public static bool operator !=(Doudec left, Doudec right) => !left.Equals(right);

        public static bool operator >(Doudec left, Doudec right) => left.CompareTo(right) > 0;
        public static bool operator >=(Doudec left, Doudec right) => left.CompareTo(right) > 0 || left.Equals(right);
        public static bool operator <(Doudec left, Doudec right) => left.CompareTo(right) < 0;
        public static bool operator <=(Doudec left, Doudec right) => left.CompareTo(right) < 0 || left.Equals(right);
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
