//#define USE_OP
//#define ALLOW_INT_IN_PARSE
//#define newFraction
////#define oldFraction

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace JesseRussell.Numerics
{
    public readonly struct Fraction
    {
        public readonly BigInteger Numerator;
        public readonly BigInteger Denominator;

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }
        public Fraction(BigInteger numerator)
        {
            Numerator = numerator;
            Denominator = 1;
        }
        public static readonly Fraction Zero = new Fraction(0, 1);
        public static readonly Fraction Undefined = new Fraction(0, 0);

        public bool IsNegative => Numerator.Sign * Denominator.Sign < 0;
        public bool IsZero => Numerator.IsZero && !Denominator.IsZero;
        public bool IsUndefined => Denominator.IsZero;
        public bool IsWhole => Numerator % Denominator == 0;
        public int Sign => Numerator.Sign * Denominator.Sign;
        public Fraction Abs => new Fraction(BigInteger.Abs(Numerator), BigInteger.Abs(Denominator));

        #region operation
        /// <summary>
        /// Returns the fraction in its simplest form. Examples: 3/6 -> 1/2  2/-4 -> -1/2  -5/-20 -> 1/4
        /// </summary>
        public Fraction Simplify()
        {
            // Special cases:
            if (IsZero) return Zero;
            if (IsUndefined) return Undefined;

            // Record without sign:
            BigInteger numerator_abs = BigInteger.Abs(Numerator);
            BigInteger denominator_abs = BigInteger.Abs(Denominator);

            // Get greatest common divisor:
            BigInteger gcd = BigInteger.GreatestCommonDivisor(numerator_abs, denominator_abs);

            // Return with gcd applied and the sign re-applied:
            return new Fraction(
                (numerator_abs / gcd) * Sign,
                denominator_abs / gcd);
        }

        /// <summary>
        /// Returns the Fraction with its sign moved to the numerator. Example: 1/-2 -> -1/2  -1/-2 -> 1/2   -1/2 -> -1/2   1/2 -> 1/2
        /// </summary>
        public Fraction SimplifySign() => new Fraction(
                BigInteger.Abs(Numerator) * Sign,
                BigInteger.Abs(Denominator));

        public static (Fraction a, Fraction b) MakeCompatible(Fraction a, Fraction b)
        {
            a = a.SimplifySign();
            b = b.SimplifySign();
            if (a.Denominator == b.Denominator) return (a, b);

            BigInteger commonDenominator = a.Denominator * b.Denominator;
            return (
                new Fraction(
                    a.Numerator * b.Denominator,
                    commonDenominator),
                new Fraction(
                    b.Numerator * a.Denominator,
                    commonDenominator));
        }

        public Fraction Add(Fraction other) => SimplifySign().Add_helper(other.SimplifySign());
        private Fraction Add_helper(Fraction other)
        {
            if (Denominator == other.Denominator)
                return new Fraction(Numerator + other.Numerator, Denominator);
            return new Fraction(Numerator * other.Denominator + other.Numerator * Denominator, Denominator * other.Denominator);
        }

        public Fraction Subtract(Fraction other) => SimplifySign().Subtract_helper(other.SimplifySign());
        private Fraction Subtract_helper(Fraction other)
        {
            if (Denominator == other.Denominator)
                return new Fraction(Numerator - other.Numerator, Denominator);
            return new Fraction(Numerator * other.Denominator - other.Numerator * Denominator, Denominator * other.Denominator);
        }

        public Fraction Multiply(Fraction other) => new Fraction(Numerator * other.Numerator, Denominator * other.Denominator);
        public Fraction Divide(Fraction other) => new Fraction(Numerator * other.Denominator, Denominator * other.Numerator);

        /// <summary>
        /// Returns the unsimplified remainder of the division of the current fraction and other.
        /// </summary>
        /// <param name="other">The Fraction that the current fraction is being divided by.</param>
        /// <returns>The remainder from the division.</returns>
        public Fraction Remainder(Fraction other) => Subtract(Divide(other).Truncate().Multiply(other));

        public Fraction Negate() => new Fraction(BigInteger.Negate(Numerator), Denominator);

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
        /// Returns the mediant (numerator + other.numerator / denominator + other.denominator) with the given fraction.
        /// </summary>
        public Fraction Mediant(Fraction other) => new Fraction(Numerator + other.Numerator, Denominator + other.Denominator);

        /// <summary>
        /// Returns the current fraction plus 1.
        /// </summary>
        public Fraction Increment() => new Fraction(Numerator + Denominator, Denominator);

        /// <summary>
        /// Returns the current fraction minus 1.
        /// </summary>
        public Fraction Decrement() => new Fraction(Numerator - Denominator, Denominator);

        /// <summary>
        /// Returns the whole portion of the fraction. Example: 5/2 equals 2 + 1/2, so the Truncation of 5/2 would return 2.
        /// </summary>
        /// <returns>The whole portion of the fraction.</returns>
        public Fraction Truncate() => new Fraction(ToBigInteger(), 1);

        /// <summary>
        /// Returns the fractions rounded to the next lowest integer.
        /// </summary>
        /// <returns></returns>
        public Fraction Floor()
        {
            if (IsWhole) return this;
            else if (IsNegative) return new Fraction(ToBigInteger() - 1, 1);
            else return new Fraction(ToBigInteger(), 1);
        }

        /// <summary>
        /// Returns the fraction rounded to the next highest integer.
        /// </summary>
        /// <returns></returns>
        public Fraction Ceiling()
        {
            if (IsWhole) return this;
            else if (IsNegative) return new Fraction(ToBigInteger(), 1);
            else return new Fraction(ToBigInteger() + 1, 1);
        }

        public static Fraction Min(Fraction a, Fraction b) => b < a ? b : a;
        public static Fraction Max(Fraction a, Fraction b) => b > a ? b : a;
        #endregion

        // o============o
        // | Operators: |
        // o============o
        public static Fraction operator ++(Fraction self) => self.Increment().Simplify();
        public static Fraction operator --(Fraction self) => self.Decrement().Simplify();
        public static FractionOperation operator +(Fraction left, Fraction right) => (FractionOperation)left.Add(right);
        public static FractionOperation operator -(Fraction left, Fraction right) => (FractionOperation)left.Subtract(right);
        public static FractionOperation operator *(Fraction left, Fraction right) => (FractionOperation)left.Multiply(right);
        public static FractionOperation operator /(Fraction left, Fraction right) => (FractionOperation)left.Divide(right);
        public static FractionOperation operator %(Fraction left, Fraction right) => (FractionOperation)left.Remainder(right);
        public static bool operator ==(Fraction a, Fraction b) => a.Equals(b);
        public static bool operator !=(Fraction a, Fraction b) => !a.Equals(b);
        public static bool operator >(Fraction a, Fraction b) => a.CompareTo(b) > 0;
        public static bool operator <(Fraction a, Fraction b) => a.CompareTo(b) < 0;
        public static bool operator >=(Fraction a, Fraction b) => a.CompareTo(b) >= 0;
        public static bool operator <=(Fraction a, Fraction b) => a.CompareTo(b) <= 0;

        // o========o
        // | Casts: |
        // o========o
        //to
        public static explicit operator BigInteger(Fraction f) => f.ToBigInteger();
        public static explicit operator float(Fraction f) => f.ToFloat();
        public static explicit operator double(Fraction f) => f.ToDouble();
        public static explicit operator decimal(Fraction f) => f.ToDecimal();
        public static explicit operator Doudec(Fraction f) => f.ToDoudec();


        //from
        public static implicit operator Fraction(BigInteger i) => new Fraction(i, 1);
        public static implicit operator Fraction(long i) => new Fraction(i, 1);
        public static implicit operator Fraction(int i) => new Fraction(i, 1);
        public static implicit operator Fraction(short i) => new Fraction(i, 1);
        public static implicit operator Fraction(sbyte i) => new Fraction(i, 1);
        public static implicit operator Fraction(UBigInteger i) => new Fraction(i, 1);
        public static implicit operator Fraction(ulong i) => new Fraction(i, 1);
        public static implicit operator Fraction(uint i) => new Fraction(i, 1);
        public static implicit operator Fraction(ushort i) => new Fraction(i, 1);
        public static implicit operator Fraction(byte i) => new Fraction(i, 1);
        public static explicit operator Fraction(float f) => FromDouble(f);
        public static explicit operator Fraction(double f) => FromDouble(f);
        public static implicit operator Fraction(decimal f) => FromDecimal(f);
        public static explicit operator Fraction(Doudec f) => FromDoudec(f);


        // o=============o
        // | Conversion: |
        // o=============o
        public BigInteger ToBigInteger() => Numerator / Denominator;
        public double ToDouble() => (double)Numerator / (double)Denominator;
        public float ToFloat() => (float)Numerator / (float)Denominator;
        public decimal ToDecimal() => (decimal)Numerator / (decimal)Denominator;
        public Doudec ToDoudec() => (Doudec)Numerator / (Doudec)Denominator;
        public static Fraction FromDouble(double d)
        {
            // fancier but less effective way of converting float to fraction:
            //const int ITERATION_LIMIT = 10000;
            //const double ACCURACY_THRESHOLD = double.Epsilon * 10e100;

            //// Safety checks:
            //if (double.IsNaN(d)) throw new InvalidCastException("Fraction cannot represent NaN");
            //if (double.IsInfinity(d)) throw new InvalidCastException("Fraction cannot represent infinity");

            //// record and remove sign
            //int sign = Math.Sign(d);
            //d = Math.Abs(d);

            //Fraction upper, lower, mediant;
            //upper = new Fraction((BigInteger)Math.Ceiling(d));
            //lower = new Fraction((BigInteger)Math.Floor(d));

            //// main loop:
            //for (int i = 0; i < ITERATION_LIMIT; ++i)
            //{
            //    mediant = lower.Mediant(upper);

            //    double mediant_double = mediant.ToDouble();

            //    if (d < mediant_double)
            //    {
            //        if (mediant_double - d < ACCURACY_THRESHOLD)
            //            return mediant * sign;
            //        upper = mediant;
            //    }
            //    else
            //    {
            //        if (d - mediant_double < ACCURACY_THRESHOLD)
            //            return mediant * sign;
            //        lower = mediant;
            //    }
            //}

            //// doing this part one more time so the second half of the last loop doesn't go to waste.
            //mediant = lower.Mediant(upper);

            //// return with sign:
            //return mediant * sign;
            const int ITERATION_LIMIT = 400;

            int sign = Math.Sign(d);
            d = Math.Abs(d);

            double whole = Math.Truncate(d);
            double numerator = d - whole;

            int exponent = 0;
            for (; exponent < ITERATION_LIMIT && numerator % 1 != 0; ++exponent)
                numerator *= 10;

            BigInteger denominator = 1;
            for (int i = 0; i < exponent; ++i)
                denominator *= 10;

            return (new Fraction((BigInteger)numerator, denominator) + (BigInteger)whole) * sign;
        }

        public static Fraction FromFloat(float f) => FromDouble(f);

        public static Fraction FromDecimal(decimal d)
        {
            const int ITERATION_LIMIT = 400;

            int sign = Math.Sign(d);
            d = Math.Abs(d);

            decimal whole = decimal.Truncate(d);
            decimal numerator = d - whole;

            int exponent = 0;
            for (; exponent < ITERATION_LIMIT && numerator % 1 != 0; ++exponent)
                numerator *= 10;

            BigInteger denominator = 1;
            for (int i = 0; i < exponent; ++i)
                denominator *= 10;

            return (new Fraction((BigInteger)numerator, denominator) + (BigInteger)whole) * sign;
        }

        public static Fraction FromDoudec(Doudec d) => d.IsDecimal ? FromDecimal(d.Decimal) : FromDouble(d.Double);

        // o=============o
        // | Comparison: |
        // o=============o
        #region comparison
        public int CompareTo(Fraction other)
        {
            (Fraction a, Fraction b) = MakeCompatible(this, other);

            return a.Numerator.CompareTo(b.Numerator);
        }

        public bool HardEquals(Fraction other) => Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);
        public bool Equals(Fraction other) => Simplify().HardEquals(other.Simplify());
        public bool Equals(BigInteger other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(long other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(int other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(short other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(sbyte other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(UBigInteger other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(ulong other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(uint other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(ushort other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public bool Equals(byte other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        public override bool Equals(object other)
        {
            return other switch
            {
                Fraction f => Equals(f),
                BigInteger bi => Equals(bi),
                long l => Equals(l),
                int i => Equals(i),
                short s => Equals(s),
                sbyte sb => Equals(sb),
                UBigInteger ubi => Equals(ubi),
                ulong ul => Equals(ul),
                uint ui => Equals(ui),
                ushort us => Equals(us),
                byte b => Equals(b),
                _ => false,
            };
        }

        public override int GetHashCode()
        {
            Fraction simp = Simplify();
            return HashCode.Combine(simp.Numerator, simp.Denominator);
        }
        #endregion

        // parsing:
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
            else // *assume length 0
            {
                result = default;
                return false;
            }
        }

        public static Fraction Parse(string s)
        {
            if (TryParse(s, out Fraction result))
                return result;
            else
                throw new FormatException("The string is not a valid fraction.");
        }

        public override string ToString() => $"{Numerator}/{Denominator}";
    }




    public readonly struct FractionOperation
    {
        public readonly Fraction Unsimplified;
        //public Fraction Simplified => Unsimplified.Simplify();
        public Fraction Simplify() => Unsimplified.Simplify();
        public FractionOperation(Fraction unsimplified) => Unsimplified = unsimplified;

        // casting:
            // to
        public static implicit operator Fraction(FractionOperation fo) => fo.Unsimplified.Simplify();

            // from
        public static explicit operator FractionOperation(Fraction f) => new FractionOperation(f);
        public static implicit operator FractionOperation(BigInteger i) => new FractionOperation(i);
        public static implicit operator FractionOperation(long i) => new FractionOperation(i);
        public static implicit operator FractionOperation(int i) => new FractionOperation(i);
        public static implicit operator FractionOperation(short i) => new FractionOperation(i);
        public static implicit operator FractionOperation(sbyte i) => new FractionOperation(i);
        public static implicit operator FractionOperation(UBigInteger i) => new FractionOperation(i);
        public static implicit operator FractionOperation(ulong i) => new FractionOperation(i);
        public static implicit operator FractionOperation(uint i) => new FractionOperation(i);
        public static implicit operator FractionOperation(ushort i) => new FractionOperation(i);
        public static implicit operator FractionOperation(byte i) => new FractionOperation(i);

        // operators:
        public static FractionOperation operator ++(FractionOperation self) => (FractionOperation)self.Unsimplified.Increment();
        public static FractionOperation operator --(FractionOperation self) => (FractionOperation)self.Unsimplified.Decrement();
        public static FractionOperation operator +(FractionOperation left, FractionOperation right) => (FractionOperation)left.Unsimplified.Add(right.Unsimplified);
        public static FractionOperation operator +(FractionOperation left, Fraction right) => (FractionOperation)left.Unsimplified.Add(right);
        public static FractionOperation operator +(Fraction left, FractionOperation right) => (FractionOperation)left.Add(right.Unsimplified);
        public static FractionOperation operator -(FractionOperation left, FractionOperation right) => (FractionOperation)left.Unsimplified.Subtract(right.Unsimplified);
        public static FractionOperation operator -(FractionOperation left, Fraction right) => (FractionOperation)left.Unsimplified.Subtract(right);
        public static FractionOperation operator -(Fraction left, FractionOperation right) => (FractionOperation)left.Subtract(right.Unsimplified);
        public static FractionOperation operator *(FractionOperation left, FractionOperation right) => (FractionOperation)left.Unsimplified.Multiply(right.Unsimplified);
        public static FractionOperation operator *(FractionOperation left, Fraction right) => (FractionOperation)left.Unsimplified.Multiply(right);
        public static FractionOperation operator *(Fraction left, FractionOperation right) => (FractionOperation)left.Multiply(right.Unsimplified);
        public static FractionOperation operator /(FractionOperation left, FractionOperation right) => (FractionOperation)left.Unsimplified.Divide(right.Unsimplified);
        public static FractionOperation operator /(FractionOperation left, Fraction right) => (FractionOperation)left.Unsimplified.Divide(right);
        public static FractionOperation operator /(Fraction left, FractionOperation right) => (FractionOperation)left.Divide(right.Unsimplified);
        public static FractionOperation operator %(FractionOperation left, FractionOperation right) => (FractionOperation)left.Unsimplified.Remainder(right.Unsimplified);
        public static FractionOperation operator %(FractionOperation left, Fraction right) => (FractionOperation)left.Unsimplified.Remainder(right);
        public static FractionOperation operator %(Fraction left, FractionOperation right) => (FractionOperation)left.Remainder(right.Unsimplified);
        public static bool operator ==(FractionOperation a, FractionOperation b) => a.Unsimplified.Equals(b.Unsimplified);
        public static bool operator !=(FractionOperation a, FractionOperation b) => !a.Unsimplified.Equals(b.Unsimplified);
        public static bool operator >(FractionOperation a, FractionOperation b) => a.Unsimplified.CompareTo(b.Unsimplified) > 0;
        public static bool operator <(FractionOperation a, FractionOperation b) => a.Unsimplified.CompareTo(b.Unsimplified) < 0;
        public static bool operator >=(FractionOperation a, FractionOperation b) => a.Unsimplified.CompareTo(b.Unsimplified) >= 0;
        public static bool operator <=(FractionOperation a, FractionOperation b) => a.Unsimplified.CompareTo(b.Unsimplified) <= 0;

        // object override methods:
        public override string ToString() => Unsimplified.Simplify().ToString();
        public override bool Equals(object obj) => Unsimplified.Equals(obj);
        public override int GetHashCode() => Unsimplified.GetHashCode();
    }


    /// <summary>
    /// Extension methods for Fraction.
    /// </summary>
    public static class FractionExtensions
    {
        public static Fraction NextFraction(this Random rand)
        {
            return new Fraction(rand.Next(int.MinValue, int.MaxValue), rand.Next(int.MinValue, int.MaxValue));
        }

        public static Fraction NextFraction(this Random rand, int minNumerator, int maxNumerator, int minDenominator, int maxDenominator)
        {
            return new Fraction(rand.Next(minNumerator, maxNumerator), rand.Next(minDenominator, maxDenominator));
        }

        public static Fraction Sum(this IEnumerable<Fraction> items)
        {
            Fraction total = Fraction.Zero;
            int i = 0;

            foreach (Fraction f in items)
            {
                total = total.Add(f);
                i++;

                if (i >= 8)
                {
                    i = 0;
                    total = total.Simplify();
                }
            }

            return total.Simplify();
        }
    }
}


//namespace JesseRussell.Numerics
//{
//    /// <summary>
//    /// Represents a fraction of arbitrary size.
//    /// </summary>
//    public readonly struct Fraction : IComparable<Fraction>, IComparable<BigInteger>, IComparable<FractionOperation>, IComparable, IEquatable<Fraction>, IEquatable<BigInteger>
//    {
//        #region public derived Properties
//        // *derived: meaning that these properties must be derived from other fields or properties. Essentially, they cannot increase the size of the struct.

//        /// <summary>
//        /// True if negative, false otherwise.
//        /// </summary>
//        public bool IsNegative => (!IsUndefined) && (Numerator.Sign == -1) ^ (Denominator.Sign == -1);

//        /// <summary>
//        /// True if this fractions represents a whole number. Essentially if the denominator equals 1 after simplification.
//        /// </summary>
//        public bool IsWhole => Denominator != 0 && Numerator % Denominator == 0;

//        /// <summary>
//        /// True if numerator is zero, false otherwise.
//        /// </summary>
//        public bool IsZero => Numerator.IsZero && !IsUndefined;

//        /// <summary>
//        /// True if denominator is zero, false otherwise.
//        /// </summary>
//        public bool IsUndefined => Denominator.IsZero;

//        /// <summary>
//        /// Returns the sign (negative one, one, or zero) of the current fraction.
//        /// </summary>
//        public int Sign => Numerator.Sign * Denominator.Sign;

//        /// <summary>
//        /// Returns the absoluteValue.
//        /// </summary>
//        public Fraction AbsoluteValue => new Fraction(BigInteger.Abs(Numerator), BigInteger.Abs(Denominator));

//        /// <summary>
//        /// Returns the unsigned representation of the Numerator.
//        /// </summary>
//        public UBigInteger UNumerator => new UBigInteger(Numerator);

//        /// <summary>
//        /// Returns the unsigned representation of the Denominator.
//        /// </summary>
//        public UBigInteger UDenominator => new UBigInteger(Denominator);
//        #endregion

//        #region public readonly Fields
//        /// <summary>
//        /// Top number.
//        /// </summary>
//        public readonly BigInteger Numerator;

//        /// <summary>
//        /// Bottom number.
//        /// </summary>
//        public readonly BigInteger Denominator;
//        #endregion

//        #region public Constructors
//        public Fraction(BigInteger numerator, BigInteger denominator)
//        {
//            Numerator = numerator;
//            Denominator = denominator;
//        }

//        public Fraction(BigInteger numerator) : this(numerator, 1) { }

//        public Fraction(UBigInteger numerator, UBigInteger denominator, bool isNegative)
//        {
//            Numerator = (BigInteger)numerator * (isNegative ? -1 : 1);
//            Denominator = denominator;
//        }
//        #endregion

//        #region public Methods
//        #region Math
//        /// <summary>
//        /// Returns the current fraction with the combination sign moved to the numerator.    1/-2 --> -1/2    -1/-2 --> 1/2
//        /// </summary>
//        public Fraction SimplifySign()
//        {
//            return new Fraction(BigInteger.Abs(Numerator) * (IsNegative ? -1 : 1), BigInteger.Abs(Denominator));
//        }

//        /// <summary>
//        /// Returns the current fraction simplified.
//        /// </summary>
//        /// <returns></returns>
//        public Fraction Simplify()
//        {
//            // Special circumstances:
//            if (Denominator.IsZero) return Undefined;
//            if (Numerator.IsZero) return Zero;
//            //

//            // Get sign.
//            bool negative = IsNegative;

//            // Remove sign:
//            BigInteger numerator = BigInteger.Abs(Numerator);
//            BigInteger denominator = BigInteger.Abs(Denominator);
//            //

//            // Get gcd.
//            BigInteger gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);

//            // Apply gcd:
//            numerator /= gcd;
//            denominator /= gcd;
//            //


//            // Re-apply sign and finish.
//            return new Fraction(numerator * (negative ? -1 : 1), denominator);
//        }

//        /// <summary>
//        /// Returns the unsimplified addition of the current fraction and other.
//        /// </summary>
//        public Fraction Add(Fraction other)
//        {
//            #region copied to Subtract(Fraction other)
//            // Simplify sign:
//            BigInteger numerator = Numerator * Denominator.Sign;
//            BigInteger other_numerator = other.Numerator * other.Denominator.Sign;
//            //

//            // Equalize denominators:
//            BigInteger denominator;
//            if (Denominator != other.Denominator)
//            {
//                numerator *= BigInteger.Abs(other.Denominator);
//                other_numerator *= BigInteger.Abs(Denominator);
//                denominator = BigInteger.Abs(Denominator * other.Denominator);
//            }
//            else
//            {
//                denominator = Denominator;
//            }
//            //
//            #endregion

//            return new Fraction(numerator + other_numerator, denominator);
//        }

//        /// <summary>
//        /// Returns the unsimplified subtraction of the current fraction and other.
//        /// </summary>
//        public Fraction Subtract(Fraction other)
//        {
//            #region copied from Add(Fraction other)
//            // Simplify sign:
//            BigInteger numerator = Numerator * Denominator.Sign;
//            BigInteger other_numerator = other.Numerator * other.Denominator.Sign;
//            //

//            // Equalize denominators:
//            BigInteger denominator;
//            if (Denominator != other.Denominator)
//            {
//                numerator *= BigInteger.Abs(other.Denominator);
//                other_numerator *= BigInteger.Abs(Denominator);
//                denominator = BigInteger.Abs(Denominator * other.Denominator);
//            }
//            else
//            {
//                denominator = Denominator;
//            }
//            //
//            #endregion

//            return new Fraction(numerator - other_numerator, denominator);
//        }

//        /// <summary>
//        /// Returns the unsimplified multiplication of the current fraction and other.
//        /// </summary>
//        public Fraction Multiply(Fraction other) => new Fraction(Numerator * other.Numerator, Denominator * other.Denominator);

//        /// <summary>
//        /// Returns the unsimplified division of the current fraction and other.
//        /// </summary>
//        public Fraction Divide(Fraction other) => new Fraction(Numerator * other.Denominator, Denominator * other.Numerator);

//        /// <summary>
//        /// Returns the unsimplified exponentiation of the current fraction and other.
//        /// </summary>
//        public Fraction Power(int exponent)
//        {
//            if (exponent < 0)
//            {
//                exponent = Math.Abs(exponent);
//                return new Fraction(
//                    BigInteger.Pow(Denominator, exponent),
//                    BigInteger.Pow(Numerator, exponent));
//            }
//            else
//            {
//                return new Fraction(
//                    BigInteger.Pow(Numerator, exponent),
//                    BigInteger.Pow(Denominator, exponent));
//            }
//        }

//        /// <summary>
//        /// Returns the naive sum (numerator + other.numerator / denominator + other.denominator) with the given fraction.
//        /// </summary>
//        public Fraction NaiveSum(Fraction other) => new Fraction(Numerator + other.Numerator, Denominator + other.Denominator);

//        /// <summary>
//        /// Returns the current fraction plus 1.
//        /// </summary>
//        public Fraction Increment() => new Fraction(Numerator + Denominator, Denominator);

//        /// <summary>
//        /// Returns the current fraction minus 1.
//        /// </summary>
//        public Fraction Decrement() => new Fraction(Numerator - Denominator, Denominator);

//        /// <summary>
//        /// Returns the whole portion of the fraction. Example: 5/2 equals 2 + 1/2, so the Truncation of 5/2 would return 2.
//        /// </summary>
//        /// <returns>The whole portion of the fraction.</returns>
//        public Fraction Truncate() => ToBigInteger();

//        /// <summary>
//        /// Returns the fractions rounded to the next lowest integer.
//        /// </summary>
//        /// <returns></returns>
//        public Fraction Floor()
//        {
//            if (IsWhole) return this;
//            else if (IsNegative) return ToBigInteger() - 1;
//            else return ToBigInteger();
//        }

//        /// <summary>
//        /// Returns the fraction rounded to the next highest integer.
//        /// </summary>
//        /// <returns></returns>
//        public Fraction Ceiling()
//        {
//            if (IsWhole) return this;
//            else if (IsNegative) return ToBigInteger();
//            else return ToBigInteger() + 1;
//        }

//        /// <summary>
//        /// Returns the unsimplified remainder of the division of the current fraction and other.
//        /// </summary>
//        /// <param name="other">The Fraction that the current fraction is being divided by.</param>
//        /// <returns>The remainder from the division.</returns>
//        public Fraction Remainder(Fraction other) => Subtract(Divide(other).Truncate().Multiply(other));

//        /// <summary>
//        /// Returns the unsimplified negation of the current fraction.
//        /// </summary>
//        /// <returns>The unsimplified negation of the current fraction.</returns>
//        public Fraction Negate() => new Fraction(Numerator * -1, Denominator);
//        #endregion

//        #region Comparison

//        /// <summary>
//        /// Returns true if the numerators and denominators of the unsimplified current fraction and the unsimplified other fraction are equal.
//        /// </summary>
//        public bool HardEquals(Fraction other)
//        {
//            return Numerator == other.Numerator &&
//                   Denominator == other.Denominator;
//        }

//        /// <summary>
//        /// Returns the result of Fraction.HardEquals(Fraction other) on the simplified current fraction and the simplified other fraction.
//        /// </summary>
//        public bool SoftEquals(Fraction other)
//        {
//            return Sign == other.Sign &&
//                   IsUndefined == other.IsUndefined &&
//                   Simplify().HardEquals(other.Simplify());
//        }

//        /// <summary>
//        /// Returns whether other is equal to the current fraction.
//        /// </summary>
//        public bool Equals(Fraction other) => SoftEquals(other);

//        /// <summary>
//        /// Returns whether bi is equal to the current fraction.
//        /// </summary>
//        public bool Equals(BigInteger bi)
//        {
//            Fraction simplified = Simplify();
//            return simplified.Denominator == 1 && simplified.Numerator == bi;
//        }

//        /// <summary>
//        /// Returns whether d is equal to the current fraction.
//        /// </summary>
//        public bool Equals(double d) => Equals(FromDouble(d));

//        /// <summary>
//        /// Returns whether f is equal to the current fraction.
//        /// </summary>
//        public bool Equals(float f) => Equals(FromDouble(f));

//        /// <summary>
//        /// Returns whether dec is equal to the current fraction.
//        /// </summary>
//        public bool Equals(decimal dec) => Equals(FromDecimal(dec));

//        /// <summary>
//        /// Returns whether dd is equal to the current fraction.
//        /// </summary>
//        public bool Equals(Doudec dd) => Equals(FromDoudec(dd));

//        /// <summary>
//        /// Returns whether ift is equal to the current fraction.
//        /// </summary>
//        public bool Equals(IntFloat ift) => ift.IsInt ? Equals(ift.Int) : Equals(ift.Float);

//        /// <summary>
//        /// Returns whether fo is equal to the current fraction.
//        /// </summary>
//        public bool Equals(FractionOperation fo) => SoftEquals(fo.Unsimplified);

//        /// <summary>
//        /// Returns a hash code representing the current fraction.
//        /// </summary>
//        /// <returns></returns>
//        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);

//        /// <summary>
//        /// Returns whether obj is equal to the current fraction.
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <returns></returns>
//        public override bool Equals(Object obj)
//        {
//            return obj switch
//            {
//                Fraction f => Equals((Fraction)f),
//                FractionOperation fo => Equals((FractionOperation)fo),
//                sbyte sb => Equals((BigInteger)sb),
//                short s => Equals((BigInteger)s),
//                int i => Equals((BigInteger)i),
//                long l => Equals((BigInteger)l),
//                BigInteger bi => Equals((BigInteger)bi),
//                byte b => Equals((BigInteger)b),
//                ushort us => Equals((BigInteger)us),
//                uint ui => Equals((BigInteger)ui),
//                ulong ul => Equals((BigInteger)ul),
//                UBigInteger ubi => Equals((BigInteger)ubi),
//                double d => Equals(d),
//                float f => Equals(f),
//                Doudec dd => Equals(dd),
//                IntFloat ift => Equals(ift),
//                _ => throw new ArgumentException("The parameter must be a fraction, integer, or floating point number.")
//            };
//        }

//        /// <summary>
//        /// Returns an integer code from the comparison between the current fraction and other.
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns>Comparison int code.</returns>
//        public int CompareTo(Fraction other)
//        {
//            BigInteger numerator = BigInteger.Abs(Numerator) * BigInteger.Abs(other.Denominator);
//            BigInteger other_numerator = BigInteger.Abs(other.Numerator) * BigInteger.Abs(Denominator);

//            return (numerator * Sign).CompareTo(other_numerator * other.Sign);
//        }

//        /// <summary>
//        /// Returns an integer code from the comparison between the current fraction and big.
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns>Comparison int code.</returns>
//        public int CompareTo(BigInteger big) => CompareTo((Fraction)big);

//        /// <summary>
//        /// Returns an integer code from the comparison between the current fraction and fo.
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns>Comparison int code.</returns>
//        public int CompareTo(FractionOperation fo) => CompareTo((Fraction)fo);

//        /// <summary>
//        /// Returns an integer code from the comparison between the current fraction and d.
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns>Comparison int code.</returns>
//        public int CompareTo(double d) => CompareTo((Fraction)d);

//        /// <summary>
//        /// Returns an integer code from the comparison between the current fraction and d.
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns>Comparison int code.</returns>
//        public int CompareTo(decimal d) => CompareTo((Fraction)d);

//        /// <summary>
//        /// Returns an integer code from the comparison between the current fraction and ift.
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns>Comparison int code.</returns>
//        public int CompareTo(IntFloat ift) => ift.IsInt ? CompareTo(ift.Int) : CompareTo(ift.Float);

//        /// <summary>
//        /// Returns an integer code from the comparison between the current fraction and obj.
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns>Comparison int code.</returns>
//        public int CompareTo(object obj)
//        {
//            return obj switch
//            {
//                Fraction f => CompareTo(f),
//                FractionOperation fo => CompareTo(fo),
//                BigInteger bi => CompareTo(bi),
//                long l => CompareTo((BigInteger)l),
//                int i => CompareTo((BigInteger)i),
//                short s => CompareTo((BigInteger)s),
//                sbyte sb => CompareTo((BigInteger)sb),
//                UBigInteger ubi => CompareTo((BigInteger)ubi),
//                byte b => CompareTo((BigInteger)b),
//                ushort us => CompareTo((BigInteger)us),
//                uint ui => CompareTo((BigInteger)ui),
//                ulong ul => CompareTo((BigInteger)ul),
//                double d => CompareTo(d),
//                decimal d => CompareTo(d),
//                float f => CompareTo(f),
//                IntFloat ift => CompareTo(ift),
//                _ => throw new ArgumentException("Parameter must be a fraction, integer, double, float, IntFloat, or decimal."),
//            };
//        }
//        #endregion

//        #region Conversion
//        /// <summary>
//        /// Converts the current fraction to a BigInteger by floor-dividing it's numerator by it's denominator.
//        /// </summary>
//        /// <returns>The result of the conversion.</returns>
//        public BigInteger ToBigInteger() => Numerator / Denominator;

//        /// <summary>
//        /// Converts the current fraction to a BigInteger by floor-dividing it's numerator by it's denominator.
//        /// </summary>
//        /// <param name="remainder">The remainder left over from the floor-division.</param>
//        /// <returns>The result of the conversion.</returns>
//        public BigInteger ToBigInteger(out BigInteger remainder) => BigInteger.DivRem(Numerator, Denominator, out remainder);

//        /// <summary>
//        /// Converts the current fraction to a double by dividing it's numerator by it's denominator.
//        /// </summary>
//        /// <returns>The result of the conversion.</returns>
//        public double ToDouble() => (double)Numerator / (double)Denominator;

//        /// <summary>
//        /// Converts the current fraction to a float by dividing it's numerator by it's denominator.
//        /// </summary>
//        /// <returns>The result of the conversion.</returns>
//        public float ToFloat() => (float)Numerator / (float)Denominator;

//        /// <summary>
//        /// Converts the current fraction to a decimal by dividing it's numerator by it's denominator.
//        /// </summary>
//        /// <returns>The result of the conversion.</returns>
//        /// <exception cref="OverflowException"></exception>
//        public decimal ToDecimal() => (decimal)Numerator / (decimal)Denominator;

//        /// <summary>
//        /// Converts the current fraction to a Doudec by dividing it's numerator by it's denominator.
//        /// </summary>
//        /// <returns>The result of the conversion.</returns>
//        public Doudec ToDoudec() => (Doudec)Numerator / (Doudec)Denominator;

//        /// <summary>
//        /// Converts the current fraction to an IntFloat by dividing it's numerator by it's denominator.
//        /// </summary>
//        /// <returns>The result of the conversion.</returns>
//        public IntFloat ToIntFloat() => IsWhole ? (IntFloat)ToBigInteger() : ToDoudec();

//        #endregion
//        /// <summary>
//        /// Returns a string representation of the current fraction.
//        /// </summary>
//        /// <returns>A string representation of the current fraction.</returns>
//        public override string ToString()
//        {
//            return $"{Numerator}/{Denominator}";
//        }
//        #endregion

//        #region public static Methods
//        #region Math

//        /// <summary>
//        /// Returns two fractions representing a and b with the denominators equalized by cross multiplication.
//        /// </summary>
//        public static (Fraction a, Fraction b) EqualizeDenominators(Fraction a, Fraction b)
//        {
//            int a_sign = a.Sign, 
//                b_sign = b.Sign;

//            BigInteger
//                a_den_abs = BigInteger.Abs(a.Denominator),
//                b_den_abs = BigInteger.Abs(b.Denominator),
//                common_den_abs = a_den_abs * b_den_abs;

//            return (
//                new Fraction(BigInteger.Abs(a.Numerator) * b_den_abs * a_sign, common_den_abs),
//                new Fraction(BigInteger.Abs(b.Numerator) * a_den_abs * b_sign, common_den_abs)
//                );
//        }

//        /// <summary>
//        /// Returns the natural (base e) logarithm of the provided fraction.
//        /// </summary>
//        public static double Log(Fraction f) => Math.Log(f.ToDouble());

//        /// <summary>
//        /// Return the logarithm of the provided fraction by the provided new base.
//        /// </summary>
//        public static double Log(Fraction f, double newBase) => Math.Log(f.ToDouble(), newBase);

//        /// <summary>
//        /// Returns the base 10 logarithm of the provided fraction.
//        /// </summary>
//        public static double Log10(Fraction f) => Math.Log10(f.ToDouble());

//        /// <summary>
//        /// Returns the smallest of the two provided fractions.
//        /// </summary>
//        /// <returns> Either the smallest fraction or the first one (a) if they are equal.</returns>
//        public static Fraction Min(Fraction a, Fraction b) => a > b ? b : a;

//        /// <summary>
//        /// Returns the largest of the two provided fractions.
//        /// </summary>
//        /// <returns> Either the largest fraction or the first one (a) if they are equal.</returns>
//        public static Fraction Max(Fraction a, Fraction b) => a < b ? b : a;
//        public static Fraction Abs(Fraction f) => f.AbsoluteValue;

//        public static FractionOperation Pow(Fraction left, int right) => left.Power(right);
//        public static FractionOperation Pow(FractionOperation left, int right) => left.Unsimplified.Power(right);
//        public static double Pow(Fraction x, double y) => Math.Pow(x.ToDouble(), y);
//        public static double Pow(FractionOperation x, double y) => Math.Pow(x.Unsimplified.ToDouble(), y);
//        #endregion

//        #region Operators
//        public static FractionOperation operator -(Fraction f) => f.Negate();
//        public static FractionOperation operator +(Fraction f) => f;

//        public static FractionOperation operator +(Fraction left, Fraction right) => left.Add(right);
//        public static FractionOperation operator -(Fraction left, Fraction right) => left.Subtract(right);
//        public static FractionOperation operator *(Fraction left, Fraction right) => left.Multiply(right);
//        public static FractionOperation operator /(Fraction left, Fraction right) => left.Divide(right);
//        public static FractionOperation operator %(Fraction left, Fraction right) => left.Remainder(right);
//        public static Fraction operator ++(Fraction f) => f.Increment();
//        public static Fraction operator --(Fraction f) => f.Decrement();

//        public static bool operator <(Fraction left, Fraction right) => left.CompareTo(right) < 0;
//        public static bool operator >(Fraction left, Fraction right) => left.CompareTo(right) > 0;
//        public static bool operator <=(Fraction left, Fraction right) => left.CompareTo(right) <= 0;
//        public static bool operator >=(Fraction left, Fraction right) => left.CompareTo(right) >= 0;

//        public static bool operator ==(Fraction left, Fraction right) => left.SoftEquals(right);
//        public static bool operator !=(Fraction left, Fraction right) => !left.SoftEquals(right);
//        #endregion

//        #region Parse
//        public static bool TryParse(string s, out Fraction result)
//        {
//            string[] split = s.Split('/', 2);
//            if (split.Length == 2)
//            {
//                if (BigInteger.TryParse(split[0], out BigInteger numerator) && BigInteger.TryParse(split[1], out BigInteger denominator))
//                {
//                    result = new Fraction(numerator, denominator);
//                    return true;
//                }
//                else
//                {
//                    result = default;
//                    return false;
//                }
//            }
//            else if (split.Length == 1)
//            {
//                if (BigInteger.TryParse(split[0], out BigInteger numerator))
//                {
//                    result = new Fraction(numerator);
//                    return true;
//                }
//                else
//                {
//                    result = default;
//                    return false;
//                }
//            }
//            else // *assume 0
//            {
//                result = default;
//                return false;
//            }
//        }

//        public static Fraction Parse(string s)
//        {
//            if (TryParse(s, out Fraction result))
//            {
//                return result;
//            }
//            else
//            {
//                throw new FormatException("String is not a valid fraction.");
//            }
//        }
//        #endregion

//        #region Conversion
//        public static Fraction FromDoudec(Doudec dd) => dd.Value switch
//        {
//            double d => FromDouble(d),
//            decimal dec => FromDecimal(dec),
//            _ => FromDouble(dd.Double)
//        };

//        public static Fraction FromDouble(double d)
//        {
//            if (double.IsInfinity(d)) throw new OverflowException("Fraction cannot represent infinity.");

//            // Trying to convert to decimal first...
//            if (MathUtils.TryToDecimalStrictly(d, out decimal dec))
//            {
//                return FromDecimal(dec);
//            }

//            // Can't use decimal, will have to fall back on double instead.
//            double whole = Math.Truncate(d);
//            double numerator = d - whole;

//            #region Copied to FromDecimal(decimal d) with mods
//            int i = 0;
//            while (i < FROM_DOUBLE_PRECISION_LIMIT && numerator % 1 != 0)
//            {
//                numerator *= 10;
//                ++i;
//            }


//            return new Fraction((BigInteger)whole, 1) + new Fraction((BigInteger)numerator, BigInteger.Pow(10, i));
//            #endregion
//        }
//        public static Fraction FromDecimal(decimal d)
//        {
//            decimal whole = Math.Truncate(d);
//            decimal numerator = d - whole;

//            #region Copied from FromDouble(double d) with mods
//            int i = 0;
//            while (i < FROM_DECIMAL_PRECISION_LIMIT && numerator % 1 != 0)
//            {
//                numerator *= 10;
//                ++i;
//            }


//            return new Fraction((BigInteger)whole, 1) + new Fraction((BigInteger)numerator, BigInteger.Pow(10, i));
//            #endregion
//        }
//        #endregion

//        #region Casts
//        #region from
//        // int -> fraction
//        public static implicit operator Fraction(sbyte i) => new Fraction(i);
//        public static implicit operator Fraction(short i) => new Fraction(i);
//        public static implicit operator Fraction(int i) => new Fraction(i);
//        public static implicit operator Fraction(long i) => new Fraction(i);
//        public static implicit operator Fraction(BigInteger i) => new Fraction(i);

//        public static implicit operator Fraction(byte i) => new Fraction(i);
//        public static implicit operator Fraction(ushort i) => new Fraction(i);
//        public static implicit operator Fraction(uint i) => new Fraction(i);
//        public static implicit operator Fraction(ulong i) => new Fraction(i);
//        public static implicit operator Fraction(UBigInteger i) => new Fraction(i);

//        // floating point -> fraction
//        public static explicit operator Fraction(float f) => Fraction.FromDouble(f);
//        public static explicit operator Fraction(double f) => Fraction.FromDouble(f);

//        // decimal -> fraction
//        public static explicit operator Fraction(decimal f) => Fraction.FromDecimal(f);

//        // Doudec -> fraction
//        public static explicit operator Fraction(Doudec f) => Fraction.FromDoudec(f);
//        #endregion

//        #region to
//        // fraction -> int
//        public static explicit operator sbyte(Fraction f) => (sbyte)f.ToBigInteger();
//        public static explicit operator short(Fraction f) => (short)f.ToBigInteger();
//        public static explicit operator int(Fraction f) => (int)f.ToBigInteger();
//        public static explicit operator long(Fraction f) => (long)f.ToBigInteger();
//        public static explicit operator BigInteger(Fraction f) => f.ToBigInteger();

//        public static explicit operator byte(Fraction f) => (byte)f.ToBigInteger();
//        public static explicit operator ushort(Fraction f) => (ushort)f.ToBigInteger();
//        public static explicit operator uint(Fraction f) => (uint)f.ToBigInteger();
//        public static explicit operator ulong(Fraction f) => (ulong)f.ToBigInteger();
//        public static explicit operator UBigInteger(Fraction f) => (UBigInteger)f.ToBigInteger();

//        // fraction -> floating point
//        public static explicit operator float(Fraction f) => f.ToFloat();
//        public static explicit operator double(Fraction f) => f.ToDouble();

//        // fraction -> decimal
//        public static explicit operator decimal(Fraction f) => f.ToDecimal();

//        // fraction -> Doudec
//        public static explicit operator Doudec(Fraction f) => f.ToDoudec();
//        #endregion
//        #endregion
//        #endregion

//        #region public static readonly Fields
//        public static readonly Fraction Zero = new Fraction(0, 1);
//        public static readonly Fraction Undefined = new Fraction(0, 0);
//        #endregion

//        #region private const Fields
//        private const int FROM_DOUBLE_PRECISION_LIMIT = 326;
//        private const int FROM_DECIMAL_PRECISION_LIMIT = 31;
//        #endregion
//    }

//    /// <summary>
//    /// Doesn't automatically simplify until it is cast to a Fraction.
//    /// </summary>
//    public readonly struct FractionOperation : IComparable<Fraction>, IComparable<FractionOperation>, IEquatable<FractionOperation>, IEquatable<Fraction>, IComparable
//    {
//        #region (passthrough) public Properties
//        public BigInteger Numerator => Unsimplified.Numerator;
//        public BigInteger Denominator => Unsimplified.Denominator;
//        public UBigInteger UNumerator => Unsimplified.UNumerator;
//        public BigInteger UDenominator => Unsimplified.UDenominator;
//        public bool IsNegative => Unsimplified.IsNegative;
//        public bool IsZero => Unsimplified.IsZero;
//        public bool IsUndefined => Unsimplified.IsUndefined;
//        public BigInteger Sign => Unsimplified.Sign;
//        #endregion

//        #region public readonly Fields
//        public readonly Fraction Unsimplified;
//        #endregion

//        #region public Constructors
//        public FractionOperation(Fraction unsimplified)
//        {
//            Unsimplified = unsimplified;
//        }

//        public FractionOperation(BigInteger numerator, BigInteger denominator) : this(new Fraction(numerator, denominator)) { }
//        public FractionOperation(UBigInteger numerator, UBigInteger denominator, bool isNegative) : this(new Fraction(numerator, denominator, isNegative)) { }
//        public FractionOperation(BigInteger numerator) : this(new Fraction(numerator)) { }
//        #endregion

//        #region public Methods
//        public override string ToString()
//        {
//            return Unsimplified.Simplify().ToString();
//        }

//        #region Comparison
//        public int CompareTo(Fraction f) => Unsimplified.CompareTo(f);
//        public int CompareTo(FractionOperation other) => Unsimplified.CompareTo(other);
//        public int CompareTo(object obj) => Unsimplified.CompareTo(obj);

//        public bool Equals(Fraction other) => Unsimplified.Equals(other);
//        public bool Equals(FractionOperation other) => Unsimplified.Equals(other.Unsimplified);
//        public override bool Equals(object obj) => Unsimplified.Equals(obj);
//        public override int GetHashCode() => Unsimplified.GetHashCode();
//        #endregion

//        #region Math
//        public FractionOperation Floor() => Unsimplified.Floor();
//        public FractionOperation Ceiling() => Unsimplified.Ceiling();
//        public FractionOperation Truncate() => Unsimplified.Truncate();
//        #endregion
//        #endregion

//        #region public static Methods
//        #region Operators
//        public static FractionOperation operator -(FractionOperation fo) => -fo.Unsimplified;
//        public static FractionOperation operator +(FractionOperation fo) => +fo.Unsimplified;

//        public static FractionOperation operator +(FractionOperation left, Fraction right) => left.Unsimplified + right;
//        public static FractionOperation operator +(Fraction left, FractionOperation right) => left + right.Unsimplified;
//        public static FractionOperation operator +(FractionOperation left, FractionOperation right) => left.Unsimplified + right.Unsimplified;

//        public static FractionOperation operator -(FractionOperation left, Fraction right) => left.Unsimplified - right;
//        public static FractionOperation operator -(Fraction left, FractionOperation right) => left - right.Unsimplified;
//        public static FractionOperation operator -(FractionOperation left, FractionOperation right) => left.Unsimplified - right.Unsimplified;

//        public static FractionOperation operator *(FractionOperation left, Fraction right) => left.Unsimplified * right;
//        public static FractionOperation operator *(Fraction left, FractionOperation right) => left * right.Unsimplified;
//        public static FractionOperation operator *(FractionOperation left, FractionOperation right) => left.Unsimplified * right.Unsimplified;

//        public static FractionOperation operator /(FractionOperation left, Fraction right) => left.Unsimplified / right;
//        public static FractionOperation operator /(Fraction left, FractionOperation right) => left / right.Unsimplified;
//        public static FractionOperation operator /(FractionOperation left, FractionOperation right) => left.Unsimplified / right.Unsimplified;

//        public static FractionOperation operator %(FractionOperation left, Fraction right) => left.Unsimplified % right;
//        public static FractionOperation operator %(Fraction left, FractionOperation right) => left % right.Unsimplified;
//        public static FractionOperation operator %(FractionOperation left, FractionOperation right) => left.Unsimplified % right.Unsimplified;

//        public static FractionOperation operator ++(FractionOperation fo) => fo.Unsimplified.Increment();
//        public static FractionOperation operator --(FractionOperation fo) => fo.Unsimplified.Decrement();

//        public static bool operator <(FractionOperation left, FractionOperation right) => left.CompareTo(right) < 0;
//        public static bool operator <(Fraction left, FractionOperation right) => left.CompareTo(right) < 0;
//        public static bool operator <(FractionOperation left, Fraction right) => left.CompareTo(right) < 0;

//        public static bool operator >(FractionOperation left, FractionOperation right) => left.CompareTo(right) > 0;
//        public static bool operator >(Fraction left, FractionOperation right) => left.CompareTo(right) > 0;
//        public static bool operator >(FractionOperation left, Fraction right) => left.CompareTo(right) > 0;

//        public static bool operator <=(FractionOperation left, FractionOperation right) => left.CompareTo(right) <= 0;
//        public static bool operator <=(Fraction left, FractionOperation right) => left.CompareTo(right) <= 0;
//        public static bool operator <=(FractionOperation left, Fraction right) => left.CompareTo(right) <= 0;

//        public static bool operator >=(FractionOperation left, FractionOperation right) => left.CompareTo(right) >= 0;
//        public static bool operator >=(Fraction left, FractionOperation right) => left.CompareTo(right) >= 0;
//        public static bool operator >=(FractionOperation left, Fraction right) => left.CompareTo(right) >= 0;

//        public static bool operator ==(FractionOperation left, FractionOperation right) => left.Equals(right);
//        public static bool operator ==(Fraction left, FractionOperation right) => left.Equals(right);
//        public static bool operator ==(FractionOperation left, Fraction right) => left.Equals(right);

//        public static bool operator !=(FractionOperation left, FractionOperation right) => !left.Equals(right);
//        public static bool operator !=(Fraction left, FractionOperation right) => !left.Equals(right);
//        public static bool operator !=(FractionOperation left, Fraction right) => !left.Equals(right);
//        #endregion

//        #region Casts
//        #region to
//        // FractionOperation -> Fraction
//        public static implicit operator Fraction(FractionOperation fo) => fo.Unsimplified.Simplify();

//        // FractionOperation -> floating point
//        public static explicit operator float(FractionOperation fo) => (float)fo.Unsimplified;
//        public static explicit operator double(FractionOperation fo) => (double)fo.Unsimplified;
//        public static explicit operator decimal(FractionOperation fo) => (decimal)fo.Unsimplified;
//        public static explicit operator Doudec(FractionOperation fo) => (Doudec)fo.Unsimplified;

//        // FracitonOperation -> integer
//        public static explicit operator BigInteger(FractionOperation fo) => (BigInteger)fo.Unsimplified;
//        public static explicit operator long(FractionOperation fo) => (long)fo.Unsimplified;
//        public static explicit operator int(FractionOperation fo) => (int)fo.Unsimplified;
//        public static explicit operator short(FractionOperation fo) => (short)fo.Unsimplified;
//        public static explicit operator sbyte(FractionOperation fo) => (sbyte)fo.Unsimplified;

//        public static explicit operator UBigInteger(FractionOperation fo) => (UBigInteger)fo.Unsimplified;
//        public static explicit operator ulong(FractionOperation fo) => (ulong)fo.Unsimplified;
//        public static explicit operator uint(FractionOperation fo) => (uint)fo.Unsimplified;
//        public static explicit operator ushort(FractionOperation fo) => (ushort)fo.Unsimplified;
//        public static explicit operator byte(FractionOperation fo) => (byte)fo.Unsimplified;
//        #endregion

//        #region from
//        // Fraction -> FractionOperation
//        public static implicit operator FractionOperation(Fraction f) => new FractionOperation(f);

//        //// floating point -> FractionOperation
//        //public static explicit operator FractionOperation(float f) => new FractionOperation((Fraction)f);
//        //public static explicit operator FractionOperation(double f) => new FractionOperation((Fraction)f);
//        //public static explicit operator FractionOperation(decimal f) => new FractionOperation((Fraction)f);
//        //public static explicit operator FractionOperation(Doudec f) => new FractionOperation((Fraction)f);

//        //// integer -> FractionOperation
//        //public static implicit operator FractionOperation(BigInteger i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(long i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(int i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(short i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(sbyte i) => new FractionOperation((Fraction) i);

//        //public static implicit operator FractionOperation(UBigInteger i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(ulong i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(uint i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(ushort i) => new FractionOperation((Fraction) i);
//        //public static implicit operator FractionOperation(byte i) => new FractionOperation((Fraction) i);
//        #endregion
//        #endregion
//        #endregion
//    }

//    /// <summary>
//    /// Extension methods for Fraction.
//    /// </summary>
//    public static class FractionExtensions
//    {
//        public static Fraction NextFraction(this Random rand)
//        {
//            return new Fraction(rand.Next(int.MinValue, int.MaxValue), rand.Next(int.MinValue, int.MaxValue));
//        }

//        public static Fraction NextFraction(this Random rand, int minNumerator, int maxNumerator, int minDenominator, int maxDenominator)
//        {
//            return new Fraction(rand.Next(minNumerator, maxNumerator), rand.Next(minDenominator, maxDenominator));
//        }

//        public static Fraction Sum(this IEnumerable<Fraction> items)
//        {
//            Fraction total = new Fraction(0);
//            int i = 0;

//            foreach (Fraction f in items)
//            {
//                total = total.Add(f);
//                i++;

//                if (i >= 8)
//                {
//                    i = 0;
//                    total = total.Simplify();
//                }
//            }

//            return total.Simplify();
//        }
//    }
//}