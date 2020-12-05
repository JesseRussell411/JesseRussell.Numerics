#define USE_OP
#define ALLOW_INT_IN_PARSE
#define newFraction
//#define oldFraction

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlTypes;

namespace JesseRussell.Numerics
{
#if newFraction
    /// <summary>
    /// Represents a fraction of arbitrary size.
    /// </summary>
    /// 
    /// <Author>
    /// Jesse Russell
    /// </Author>
    public struct Fraction : IComparable<Fraction>, IComparable<BigInteger>, IComparable<FractionOperation>, IComparable, IEquatable<Fraction>, IEquatable<BigInteger>
    {
        #region public derived Properties
        // *derived: meaning that these properties must be derived from other fields or properties. Essentially, they cannot increase the size of the struct.

        /// <summary>
        /// True if negative, false otherwise.
        /// </summary>
        public bool IsNegative => (!IsUndefined) && (Numerator.Sign == -1) ^ (Denominator.Sign == -1);

        public bool IsWhole => Numerator % Denominator == 0;

        /// <summary>
        /// True if numerator is zero, false otherwise.
        /// </summary>
        public bool IsZero => Numerator.IsZero;

        /// <summary>
        /// True if denominator is zero, false otherwise.
        /// </summary>
        public bool IsUndefined => Denominator.IsZero;

        /// <summary>
        /// Returns the sign (negative one, one, or zero) of the current fraction.
        /// </summary>
        public int Sign => IsZero ? 0 : IsNegative ? -1 : 1;

        public Fraction AbsoluteValue => new Fraction(BigInteger.Abs(Numerator), BigInteger.Abs(Denominator));

        /// <summary>
        /// Returns the unsigned representation of the Numerator.
        /// </summary>
        public UBigInteger UNumerator => new UBigInteger(Numerator);

        /// <summary>
        /// Returns the unsigned representation of the Denominator.
        /// </summary>
        public UBigInteger UDenominator => new UBigInteger(Denominator);
        #endregion

        #region public readonly Fields
        /// <summary>
        /// Top number.
        /// </summary>
        public readonly BigInteger Numerator;

        /// <summary>
        /// Bottom number.
        /// </summary>
        public readonly BigInteger Denominator;
        #endregion

        #region public Constructors
        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public Fraction(BigInteger numerator) : this(numerator, 1) { }

        public Fraction(UBigInteger numerator, UBigInteger denominator, bool isNegative)
        {
            Numerator = (BigInteger)numerator * (isNegative ? -1 : 1);
            Denominator = denominator;
        }
        #endregion

        #region public Methods
        #region Math
        /// <summary>
        /// Returns the current fraction with the combination sign moves to the numerator.    1/-2 --> -1/2    -1/-2 --> 1/2
        /// </summary>
        public Fraction SimplifySign()
        {
            return new Fraction(BigInteger.Abs(Numerator) * (IsNegative ? -1 : 1), BigInteger.Abs(Denominator));
        }

        /// <summary>
        /// Returns the current fraction simplified.
        /// </summary>
        /// <returns></returns>
        public Fraction Simplify()
        {
            if (Numerator.IsZero) return Zero;
            if (Denominator.IsZero) return Undefined;

            bool negative = IsNegative;
            BigInteger numerator = BigInteger.Abs(Numerator);
            BigInteger denominator = BigInteger.Abs(Denominator);

            BigInteger gcd;
            while ((gcd = BigInteger.GreatestCommonDivisor(numerator, denominator)) > 1)
            {
                numerator /= gcd;
                denominator /= gcd;
            }

            return new Fraction(numerator * (negative ? -1 : 1), denominator);
        }

        /// <summary>
        /// Returns the unsimplified addition of the current fraction and other.
        /// </summary>
        public Fraction Add(Fraction other)
        {
            #region copied to Subtract(Fraction other)
            // Simplify sign
            BigInteger numerator = Numerator * Denominator.Sign;
            BigInteger other_numerator = other.Numerator * other.Denominator.Sign;
            //

            // Equalize denominators
            BigInteger denominator;
            if (Denominator != other.Denominator)
            {
                numerator *= BigInteger.Abs(other.Denominator);
                other_numerator *= BigInteger.Abs(Denominator);
                denominator = BigInteger.Abs(Denominator * other.Denominator);
            }
            else
            {
                denominator = Denominator;
            }
            //
            #endregion

            return new Fraction(numerator + other_numerator, denominator);
        }

        /// <summary>
        /// Returns the unsimplified subtraction of the current fraction and other.
        /// </summary>
        public Fraction Subtract(Fraction other)
        {
            #region copied from Add(Fraction other)
            // Simplify sign
            BigInteger numerator = Numerator * Denominator.Sign;
            BigInteger other_numerator = other.Numerator * other.Denominator.Sign;
            //

            // Equalize denominators
            BigInteger denominator;
            if (Denominator != other.Denominator)
            {
                numerator *= BigInteger.Abs(other.Denominator);
                other_numerator *= BigInteger.Abs(Denominator);
                denominator = BigInteger.Abs(Denominator * other.Denominator);
            }
            else
            {
                denominator = Denominator;
            }
            //
            #endregion

            return new Fraction(numerator - other_numerator, denominator);
        }

        /// <summary>
        /// Returns the unsimplified multiplication of the current fraction and other.
        /// </summary>
        public Fraction Multiply(Fraction other) => new Fraction(Numerator * other.Numerator, Denominator * other.Denominator);

