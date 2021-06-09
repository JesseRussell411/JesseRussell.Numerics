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
        [FieldOffset(0)]
        public readonly bool IsDouble;
        [FieldOffset(1)]
        private readonly double floatingPoint;
        [FieldOffset(1)]
        private readonly decimal fixedPoint;
        public double Double => IsDouble ? floatingPoint : Convert.ToDouble(fixedPoint);
        public decimal Decimal => IsDouble ? Convert.ToDecimal(floatingPoint) : fixedPoint;
        public object Value => IsDouble ? (object)floatingPoint : fixedPoint;
        public bool IsDecimal => !IsDouble;

        // o---------------o
        // | Constructors: |
        // o---------------o
        public Doudec(double d)
        {
            IsDouble = true;
            fixedPoint = default;
            floatingPoint = d;
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
                else return false;
            }
            else if (!other.IsDouble)
            {
                if (TryToDecimal(out decimal this_dec))
                {
                    return this_dec.Equals(other.fixedPoint);
                }
                else return false;
            }
            else
            {
                return floatingPoint.Equals(other.floatingPoint);
            }
        }


        public bool Equals(double d) => Equals((Doudec)d);
        public bool Equals(decimal dec) => Equals((Doudec)dec);

        public bool Equals(sbyte i) => Equals((Doudec)i);
        public bool Equals(short i) => Equals((Doudec)i);
        public bool Equals(int i) => Equals((Doudec)i);
        public bool Equals(long i) => Equals((Doudec)i);
        public bool Equals(BigInteger i)
        {
            if (this % 1 != 0) return false;
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


        public bool Equals(byte i) => Equals((Doudec)i);
        public bool Equals(ushort i) => Equals((Doudec)i);
        public bool Equals(uint i) => Equals((Doudec)i);
        public bool Equals(ulong i) => Equals((Doudec)i);
        public bool Equals(UBigInteger ubig) => Equals((BigInteger)ubig);
        public override bool Equals(object obj) => obj switch
        {
            Doudec dd => Equals(dd),
            double d => Equals(d),
            float f => Equals(f),
            decimal dec => Equals(dec),

            sbyte i => Equals(i),
            short i => Equals(i),
            int i => Equals(i),
            long i => Equals(i),

            byte i => Equals(i),
            ushort i => Equals(i),
            uint i => Equals(i),
            ulong i => Equals(i),
            _ => throw new ArgumentException("Argument must be a Doudec, double, float, decimal, or integer")
        };

        public override int GetHashCode()
        {
            if (TryToDecimal(out decimal result))
            {
                return result.GetHashCode();
            }
            else
            {
                return Double.GetHashCode();
            }
        }

        #endregion
        public override string ToString() => IsDouble ? floatingPoint.ToString() : fixedPoint.ToString();
        
        public bool TryToDecimal(out decimal result)
        {
            if (!IsDouble)
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
        public static implicit operator Doudec(float f) => FromDouble(f);
        public static implicit operator Doudec(double f) => FromDouble(f);
        public static implicit operator Doudec(decimal dec) => FromDecimal(dec);
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

        #region double Passthrough
        public static bool IsNaN(Doudec d) => d.IsDouble && double.IsNaN(d.floatingPoint);
        public static bool IsInfinity(Doudec d) => d.IsDouble && double.IsInfinity(d.floatingPoint);
        public static bool IsFinite(Doudec d) => !d.IsDouble || double.IsFinite(d.floatingPoint);
        public static bool IsNegative(Doudec d) => d.IsDouble ? double.IsNegative(d.floatingPoint) : d.fixedPoint < 0;
        public static bool IsNormal(Doudec d) => d.IsDouble && double.IsNormal(d.floatingPoint);
        public static bool IsSubnormal(Doudec d) => d.IsDouble && double.IsSubnormal(d.floatingPoint);
        public static bool IsPositiveInfinity(Doudec d) => d.IsDouble && double.IsPositiveInfinity(d.floatingPoint);
        public static bool IsNegativeInfinity(Doudec d) => d.IsDouble && double.IsNegativeInfinity(d.floatingPoint);
        #endregion

        #region Parse
        public static bool TryParse(string s, out Doudec result)
        {
            // Try Parsing both double and decimal. Null means failed:
            decimal? dec = decimal.TryParse(s, out decimal decr) ? (decimal?)decr : null;
            double? d = double.TryParse(s, out double dr) ? (double?)dr : null;
            //

            // Base the result on that...
            if (d != null)
            {
                if (dec != null)
                {
                    //  double & decimal

                    if ((double)dec == d)
                    {
                        result = (decimal)dec;
                        return true;
                    }
                    else
                    {
                        result = (double)d;
                        return true;
                    }
                }
                else
                {
                    //  double & !decimal

                    result = (double)d;
                    return true;
                }
            }
            else
            {
                if (dec != null)
                {
                    //  !double & decimal

                    result = (decimal)dec;
                    return true;
                }
                else
                {
                    //  !double & !decimal

                    result = default;
                    return false;
                }
            }
        }
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
        public static Doudec Sqrt(Doudec x) => Math.Sqrt(x.Double);
        public static Doudec Pow(Doudec x, double y) => Math.Pow(x.Double, y);
        public static Doudec Floor(Doudec dd) => dd.IsDouble ? (Doudec) Math.Floor(dd.Double) : Math.Floor(dd.Decimal);
        public static Doudec Ceiling(Doudec dd) => dd.IsDouble ? (Doudec) Math.Ceiling(dd.Double) : Math.Ceiling(dd.Decimal);
        public static Doudec Truncate(Doudec dd) => dd.IsDouble ? (Doudec) Math.Truncate(dd.Double) : Math.Truncate(dd.Decimal);
        public static Doudec Abs(Doudec dd) => dd.IsDouble ? (Doudec) Math.Abs(dd.Double) : Math.Abs(dd.Decimal);
        public static Doudec Log(Doudec x) => Math.Log(x.Double);
        public static Doudec Log(Doudec x, Doudec newbase) => Math.Log(x.Double, newbase.Double);
        public static Doudec Log10(Doudec x) => Math.Log10(x.Double);
        public static int Sign(Doudec x) => x.IsDouble ? Math.Sign(x.floatingPoint) : Math.Sign(x.fixedPoint);
        public static Doudec Neg(Doudec x) => x.IsDouble ? (Doudec)(-x.floatingPoint) : -x.fixedPoint;
        #endregion

        #region Conversion
        public static Doudec FromDouble(double d) => MathUtils.TryToDecimalStrictly(d, out decimal dec) ? new Doudec(dec) : new Doudec(d);
        public static Doudec FromDecimal(decimal dec)
        {
            if (dec == 0) return new Doudec(decimal.Zero);
            return new Doudec(dec);
        }
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

        #region public static Properties { get; }
        public static Doudec NaN => new Doudec(double.NaN);
        public static Doudec PositiveInfinity => new Doudec(double.PositiveInfinity);
        public static Doudec NegativeInfinity => new Doudec(double.NegativeInfinity);
        public static Doudec Epsilon => new Doudec(double.Epsilon);
        #endregion
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
