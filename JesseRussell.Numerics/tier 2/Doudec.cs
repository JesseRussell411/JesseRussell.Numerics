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
        #region public Constructors
        public Doudec(double d)
        {
            doubleNotDecimal = true;
            decim = default;
            doub = d;
        }
        public Doudec(decimal dec)
        {
            doubleNotDecimal = false;
            doub = default;
            decim = dec;
        }

        public Doudec(sbyte i) : this((decimal)i) { }
        public Doudec(short i) : this((decimal)i) { }
        public Doudec(int i) : this((decimal)i) { }
        public Doudec(long i) : this((decimal)i) { }

        public Doudec(byte i) : this((decimal)i) { }
        public Doudec(ushort i) : this((decimal)i) { }
        public Doudec(uint i) : this((decimal)i) { }
        public Doudec(ulong i) : this((decimal)i) { }
        #endregion

        #region public Properties
        public double Double => doubleNotDecimal ? doub : Convert.ToDouble(decim);
        public decimal Decimal => doubleNotDecimal ? Convert.ToDecimal(doub) : decim;
        public object Value => doubleNotDecimal ? (object)doub : decim;
        public bool IsDouble => doubleNotDecimal;
        public bool IsDecimal => !doubleNotDecimal;
        #endregion

        #region public Methods
        #region Math
        public Doudec Increment() => doubleNotDecimal ? (Doudec)doub + 1.0 : decim + 1M;
        public Doudec Decrement() => doubleNotDecimal ? (Doudec)doub - 1.0 : decim - 1M;
        #endregion

        #region Comparison
        public int CompareTo(Doudec other)
        {
            if (doubleNotDecimal || other.doubleNotDecimal) return Double.CompareTo(other.Double);
            else return decim.CompareTo(other.decim);
        }
        public int CompareTo(double d) => CompareTo((Doudec)d);
        public int CompareTo(decimal dec) => CompareTo((Doudec)dec);
        public bool Equals(Doudec other)
        {
            if (!doubleNotDecimal)
            {
                if (other.TryToDecimal(out decimal other_dec))
                {
                    return decim.Equals(other_dec);
                }
                else return false;
            }
            else if (!other.doubleNotDecimal)
            {
                if (TryToDecimal(out decimal this_dec))
                {
                    return this_dec.Equals(other.decim);
                }
                else return false;
            }
            else
            {
                return doub.Equals(other.doub);
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
            if (doubleNotDecimal)
            {
                if (double_minValue_BigInteger <= i && i <= double_maxValue_BigInteger)
                {
                    return (BigInteger)doub == i;
                }
                else return false;
            }
            else
            {
                if (decimal_minValue_BigInteger <= i && i <= decimal_maxValue_BigInteger)
                {
                    return (BigInteger)decim == i;
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
        public override string ToString() => doubleNotDecimal ? doub.ToString() : decim.ToString();
        
        public bool TryToDecimal(out decimal result)
        {
            if (!doubleNotDecimal)
            {
                result = decim;
                return true;
            }
            else
            {
                return MathUtils.TryToDecimalStrictly(doub, out result);
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
        public static explicit operator sbyte(Doudec d) => d.doubleNotDecimal ? (sbyte)d.doub : (sbyte)d.decim;
        public static explicit operator short(Doudec d) => d.doubleNotDecimal ? (short)d.doub : (short)d.decim;
        public static explicit operator int(Doudec d) => d.doubleNotDecimal ? (int)d.doub : (int)d.decim;
        public static explicit operator long(Doudec d) => d.doubleNotDecimal ? (long)d.doub : (long)d.decim;
        public static explicit operator BigInteger(Doudec d) => d.doubleNotDecimal ? (BigInteger)d.doub : (BigInteger)d.decim;

        public static explicit operator byte(Doudec d) => d.doubleNotDecimal ? (byte)d.doub : (byte)d.decim;
        public static explicit operator ushort(Doudec d) => d.doubleNotDecimal ? (ushort)d.doub : (ushort)d.decim;
        public static explicit operator uint(Doudec d) => d.doubleNotDecimal ? (uint)d.doub : (uint)d.decim;
        public static explicit operator ulong(Doudec d) => d.doubleNotDecimal ? (ulong)d.doub : (ulong)d.decim;
        public static explicit operator UBigInteger(Doudec d) => d.doubleNotDecimal ? (UBigInteger)d.doub : (UBigInteger)d.decim;

        // Doudec -> floating point
        public static explicit operator float(Doudec d) => (float)d.Double;
        public static explicit operator double(Doudec d) => (double)d.Double;
        public static explicit operator decimal(Doudec d) => d.Decimal;
        #endregion
        #endregion

        #region double Passthrough
        public static bool IsNaN(Doudec d) => d.doubleNotDecimal && double.IsNaN(d.doub);
        public static bool IsInfinity(Doudec d) => d.doubleNotDecimal && double.IsInfinity(d.doub);
        public static bool IsFinite(Doudec d) => !d.doubleNotDecimal || double.IsFinite(d.doub);
        public static bool IsNegative(Doudec d) => d.doubleNotDecimal ? double.IsNegative(d.doub) : d.decim < 0;
        public static bool IsNormal(Doudec d) => d.doubleNotDecimal && double.IsNormal(d.doub);
        public static bool IsSubnormal(Doudec d) => d.doubleNotDecimal && double.IsSubnormal(d.doub);
        public static bool IsPositiveInfinity(Doudec d) => d.doubleNotDecimal && double.IsPositiveInfinity(d.doub);
        public static bool IsNegativeInfinity(Doudec d) => d.doubleNotDecimal && double.IsNegativeInfinity(d.doub);
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
            if (left.doubleNotDecimal || right.doubleNotDecimal)
            {
                return left.Double + right.Double;
            }
            else
            {
                try
                {
                    return left.decim + right.decim;
                }
                catch (OverflowException)
                {
                    return left.Double + right.Double;
                }
            }
        }
        public static Doudec Subtract(Doudec left, Doudec right)
        {
            if (left.doubleNotDecimal || right.doubleNotDecimal)
            {
                return left.Double - right.Double;
            }
            else
            {
                try
                {
                    return left.decim - right.decim;
                }
                catch (OverflowException)
                {
                    return left.Double - right.Double;
                }
            }
        }
        public static Doudec Multiply(Doudec left, Doudec right)
        {
            if (left.doubleNotDecimal || right.doubleNotDecimal)
            {
                return left.Double * right.Double;
            }
            else
            {
                try
                {
                    return left.decim * right.decim;
                }
                catch (OverflowException)
                {
                    return left.Double * right.Double;
                }
            }
        }
        public static Doudec Divide(Doudec left, Doudec right)
        {
            if (left.doubleNotDecimal || right.doubleNotDecimal)
            {
                return left.Double / right.Double;
            }
            else
            {
                try
                {
                    return left.decim / right.decim;
                }
                catch (OverflowException)
                {
                    return left.Double / right.Double;
                }
            }
        }
        public static Doudec Remainder(Doudec left, Doudec right)
        {
            if (left.doubleNotDecimal || right.doubleNotDecimal)
            {
                return left.Double % right.Double;
            }
            else
            {
                try
                {
                    return left.decim % right.decim;
                }
                catch (OverflowException)
                {
                    return left.Double % right.Double;
                }
            }
        }
        public static Doudec Pow(Doudec x, double y) => Math.Pow(x.Double, y);
        public static Doudec Floor(Doudec dd) => dd.doubleNotDecimal ? (Doudec) Math.Floor(dd.Double) : Math.Floor(dd.Decimal);
        public static Doudec Ceiling(Doudec dd) => dd.doubleNotDecimal ? (Doudec) Math.Ceiling(dd.Double) : Math.Ceiling(dd.Decimal);
        public static Doudec Truncate(Doudec dd) => dd.doubleNotDecimal ? (Doudec) Math.Truncate(dd.Double) : Math.Truncate(dd.Decimal);
        public static Doudec Abs(Doudec dd) => dd.doubleNotDecimal ? (Doudec) Math.Abs(dd.Double) : Math.Abs(dd.Decimal);
        public static Doudec Log(Doudec x) => Math.Log(x.Double);
        public static Doudec Log(Doudec x, Doudec newbase) => Math.Log(x.Double, newbase.Double);
        public static Doudec Log10(Doudec x) => Math.Log10(x.Double);
        public static int Sign(Doudec x) => x.doubleNotDecimal ? Math.Sign(x.doub) : Math.Sign(x.decim);
        public static Doudec Neg(Doudec x) => x.doubleNotDecimal ? (Doudec)(-x.doub) : -x.decim;
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

        #region private readonly Fields
        [FieldOffset(0)]
        private readonly bool doubleNotDecimal;
        [FieldOffset(1)]
        private readonly double doub;
        [FieldOffset(1)]
        private readonly decimal decim;
        #endregion
    }

    public static class DoudecUtils
    {
        public static Doudec Sum(this IEnumerable<Doudec> items) => items.Aggregate((total, next) => total + next);
    }
}