        /// <summary>
        /// Returns the unsimplified division of the current fraction and other.
        /// </summary>
        public Fraction Divide(Fraction other) => new Fraction(Numerator * other.Denominator, Denominator * other.Numerator);

        /// <summary>
        /// Returns the unsimplified exponentiation of the current fraction and other.
        /// </summary>
        public Fraction Power(int exponent)
        {
            if (exponent < 0)
            {
                exponent = Math.Abs(exponent);
                return new Fraction(
                    BigInteger.Pow(Denominator, exponent),
                    BigInteger.Pow(Numerator, exponent));
            }
            else
            {
                return new Fraction(
                    BigInteger.Pow(Numerator, exponent),
                    BigInteger.Pow(Denominator, exponent));
            }
        }

        /// <summary>
        /// Returns the current fraction plus 1.
        /// </summary>
        public Fraction Increment() => new Fraction(Numerator + Denominator, Denominator);

        /// <summary>
        /// Returns the current fraction minus 1.
        /// </summary>
        public Fraction Decrement() => new Fraction(Numerator - Denominator, Denominator);

        /// <summary>
        /// Returns the whole portion of the fraction. Example: 5/2 equals 2 + 1/2, so the Floor of 5/2 would return 2.
        /// </summary>
        /// <returns>The whole portion of the fraction.</returns>
        public Fraction Truncate() => ToBigInteger();

        public Fraction Floor()
        {
            if (IsWhole) return this;
            else if (IsNegative) return ToBigInteger() - 1;
            else return ToBigInteger();
        }
        public Fraction Ceiling()
        {
            if (IsWhole) return this;
            else if (IsNegative) return ToBigInteger();
            else return ToBigInteger() + 1;
        }

        /// <summary>
        /// Returns the unsimplified remainder of the division of the current fraction and other.
        /// </summary>
        /// <param name="other">The Fraction that the current fraction is being divided by.</param>
        /// <returns>The remainder from the division.</returns>
        public Fraction Remainder(Fraction other) => Subtract(Divide(other).Floor().Multiply(other));

        /// <summary>
        /// Returns the unsimplified negation of the current fraction.
        /// </summary>
        /// <returns>The unsimplified negation of the current fraction.</returns>
        public Fraction Negate() => new Fraction(Numerator * -1, Denominator);
        #endregion

        #region Comparison

        /// <summary>
        /// Returns true if the numerators and denominators of the unsimplified current fraction and the unsimplified other fraction are equal.
        /// </summary>
        public bool HardEquals(Fraction other)
        {
            return Numerator == other.Numerator &&
                   Denominator == other.Denominator;
        }

        /// <summary>
        /// Returns the result of Fraction.HardEquals(Fraction other) on the simplified current fraction and the simplified other fraction.
        /// </summary>
        public bool SoftEquals(Fraction other)
        {
            return Sign == other.Sign &&
                   IsUndefined == other.IsUndefined &&
                   Simplify().HardEquals(other.Simplify());
        }

        /// <summary>
        /// Returns whether other is equal to the current fraction.
        /// </summary>
        public bool Equals(Fraction other) => SoftEquals(other);

        /// <summary>
        /// Returns whether bi is equal to the current fraction.
        /// </summary>
        public bool Equals(BigInteger bi)
        {
            Fraction simplified = Simplify();
            return simplified.Denominator == 1 && simplified.Numerator == bi;
        }
        public bool Equals(double d) => Equals(FromDouble(d));
        public bool Equals(float f) => Equals(FromDouble(f));
        public bool Equals(decimal dec) => Equals(FromDecimal(dec));
        public bool Equals(Doudec dd) => Equals(FromDoudec(dd));

        /// <summary>
        /// Returns whether ift is equal to the current fraction.
        /// </summary>
        public bool Equals(IntFloat ift) => ift.IsInt ? Equals(ift.Int) : Equals(ift.Float);

        /// <summary>
        /// Returns whether fo is equal to the current fraction.
        /// </summary>
        public bool Equals(FractionOperation fo) => SoftEquals(fo.Unsimplified);

        /// <summary>
        /// Returns a hash code representing the current fraction.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);

