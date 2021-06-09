using System;
using System.Collections.Generic;
using System.Numerics;

namespace JesseRussell.Numerics
{
    public readonly struct Fraction
    {
        /// <summary>
        /// Dividend (top number)
        /// </summary>
        public readonly BigInteger Numerator;
        /// <summary>
        /// Divisor (bottom number)
        /// </summary>
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

        /// <summary>
        /// Fraction that equals 0.
        /// </summary>
        public static readonly Fraction Zero = new Fraction(0, 1);

        /// <summary>
        /// Fraction with a numerator equal to 0.
        /// </summary>
        public static readonly Fraction Undefined = new Fraction(0, 0);

        /// <summary>
        /// Returns whether the calling fraction is negative or not.
        /// </summary>
        public bool IsNegative => Numerator.Sign * Denominator.Sign < 0;

        /// <summary>
        /// Returns whether the calling fraction is equal to zero or not.
        /// </summary>
        public bool IsZero => Numerator.IsZero && !Denominator.IsZero;

        /// <summary>
        /// Returns whether the calling fraction's numerator is equal to 0.
        /// </summary>
        public bool IsUndefined => Denominator.IsZero;

        /// <summary>
        /// Returns whether the calling fraction's denominator cleanly divides into the numerator with no remainder.
        /// </summary>
        public bool IsWhole => Numerator % Denominator == 0;

        /// <summary>
        /// Returns an integer representing the fraction's sign.
        /// </summary>
        /// <returns>
        /// (-1): calling fraction is negative
        /// (0): calling fraction is equal to 0, or is undefined
        /// (1): calling fraction is positive
        /// </returns>
        public int Sign => Numerator.Sign * Denominator.Sign;

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

        /// <summary>
        /// Returns the given fractions modified to make the denominators equal. Example: 1/2, 1/3 -> 3/6, 2/6
        /// </summary>
        public static (Fraction a, Fraction b) EqualizeDenominators(Fraction a, Fraction b)
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

        /// <summary>
        /// Returns the unsimplified sum of the calling fraction and the given fraction.
        /// </summary>
        public Fraction Add(Fraction other) => SimplifySign().Add_helper(other.SimplifySign());
        private Fraction Add_helper(Fraction other)
        {
            if (Denominator == other.Denominator)
                return new Fraction(Numerator + other.Numerator, Denominator);
            return new Fraction(Numerator * other.Denominator + other.Numerator * Denominator, Denominator * other.Denominator);
        }

        /// <summary>
        /// Returns the unsimplified difference of between the calling fraction and the given fraction.
        /// </summary>
        public Fraction Subtract(Fraction other) => SimplifySign().Subtract_helper(other.SimplifySign());
        private Fraction Subtract_helper(Fraction other)
        {
            if (Denominator == other.Denominator)
                return new Fraction(Numerator - other.Numerator, Denominator);
            return new Fraction(Numerator * other.Denominator - other.Numerator * Denominator, Denominator * other.Denominator);
        }

        /// <summary>
        /// Returns the unsimplified product of the calling fraction and the given fraction.
        /// </summary>
        public Fraction Multiply(Fraction other) => new Fraction(Numerator * other.Numerator, Denominator * other.Denominator);

        /// <summary>
        /// Returns the unsimplified quotient of the calling fraction divided by the given fraction.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Fraction Divide(Fraction other) => new Fraction(Numerator * other.Denominator, Denominator * other.Numerator);

        /// <summary>
        /// Returns the unsimplified remainder of the division of the calling fraction and other.
        /// </summary>
        /// <param name="other">The Fraction that the calling fraction is being divided by.</param>
        /// <returns>The remainder from the division.</returns>
        public Fraction Remainder(Fraction other) => Subtract(Divide(other).Truncate().Multiply(other));

        /// <summary>
        /// Returns the calling fraction negated (multiplied by -1).
        /// </summary>
        public Fraction Negate() => new Fraction(BigInteger.Negate(Numerator), Denominator);

        /// <summary>
        /// Returns the unsimplified exponentiation of the calling fraction raised to the given integer.
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
        /// Returns the mediant (numerator + other.numerator / denominator + other.denominator) with the given fraction. The mediant will always be between the two fractions.
        /// </summary>
        public Fraction Mediant(Fraction other) => new Fraction(Numerator + other.Numerator, Denominator + other.Denominator);

        /// <summary>
        /// Returns the calling fraction plus 1.
        /// </summary>
        public Fraction Increment() => new Fraction(Numerator + Denominator, Denominator);

        /// <summary>
        /// Returns the calling fraction minus 1.
        /// </summary>
        public Fraction Decrement() => new Fraction(Numerator - Denominator, Denominator);

        /// <summary>
        /// Returns the whole portion of the calling fraction. Example: 5/2 equals 2 + 1/2, so the Truncation of 5/2 would return 2.
        /// </summary>
        public Fraction Truncate() => new Fraction(ToBigInteger(), 1);

        /// <summary>
        /// Returns the calling fractions rounded to the next lowest integer. Example: 5/2 equals 2 + 1/2, so the floor of 5/2 would return 2. Note: the floor of -5/2 is -3 not -2. If truncation is desired, use Truncate.
        /// </summary>
        public Fraction Floor()
        {
            if (IsWhole) return this;
            else if (IsNegative) return new Fraction(ToBigInteger() - 1, 1);
            else return new Fraction(ToBigInteger(), 1);
        }

        /// <summary>
        /// Returns the calling fraction rounded to the next highest integer. Example: 5/2 equals 2 + 1/2, so the ceiling of 5/2 is 3. Note: the ceiling of -5/2 is -2 not -3.
        /// </summary>
        public Fraction Ceiling()
        {
            if (IsWhole) return this;
            else if (IsNegative) return new Fraction(ToBigInteger(), 1);
            else return new Fraction(ToBigInteger() + 1, 1);
        }

        /// <summary>
        /// Returns the smaller of the two fractions. If equal, fraction a is returned.
        /// </summary>
        public static Fraction Min(Fraction a, Fraction b) => b < a ? b : a;
        /// <summary>
        /// Returns the larger of the two fractions. If equal, fraction a is returned.
        /// </summary>
        public static Fraction Max(Fraction a, Fraction b) => b > a ? b : a;

        /// <summary>
        /// Returns the smaller of the calling fraction and the given fraction. If equal, the calling fraction is returned.
        /// </summary>
        /// 
        public Fraction Min(Fraction other) => Min(this, other);
        /// <summary>
        /// Returns the larger of the calling fraction and the given fraction. If equal, the calling fraction is returned.
        /// </summary>
        public Fraction Max(Fraction other) => Max(this, other);
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
        /// <summary>
        /// Returns the calling fraction converted to BigInteger, truncating the non-whole component.
        /// </summary>
        public BigInteger ToBigInteger() => Numerator / Denominator;
        /// <summary>
        /// Returns the calling fraction converted to a double.
        /// </summary>
        public double ToDouble() => (double)Numerator / (double)Denominator;
        /// <summary>
        /// Returns the calling fraction converted to a float.
        /// </summary>
        public float ToFloat() => (float)Numerator / (float)Denominator;
        /// <summary>
        /// Returns the calling fraction converted to a decimal.
        /// </summary>
        public decimal ToDecimal() => (decimal)Numerator / (decimal)Denominator;
        /// <summary>
        /// Returns the calling fraction converted to a doudec (See JesseRussell.Numerics.Doudec).
        /// </summary>
        public Doudec ToDoudec() => (Doudec)Numerator / (Doudec)Denominator;

        /// <summary>
        /// Returns the given double converted to a fraction.
        /// </summary>
        public static Fraction FromDouble(double d)
        {
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

        /// <summary>
        /// Returns the given float converted to a fraction.
        /// </summary>
        public static Fraction FromFloat(float f) => FromDouble(f);

        /// <summary>
        /// Returns the given decimal converted to a fraction.
        /// </summary>
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
        /// <summary>
        /// Returns the given doudec converted to a fraction (see JesseRussell.Numerics.Doudec).
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Fraction FromDoudec(Doudec d) => d.IsDecimal ? FromDecimal(d.Decimal) : FromDouble(d.Double);

        // o=============o
        // | Comparison: |
        // o=============o
        #region comparison
        /// <summary>
        /// Returns an integer representing a comparison between the calling fraction and other.
        /// </summary>
        /// <returns>
        /// (-1): calling &lt; other,
        /// (0): calling == other,
        /// (1): calling &gt; other
        /// </returns>
        public int CompareTo(Fraction other)
        {
            (Fraction a, Fraction b) = EqualizeDenominators(this, other);

            return a.Numerator.CompareTo(b.Numerator);
        }

        /// <summary>
        /// Returns whether the calling fraction's Numerator and Denominator are exactly equal to the given fraction's Numerator and Denominator without simplifying either first. Example: 1/2 hard-equals 1/2, but 1/2 does not hard-equal 2/4 despite them having the same simplified value.
        /// </summary>
        public bool HardEquals(Fraction other) => Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);
        /// <summary>
        /// Returns whether the calling fraction is equal to the given fraction by comparing the simplification of both fractions. Example: 1/2 equals 1/2 and 2/4 equals 1/2.
        /// </summary>
        public bool Equals(Fraction other) => Simplify().HardEquals(other.Simplify());

        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(BigInteger other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(long other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(int other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(short other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(sbyte other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(UBigInteger other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(ulong other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(uint other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(ushort other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Returns whether the calling fraction equals the given integer.
        /// </summary>
        public bool Equals(byte other)
        {
            Fraction simpl = Simplify();
            return simpl.Denominator == 1 && simpl.Numerator == other;
        }
        /// <summary>
        /// Tries to determine if the calling fraction is equal to the given object.
        /// </summary>
        /// <returns>
        /// (true): calling fraction equals the given object, (false): calling fraction does not equal the given object, OR the given object is of a type that cannot be compared to a fraction.
        /// </returns>
        public override bool Equals(object other) => TryEquals(other) ?? false;

        /// <summary>
        /// Tries to determine if the calling fraction is equal to the given object.
        /// </summary>
        /// <returns>
        /// (true): calling fraction equals the given object, (false): calling fraction does not equal the given object, (null): the given object is of a type that cannot be compared to a fraction
        /// </returns>
        public bool? TryEquals(object other)
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
                _ => null,
            };
        }

        /// <summary>
        /// Returns a hash code representing the simplified value of the calling fraction.
        /// If two hash codes are equal the fractions they represent are most-likely equal but are not guarantied to be equal;
        /// however, if two hash codes are unequal the fraction's they represent are guarantied to be unequal.
        /// </summary>
        public override int GetHashCode()
        {
            Fraction simp = Simplify();
            return HashCode.Combine(simp.Numerator, simp.Denominator);
        }
        #endregion

        // parsing:
        /// <summary>
        /// Tries to parse the given string to a fraction. A fraction is encoded as: "{NUMERATOR}/{DENOMINATOR}".
        /// </summary>
        /// <param name="s">the string to be parsed</param>
        /// <param name="result">The result of the parse if the parse succeeds.
        /// The fraction passed in can be declared in-line with the method call like this:
        /// Fraction.TryParse("1/2", out Fraction f)</param>
        /// <returns>(true): the parse was successful, (false): the parse was unsuccessful, the given string contains a syntax error</returns>
        public static bool TryParse(string s, out Fraction result)
        {
            // split the string into a max of 2 tokens.
            string[] split = s.Split('/', 2);
            if (split.Length == 2)
            {
                // Try to parse the 2 tokens as BigInteger, if both are successful, return a successfully parsed fraction.
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
                //try to parse the single token as a BigInteger. If successful, return a successfully parsed whole fraction (with denominator = 1)
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
            else // *assume split.length 0 because 2 is the max.
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Returns the given string parsed to a fraction. A fraction is encoded as: "{NUMERATOR}/{DENOMINATOR}".
        /// </summary>
        /// <param name="s">the string to parse</param>
        public static Fraction Parse(string s)
        {
            if (TryParse(s, out Fraction result))
                return result;
            else
                throw new FormatException("The string is not a valid fraction.");
        }

        /// <summary>
        /// Returns a string encoding of the calling fraction. A fraction is encoded as: "{NUMERATOR}/{DENOMINATOR}".
        /// </summary>
        public override string ToString() => $"{Numerator}/{Denominator}";
    }



    /// <summary>
    /// Fraction wrapper returned by operators such as: +, -, *, or /.
    /// Allows multiple operators to be strung together which return a simplified fraction without simplifying between every operator.
    /// </summary>
    public readonly struct FractionOperation
    {
        /// <summary>
        /// The in-progress fraction without simplification.
        /// </summary>
        public readonly Fraction Unsimplified;

        /// <summary>
        /// Returns the wrapped fraction simplified.
        /// </summary>
        public Fraction Simplify() => Unsimplified.Simplify();

        public FractionOperation(Fraction unsimplified) => Unsimplified = unsimplified;

        // o----------o
        // | casting: |
        // o----------o

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

        // o------------o
        // | operators: |
        // o------------o
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

        // o--------------------------o
        // | object override methods: |
        // o--------------------------o

        /// <summary>
        /// Returns a string encoding of the wrapped fraction. A fraction is encoded as: "{NUMERATOR}/{DENOMINATOR}".
        /// </summary>
        public override string ToString() => Unsimplified.Simplify().ToString();

        /// <summary>
        /// Tries to determine if the wrapped fraction is equal to the given object.
        /// </summary>
        /// <returns>
        /// (true): wrapped fraction equals the given object, (false): wrapped fraction does not equal the given object, OR the given object is of a type that cannot be compared to a fraction.
        /// </returns>
        public override bool Equals(object obj) => Unsimplified.Equals(obj);
        public override int GetHashCode() => Unsimplified.GetHashCode();
    }


    /// <summary>
    /// Extension methods for Fraction.
    /// </summary>
    public static class FractionExtensions
    {
        /// <summary>
        /// Returns a random fraction in which:
        /// the numerator ranges from int.MinValue to int.MaxValue, and the denominator ranges from int.MinValue to int.MaxValue.
        /// </summary>
        public static Fraction NextFraction(this Random rand)
        {
            return new Fraction(rand.Next(int.MinValue, int.MaxValue), rand.Next(int.MinValue, int.MaxValue));
        }
        /// <summary>
        /// Returns a random fraction with the given constrants.
        /// </summary>
        public static Fraction NextFraction(this Random rand, int minNumerator, int maxNumerator, int minDenominator, int maxDenominator)
        {
            return new Fraction(rand.Next(minNumerator, maxNumerator), rand.Next(minDenominator, maxDenominator));
        }
        /// <summary>
        /// Returns a random fraction ranging from the given min to the given max. Due to floating point rounding errors, perfect accuracy is not guarantied.
        /// </summary>
        public static Fraction NextFraction(this Random rand, double min, double max)
        {
            return Fraction.FromDouble(rand.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Returns the total value of all the fractions in the calling enumerable.
        /// </summary>
        public static Fraction Sum(this IEnumerable<Fraction> items)
        {
            // TODO: multithreading?

            // initial zero
            Fraction total = Fraction.Zero;
            int i = 0;

            // main loop
            foreach (Fraction f in items)
            {
                // add the next fraction to total.
                total = total.Add(f);

                // increment i
                i++;
                
                // when i hits 8, simplify the total and reset i to 0.
                if (i >= 8)
                {
                    i = 0;
                    total = total.Simplify();
                }
            }

            // return the result simplified.
            return total.Simplify();
        }
    }
}