        /// <summary>
        /// Returns whether obj is equal to the current fraction.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            return obj switch
            {
                Fraction f => Equals((Fraction)f),
                FractionOperation fo => Equals((FractionOperation)fo),
                sbyte sb => Equals((BigInteger)sb),
                short s => Equals((BigInteger)s),
                int i => Equals((BigInteger)i),
                long l => Equals((BigInteger)l),
                BigInteger bi => Equals((BigInteger)bi),
                byte b => Equals((BigInteger)b),
                ushort us => Equals((BigInteger)us),
                uint ui => Equals((BigInteger)ui),
                ulong ul => Equals((BigInteger)ul),
                UBigInteger ubi => Equals((BigInteger)ubi),
                double d => Equals(d),
                float f => Equals(f),
                Doudec dd => Equals(dd),
                IntFloat ift => Equals(ift),
                _ => throw new ArgumentException("The parameter must be a fraction, integer, or floating point number.")
            };
        }
        public int CompareTo(Fraction other)
        {
            BigInteger numerator = BigInteger.Abs(Numerator) * BigInteger.Abs(other.Denominator);
            BigInteger other_numerator = BigInteger.Abs(other.Numerator) * BigInteger.Abs(Denominator);

            return (numerator * Sign).CompareTo(other_numerator * other.Sign);
        }
        public int CompareTo(BigInteger big) => CompareTo((Fraction)big);
        public int CompareTo(FractionOperation fo) => CompareTo((Fraction)fo);
        public int CompareTo(double d) => CompareTo((Fraction)d);
        public int CompareTo(decimal d) => CompareTo((Fraction)d);
        public int CompareTo(IntFloat ift) => ift.IsInt ? CompareTo(ift.Int) : CompareTo(ift.Float);
        public int CompareTo(object obj)
        {
            return obj switch
            {
                Fraction f => CompareTo(f),
                FractionOperation fo => CompareTo(fo),
                BigInteger bi => CompareTo(bi),
                long l => CompareTo((BigInteger)l),
                int i => CompareTo((BigInteger)i),
                short s => CompareTo((BigInteger)s),
                sbyte sb => CompareTo((BigInteger)sb),
                UBigInteger ubi => CompareTo((BigInteger)ubi),
                byte b => CompareTo((BigInteger)b),
                ushort us => CompareTo((BigInteger)us),
                uint ui => CompareTo((BigInteger)ui),
                ulong ul => CompareTo((BigInteger)ul),
                double d => CompareTo(d),
                decimal d => CompareTo(d),
                float f => CompareTo(f),
                IntFloat ift => CompareTo(ift),
                _ => throw new ArgumentException("Parameter must be a fraction, integer, double, float, IntFloat, or decimal."),
            };
        }
        #endregion

        #region Conversion
        public BigInteger ToBigInteger() => Numerator / Denominator;
        public BigInteger ToBigInteger(BigInteger remainder) => BigInteger.DivRem(Numerator, Denominator, out remainder);

        public double ToDouble() => (double)Numerator / (double)Denominator;
        public float ToFloat() => (float)Numerator / (float)Denominator;
        public decimal ToDecimal() => (decimal)Numerator / (decimal)Denominator;
        public Doudec ToDoudec() => (Doudec)Numerator / (Doudec)Denominator;
        public IntFloat ToIntFloat() => IsWhole ? (IntFloat)ToBigInteger() : ToDoudec();
        #endregion
        /// <summary>
        /// Returns a string representation of the current fraction.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
        #endregion

        #region public static Methods
        #region Math
        public static double Log(Fraction f) => Math.Log(f.ToDouble());
        public static double Log(Fraction f, double newBase) => Math.Log(f.ToDouble(), newBase);
        public static double Log10(Fraction f) => Math.Log10(f.ToDouble());
        public static Fraction Min(Fraction a, Fraction b) => a < b ? a : b;
        public static Fraction Max(Fraction a, Fraction b) => a > b ? a : b;
        public static Fraction Abs(Fraction f) => f.AbsoluteValue;
        #endregion

        //#region Math
        ///// <summary>
        ///// Returns the unsimplified result of adding left and right.
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Fraction Add(Fraction left, Fraction right) => left.Add(right);

        ///// <summary>
        ///// Returns the unsimplified result of subtracting right from left.
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Fraction Subtract(Fraction left, Fraction right) => left.Subtract(right);

        ///// <summary>
        ///// Returns the unsimplified result of multiplying left and right.
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Fraction Multiply(Fraction left, Fraction right) => left.Multiply(right);

        ///// <summary>
        ///// Returns the unsimplified result of dividing right from left.
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Fraction Divide(Fraction left, Fraction right) => left.Divide(right);

        ///// <summary>
        ///// Returns the unsimplified remainder of the division of right from left.
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static Fraction Rem(Fraction left, Fraction right) => left.Remainder(right);

        ///// <summary>
        ///// Returns the unsimplified negation of the provided fraction.
        ///// </summary>
        ///// <param name="f">The provided fraction.</param>
        ///// <returns>The unsimplified negation of the provided fraction.</returns>
        //public static Fraction Neg(Fraction f) => f.Negate();

        //public static Fraction Floor(Fraction f)
        //#endregion

        #region Operators
        public static FractionOperation operator -(Fraction f) => f.Negate();
        public static FractionOperation operator +(Fraction f) => f;

        public static FractionOperation operator +(Fraction left, Fraction right) => left.Add(right);
        public static Doudec operator +(Fraction left, Doudec right) => left.ToDoudec() + right;
        public static FractionOperation operator -(Fraction left, Fraction right) => left.Subtract(right);
        public static Doudec operator -(Fraction left, Doudec right) => left.ToDoudec() - right;
        public static FractionOperation operator *(Fraction left, Fraction right) => left.Multiply(right);
        public static Doudec operator *(Fraction left, Doudec right) => left.ToDoudec() * right;
        public static FractionOperation operator /(Fraction left, Fraction right) => left.Divide(right);
        public static Doudec operator /(Fraction left, Doudec right) => left.ToDoudec() / right;
        public static FractionOperation operator %(Fraction left, Fraction right) => left.Remainder(right);
        public static Doudec operator %(Fraction left, Doudec right) => left.ToDoudec() % right;
        public static Fraction operator ++(Fraction f) => f.Increment();
        public static Fraction operator --(Fraction f) => f.Decrement();

        public static bool operator <(Fraction left, Fraction right) => left.CompareTo(right) < 0;
        public static bool operator >(Fraction left, Fraction right) => left.CompareTo(right) > 0;
        public static bool operator <=(Fraction left, Fraction right) => left.CompareTo(right) <= 0;
        public static bool operator >=(Fraction left, Fraction right) => left.CompareTo(right) >= 0;

        public static bool operator ==(Fraction left, Fraction right) => left.SoftEquals(right);
        public static bool operator !=(Fraction left, Fraction right) => !left.SoftEquals(right);
        #endregion

        public static FractionOperation Pow(Fraction left, int right) => left.Power(right);
        public static FractionOperation Pow(FractionOperation left, int right) => left.Unsimplified.Power(right);

        #region Parse
        public static bool TryParse(string s, out Fraction result)
        {
            string[] split = s.Split('/', 2);
            if (split.Length == 2)
            {
                if (BigInteger.TryParse(split[0], out BigInteger numerator) && BigInteger.TryParse(split[1], out BigInteger denominator))
                {
                    result = new Fraction(numerator, denominator);
                    return true;
                }
                else
                {
                    result = default;
                    return false;
                }
            }
            else if (split.Length == 1)
            {
                if (BigInteger.TryParse(split[0], out BigInteger numerator))
                {
                    result = new Fraction(numerator);
                    return true;
                }
                else
                {
                    result = default;
                    return false;
                }
            }
            else // *assume 0
            {
                result = default;
                return false;
            }
        }

        public static Fraction Parse(string s)
        {
            if (TryParse(s, out Fraction result))
            {
                return result;
            }
            else
            {
                throw new FormatException("String is not a valid fraction.");
            }
        }
        #endregion

        #region Conversion
        public static Fraction FromDoudec(Doudec dd) => dd.Value switch
        {
            double d => FromDouble(d),
            decimal dec => FromDecimal(dec),
            _ => FromDouble(dd.Double)
        };

        public static Fraction FromDouble(double d)
        {
            // Trying to convert to decimal first...
            if (MathUtils.TryToDecimalStrictly(d, out decimal dec))
            {
                return FromDecimal(dec);
            }

            // Can't use decimal, will have to fall back on double instead.
            double whole = Math.Truncate(d);
            double numerator = d - whole;

            #region Copied to FromDecimal(decimal d) with mods
            int i = 0;
            while (i < FROM_DOUBLE_PRECISION_LIMIT && numerator % 1 != 0)
            {
                numerator *= 10;
                ++i;
            }


            return new Fraction((BigInteger)whole, 1) + new Fraction((BigInteger)numerator, BigInteger.Pow(10, i));
            #endregion
        }
        public static Fraction FromDecimal(decimal d)
        {
            decimal whole = Math.Truncate(d);
            decimal numerator = d - whole;

            #region Copied from FromDouble(double d) with mods
            int i = 0;
            while (i < FROM_DECIMAL_PRECISION_LIMIT && numerator % 1 != 0)
            {
                numerator *= 10;
                ++i;
            }


            return new Fraction((BigInteger)whole, 1) + new Fraction((BigInteger)numerator, BigInteger.Pow(10, i));
            #endregion
        }
        #endregion

        #region Casts
        public static implicit operator Fraction(BigInteger bi) => new Fraction(bi);

        public static implicit operator Fraction(sbyte sb) => new Fraction((BigInteger)sb);
        public static implicit operator Fraction(short s) => new Fraction((BigInteger)s);
        public static implicit operator Fraction(int i) => new Fraction((BigInteger)i);
        public static implicit operator Fraction(long l) => new Fraction((BigInteger)l);

        public static implicit operator Fraction(byte b) => new Fraction((BigInteger)b);
        public static implicit operator Fraction(ushort us) => new Fraction((BigInteger)us);
        public static implicit operator Fraction(uint ui) => new Fraction((BigInteger)ui);
        public static implicit operator Fraction(ulong ul) => new Fraction((BigInteger)ul);

        public static explicit operator BigInteger(Fraction f) => f.ToBigInteger();
        public static explicit operator long(Fraction f) => (long)f.ToBigInteger();
        public static explicit operator int(Fraction f) => (int)f.ToBigInteger();
        public static explicit operator short(Fraction f) => (short)f.ToBigInteger();
        public static explicit operator sbyte(Fraction f) => (sbyte)f.ToBigInteger();

        public static explicit operator UBigInteger(Fraction f) => (UBigInteger)f.ToBigInteger();
        public static explicit operator ulong(Fraction f) => (ulong)f.ToBigInteger();
        public static explicit operator uint(Fraction f) => (uint)f.ToBigInteger();
        public static explicit operator ushort(Fraction f) => (ushort)f.ToBigInteger();
        public static explicit operator byte(Fraction f) => (byte)f.ToBigInteger();

        public static explicit operator double(Fraction f) => f.ToDouble();
        public static explicit operator float(Fraction f) => f.ToFloat();
        public static explicit operator decimal(Fraction f) => f.ToDecimal();
        public static explicit operator Doudec(Fraction f) => f.ToDoudec();

        public static explicit operator IntFloat(Fraction f) => f.ToIntFloat();

        public static explicit operator Fraction(double d) => FromDouble(d);
        public static explicit operator Fraction(float f) => FromDouble(f);
        public static explicit operator Fraction(decimal dec) => FromDecimal(dec);
        public static explicit operator Fraction(Doudec dd) => FromDoudec(dd);
        #endregion
        #endregion

        #region public static readonly Fields
        public static readonly Fraction Zero = new Fraction(0);
        public static readonly Fraction Undefined = new Fraction(0, 0);
        #endregion

        #region private const Fields
        private const int FROM_DOUBLE_PRECISION_LIMIT = 326;
        private const int FROM_DECIMAL_PRECISION_LIMIT = 31;
        #endregion
    }

    public struct FractionOperation : IComparable<Fraction>, IComparable<BigInteger>, IComparable<FractionOperation>, IComparable
    {
        #region public Properties
        public BigInteger Numerator => Unsimplified.Numerator;
        public BigInteger Denominator => Unsimplified.Denominator;
        public UBigInteger UNumerator => Unsimplified.UNumerator;
        public BigInteger UDenominator => Unsimplified.UDenominator;
        public bool IsNegative => Unsimplified.IsNegative;
        public bool IsZero => Unsimplified.IsZero;
        public bool IsUndefined => Unsimplified.IsUndefined;
        public BigInteger Sign => Unsimplified.Sign;
        #endregion

        #region public readonly Fields
        public readonly Fraction Unsimplified;
        #endregion

        #region public Constructors
        public FractionOperation(Fraction unsimplified)
        {
            Unsimplified = unsimplified;
        }

        public FractionOperation(BigInteger numerator, BigInteger denominator) : this(new Fraction(numerator, denominator)) { }
        public FractionOperation(UBigInteger numerator, UBigInteger denominator, bool isNegative) : this(new Fraction(numerator, denominator, isNegative)) { }
        public FractionOperation(BigInteger numerator) : this(new Fraction(numerator)) { }
        #endregion

        #region public Methods
        public override string ToString()
        {
            return Unsimplified.Simplify().ToString();
        }

        #region Comparison
        public int CompareTo(Fraction f) => Unsimplified.CompareTo(f);
        public int CompareTo(BigInteger big) => Unsimplified.CompareTo(big);
        public int CompareTo(FractionOperation other) => Unsimplified.CompareTo(other);
        public int CompareTo(object obj) => Unsimplified.CompareTo(obj);
        #endregion
        #region Math
        public FractionOperation Floor() => Unsimplified.Floor();
        public FractionOperation Ceiling() => Unsimplified.Ceiling();
        public FractionOperation Truncate() => Unsimplified.Truncate();
        #endregion
        #endregion

        public static FractionOperation operator -(FractionOperation fo) => -fo.Unsimplified;
        public static FractionOperation operator +(FractionOperation fo) => +fo.Unsimplified;

        public static FractionOperation operator +(FractionOperation left, Fraction right) => left.Unsimplified + right;
        public static FractionOperation operator +(Fraction left, FractionOperation right) => left + right.Unsimplified;
        public static FractionOperation operator +(FractionOperation left, FractionOperation right) => left.Unsimplified + right.Unsimplified;

        public static FractionOperation operator -(FractionOperation left, Fraction right) => left.Unsimplified - right;
        public static FractionOperation operator -(Fraction left, FractionOperation right) => left - right.Unsimplified;
        public static FractionOperation operator -(FractionOperation left, FractionOperation right) => left.Unsimplified - right.Unsimplified;

        public static FractionOperation operator *(FractionOperation left, Fraction right) => left.Unsimplified * right;
        public static FractionOperation operator *(Fraction left, FractionOperation right) => left * right.Unsimplified;
        public static FractionOperation operator *(FractionOperation left, FractionOperation right) => left.Unsimplified * right.Unsimplified;

        public static FractionOperation operator /(FractionOperation left, Fraction right) => left.Unsimplified / right;
        public static FractionOperation operator /(Fraction left, FractionOperation right) => left / right.Unsimplified;
        public static FractionOperation operator /(FractionOperation left, FractionOperation right) => left.Unsimplified / right.Unsimplified;

        public static FractionOperation operator ++(FractionOperation fo) => fo.Unsimplified.Increment();
        public static FractionOperation operator --(FractionOperation fo) => fo.Unsimplified.Decrement();

        public static implicit operator FractionOperation(Fraction f) => new FractionOperation(f);
        public static implicit operator Fraction(FractionOperation fo) => fo.Unsimplified.Simplify();
    }
#endif

#if oldFraction
    public struct Fraction : IComparable<Fraction>
    {
    #region public Properties
        /// <summary>
        /// Top number.
        /// </summary>
        public uBigInteger Numerator { get; }
        /// <summary>
        /// Bottom number.
        /// </summary>
        public uBigInteger Denominator { get; }
        /// <summary>
        /// Stores the sign of the fraction. True for negative. False for positive.
        /// </summary>
        public bool Negative { get; }

    #region Derived Properties
        /// <summary>
        /// Returns Numerator as positive or negative depending on the value of the Negative property. Numerator * -1 if Negative; Numerator * 1 if not Negative.
        /// </summary>
        public BigInteger SignedNumerator => Numerator.Value * (Negative ? -1 : 1);

        public bool IsUndefined => Denominator == 0;
        public bool IsZero => !IsUndefined && Numerator == 0;
    #endregion
    #endregion

    #region public static Properties
        public Fraction Undefined => new Fraction(0, 0, false);
        public Fraction Zero => new Fraction(0, 1, false);
    #endregion

    #region public Constructors
        public Fraction(BigInteger? numerator = null, BigInteger? denominator = null, bool? negative = null)
        {
            Numerator = (uBigInteger?)numerator ?? numerator_default;
            Denominator = (uBigInteger?)denominator ?? denominator_default;
            Negative = (Numerator.IsZero || Denominator.IsZero) ? false : negative ?? ((numerator < 0) ^ (denominator < 0));
        }
    #endregion

    #region public Methods
        public Fraction Clone(uBigInteger? numerator_new = null, uBigInteger? denominator_new = null, bool? isNegative_new = null)
            => new Fraction(numerator_new ?? Numerator, denominator_new ?? Denominator, isNegative_new ?? Negative);
        public override string ToString()
        {
            return $"{(Negative ? "-" : "")}{Numerator}/{Denominator}";
        }
    #region Comparison
        public int CompareTo(Fraction other)
        {
            Fraction a = EqualizeDenominators(other, out Fraction b);

            return (a.Numerator.Value * (a.Negative ? -1 : 1)).CompareTo(b.Numerator.Value * (a.Negative ? -1 : 1));

        }
    #endregion
    #region Equality
        public override int GetHashCode()
        {
            Fraction this_simp = Simplify();
            return HashCode.Combine(this_simp.Numerator, this_simp.Denominator, this_simp.Negative);
        }
        public override bool Equals(object obj)
        {
            Fraction? nfrac = obj as Fraction?;
            return nfrac != null && SoftEquals((Fraction)nfrac);
        }

        /// <summary>
        /// Returns true if and only if all parameters of other match all parameters of this Fraction.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool HardEquals(Fraction other)
        {
            return Negative == other.Negative &&
                   Numerator == other.Numerator &&
                   Denominator == other.Denominator;
        }

        /// <summary>
        /// Simplifies both fractions before calling HardEquals. Determines whether the this Fraction is mathematically equal to other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool SoftEquals(Fraction other) => Simplify().HardEquals(other.Simplify());
    #endregion
    #region Math
        /// <summary>
        /// Efficiently multiplies the calling fraction by -1;
        /// </summary>
        /// <returns></returns>
        public Fraction Negate() => new Fraction(Numerator, Denominator, !Negative);

        /// <summary>
        /// Adds other to the calling function.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Fraction Add(Fraction other)
        {
            uBigInteger numerator = Numerator;
            uBigInteger denominator = Denominator;
            bool negative = Negative;
    #region Copied to Subtract(Fraction other)
            uBigInteger other_num = other.Numerator;

            // Equalize the denominators:
            if (denominator != other.Denominator)
            {
                numerator *= other.Denominator;
                other_num = other.Numerator * denominator;
                denominator *= other.Denominator;
            }
            //


            if (negative ^ other.Negative)
            {
                if (numerator > other_num)
                {
                    //IsNegative doesn't change.
                    numerator -= other_num;
                }
                else if (Numerator < other_num)
                {
                    negative = other.Negative;
                    numerator = other_num - numerator;
                }
                else // ==
                {
                    numerator = 0;
                    negative = false;
                }
            }
            else
            {
                numerator += other_num;
                negative ^= other.Negative;
            }

            return new Fraction(numerator, denominator, negative);
    #endregion
        }

        /// <summary>
        /// Subtracts other from the calling function.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>

        public Fraction Subtract(Fraction other)
        {
            uBigInteger numerator = Numerator;
            uBigInteger denominator = Denominator;
            bool negative = !Negative; // *This is the only part that was changed from Add.

    #region copied from Add(Fraction other)
            uBigInteger other_num = other.Numerator;

            // Equalize the denominators:
            if (denominator != other.Denominator)
            {
                numerator *= other.Denominator;
                other_num = other.Numerator * denominator;
                denominator *= other.Denominator;
            }
            //


            if (negative ^ other.Negative)
            {
                if (numerator > other_num)
                {
                    //IsNegative doesn't change.
                    numerator -= other_num;
                }
                else if (Numerator < other_num)
                {
                    negative = other.Negative;
                    numerator = other_num - numerator;
                }
                else // ==
                {
                    numerator = 0;
                    negative = false;
                }
            }
            else
            {
                numerator += other_num;
                negative ^= other.Negative;
            }

            return new Fraction(numerator, denominator, negative);
    #endregion
        }


        /// <summary>
        /// Multiplies the calling fraction by other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Fraction Multiply(Fraction other)
        {
            return new Fraction(
                Numerator * other.Numerator,
                Denominator * other.Denominator,
                Negative ^ other.Negative);
        }

        /// <summary>
        /// Divides the calling fraction by another other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Fraction Divide(Fraction other)
        {
            return new Fraction(
                Numerator * other.Denominator,
                Denominator * other.Numerator,
                Negative ^ other.Negative);
        }

        /// <summary>
        /// Replaces the calling fraction with its reciprocal.
        /// </summary>
        /// <returns></returns>
        public Fraction Reciprocate() => new Fraction(Denominator, Numerator, Negative);
        /// <summary>
        /// Raises the calling fraction to exp.
        /// </summary>
        /// <param name="exo">The exponent.</param>
        /// <returns>The result of raising the calling fraction to exp.</returns>
        public Fraction Power(int exp)
        {
            if (exp < 0)
            {
                exp = Math.Abs(exp);
                return new Fraction(
                    uBigInteger.Pow(Denominator, exp),
                    BigInteger.Pow(SignedNumerator, exp)
                    );
            }
            else
            {
                return new Fraction(
                    BigInteger.Pow(SignedNumerator, exp),
                    uBigInteger.Pow(Denominator, exp)
                    );
            }
        }



        /// <summary>
        /// Simplifies the calling fraction.
        /// </summary>
        /// <returns></returns>
        public Fraction Simplify()
        {
            uBigInteger numerator = Numerator;
            uBigInteger denominator = Denominator;

    #region Special cases
            if (numerator.IsZero)
            {
                return Zero;
            }

            if (denominator.IsZero)
            {
                return Undefined;
            }
    #endregion

            uBigInteger gcd;
            // Main loop...
            while ((gcd = uBigInteger.GreatestCommonDivisor(numerator, denominator)) > 1)
            {
                numerator /= gcd;
                denominator /= gcd;
            }

            // Done.
            return new Fraction(numerator, denominator, Negative);
        }
    #endregion

    #region Conversion
        public IntFloat ToIntFloat()
        {
            if (TryToInteger(out BigInteger big))
            {
                return big;
            }
            else
            {
                return ToDouble();
            }
        }
        public double ToDouble() => (double)SignedNumerator / (double)Denominator;
        public bool TryToDecimal(out decimal result, out OverflowException ofe)
        {
            try
            {
                result = (decimal)SignedNumerator / (decimal)Denominator;
                ofe = null;
                return true;
            }
            catch (OverflowException e)
            {
                ofe = e;
                result = default;
                return false;
            }
        }
        public bool TryToDecimal(out decimal result)
        {
            try
            {
                result = (decimal)SignedNumerator / (decimal)Denominator;
                return true;
            }
            catch (OverflowException e)
            {
                result = default;
                return false;
            }
        }
        public decimal ToDecimal()
        {
            if (TryToDecimal(out decimal result, out OverflowException efo))
            {
                return result;
            }
            else
            {
                throw efo;
            }
        }
        public bool TryToInteger(out BigInteger result)
        {
            result = BigInteger.DivRem(Numerator, Denominator, out BigInteger remainder);
            return remainder == 0;
        }
    #endregion
    #endregion

    #region public static Methods
    #region Math
        public FractionOperation Pow(Fraction f, int i) => new FractionOperation(f.Power(i));
    #endregion

    #region Parsing
        /// <summary>
        /// Tries to convert the string s to a fraction.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out Fraction result)
        {
            // Split string on the '/' character.
            string[] s_split = s.Split('/', 2).Select(str => str.Trim()).ToArray();

            // Try to parse the split string...
            if (s_split.Length == 2)
            {
                // Try to parse numerator and denominator...
                if (BigInteger.TryParse(s_split[0], out BigInteger numerator) && BigInteger.TryParse(s_split[1], out BigInteger denominator))
                {
                    result = new Fraction(numerator, denominator);
                    return true;
                }
                else
                {
                    result = new Fraction();
                    return false;
                }
            }
            else
            {
#if ALLOW_INT_IN_PARSE
                // Try to parse just the numerator...
                if (BigInteger.TryParse(s_split.FirstOrDefault(), out BigInteger big))
                {
                    result = big;
                    return true;
                }
                else
                {
                    result = new Fraction();
                    return false;
                }
#else
                    result = new Fraction();
                    return false;
#endif
            }
        }

        public static Fraction Parse(string s)
        {
            if (TryParse(s, out Fraction result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"{s} could not be parsed.");
            }
        }
    #endregion

    #region Casts
        public static implicit operator Fraction(BigInteger big) => new Fraction(big);
        public static implicit operator Fraction(uBigInteger big) => new Fraction(big);
        public static implicit operator Fraction(long l) => new Fraction(l);
        public static implicit operator Fraction(int i) => new Fraction(i);
        public static implicit operator Fraction(short s) => new Fraction(s);
        public static implicit operator Fraction(sbyte sb) => new Fraction(sb);
        public static implicit operator Fraction(ulong ul) => new Fraction(ul);
        public static implicit operator Fraction(uint ui) => new Fraction(ui);
        public static implicit operator Fraction(ushort us) => new Fraction(us);
        public static implicit operator Fraction(byte b) => new Fraction(b);

        public static explicit operator double(Fraction f) => f.ToDouble();
        public static explicit operator decimal(Fraction f) => f.ToDecimal();
    #endregion

    #region Operators

#if USE_OP
        public static FractionOperation operator +(Fraction left, Fraction right) => left.Add(right);
        public static FractionOperation operator -(Fraction left, Fraction right) => left.Add(right);

        public static FractionOperation operator *(Fraction left, Fraction right) => left.Multiply(right);
        public static FractionOperation operator /(Fraction left, Fraction right) => left.Divide(right);
#else
            public static Fraction operator +(Fraction left, Fraction right) => left.Add(right).Simplify();
            public static Fraction operator -(Fraction left, Fraction right) => left.Add(right).Simplify();

            public static Fraction operator *(Fraction left, Fraction right) => left.Multiply(right).Simplify();
            public static Fraction operator /(Fraction left, Fraction right) => left.Divide(right).Simplify();
#endif

        public static bool operator >(Fraction left, Fraction right) => left.CompareTo(right) > 0;
        public static bool operator >=(Fraction left, Fraction right) => left.CompareTo(right) >= 0;
        public static bool operator <(Fraction left, Fraction right) => left.CompareTo(right) < 0;
        public static bool operator <=(Fraction left, Fraction right) => left.CompareTo(right) <= 0;

        public static bool operator ==(Fraction left, Fraction right) => left.SoftEquals(right);
        public static bool operator !=(Fraction left, Fraction right) => !left.SoftEquals(right);
    #endregion
    #endregion

    #region hidden Methods
        internal Fraction EqualizeDenominators(Fraction other) => Clone(Numerator * other.Denominator, Denominator * other.Denominator);
        internal Fraction EqualizeDenominators(Fraction other, out Fraction other_equalized)
        {
            other_equalized = other.Clone(other.Numerator * other.Denominator, other.Denominator * other.Denominator);
            return EqualizeDenominators(other);
        }
    #endregion

    #region hidden static Methods
        private static uBigInteger findGCD(uBigInteger a, uBigInteger b)
        {
            // Make sure b is smaller than a...
            if (b > a)
            {
                // Swap a and b:
                uBigInteger temp = a;
                a = b;
                b = temp;
                //
            }

            // Special case: b is zero.
            if (b == 0) { return a; }

            // Main loop...
            while (a % b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            // Finished.
            return b;
        }
    #endregion

    #region public Defaults
        public static readonly Fraction Default = new Fraction(numerator_default, denominator_default, negative_default);
    #endregion

    #region hidden Defaults
        private static readonly uBigInteger numerator_default = 0;
        private static readonly uBigInteger denominator_default = 1;
        private const bool negative_default = false;
    #endregion
    }

    /// <summary>
    /// Represents a Fraction that does not auto-simplify between operations such as: +, -, *, /, and %. Simplifies when cast to Fraction, and on call of ToString(). This keeps the fractions from being simplified between every operation in a string of operations like: var frac = new Fraction(1, 2) + 6 - new Fraction(6, 4) * new Fraction(8, 6) / new Fraction(9, 5); simplify will only be called once in this line.If you don't want the simplified version, either store the result in a FractionsOperation object or wrap the expression in parenthesis and call .Value at the end.
    /// </summary>
    public struct FractionOperation
    {
        /// <summary>
        /// The Fraction represented.
        /// </summary>
        public Fraction Unsimplified { get; }
        public FractionOperation(Fraction f)
        {
            Unsimplified = f;
        }

        public override string ToString()
        {
            return Unsimplified.Simplify().ToString();
        }

    #region Operators
        public static FractionOperation operator +(FractionOperation a, Fraction b) => a.Unsimplified + b;
        public static FractionOperation operator +(Fraction a, FractionOperation b) => a + b.Unsimplified;
        public static FractionOperation operator +(FractionOperation a, FractionOperation b) => a.Unsimplified + b.Unsimplified;

        public static FractionOperation operator -(FractionOperation a, Fraction b) => a.Unsimplified - b;
        public static FractionOperation operator -(Fraction a, FractionOperation b) => a - b.Unsimplified;
        public static FractionOperation operator -(FractionOperation a, FractionOperation b) => a.Unsimplified - b.Unsimplified;

        public static FractionOperation operator *(FractionOperation a, Fraction b) => a.Unsimplified * b;
        public static FractionOperation operator *(Fraction a, FractionOperation b) => a * b.Unsimplified;
        public static FractionOperation operator *(FractionOperation a, FractionOperation b) => a.Unsimplified * b.Unsimplified;

        public static FractionOperation operator /(FractionOperation a, Fraction b) => a.Unsimplified / b;
        public static FractionOperation operator /(Fraction a, FractionOperation b) => a / b.Unsimplified;
        public static FractionOperation operator /(FractionOperation a, FractionOperation b) => a.Unsimplified / b.Unsimplified;
    #endregion

    #region Casts
        public static implicit operator Fraction(FractionOperation fo) => fo.Unsimplified.Simplify();
        public static implicit operator FractionOperation(Fraction f) => new FractionOperation(f);

        public static explicit operator double(FractionOperation fo) => (double)fo.Unsimplified;
        public static explicit operator decimal(FractionOperation fo) => (decimal)fo.Unsimplified;

    #endregion
        public Fraction ToFraction() => Unsimplified.Simplify();

    #region Pass-through
        public Fraction Simplify() => Unsimplified.Simplify();
        public Fraction Add(Fraction other) => ToFraction().Add(other);
        public Fraction Subtract(Fraction other) => ToFraction().Subtract(other);
        public Fraction Multiply(Fraction other) => ToFraction().Multiply(other);
        public Fraction Divide(Fraction other) => ToFraction().Divide(other);
        public Fraction Neg() => ToFraction().Negate();
        public Fraction Power(int exp) => ToFraction().Power(exp);
        public Fraction Reciprocate() => ToFraction().Reciprocate();
    #endregion
    }
#endif

    public static class FractionUtils
    {
        public static Fraction NextFraction(this Random rand)
        {
            return new Fraction(rand.Next(int.MinValue, int.MaxValue), rand.Next(int.MinValue, int.MaxValue));
        }

        public static Fraction NextFraction(this Random rand, int minNumerator, int maxNumerator, int minDenominator, int maxDenominator)
        {
            return new Fraction(rand.Next(minNumerator, maxNumerator), rand.Next(minDenominator, maxDenominator));
        }

        public static Fraction ToFraction(this double d) => Fraction.FromDouble(d);
        public static Fraction ToFraction(this float d) => Fraction.FromDouble(d);
        public static Fraction ToFraction(this decimal d) => Fraction.FromDecimal(d);
    }
}