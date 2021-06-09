using System;
using System.Numerics;

namespace JesseRussell.Numerics
{
    /// <summary>
    /// Unsigned BigInteger. Not really unsigned of course; just artificially forced to be positive; effectively making it unsigned for most purposes. However, unlike other unsigned types, it's overflow behavior (or rather underflow) is to become it's own absolute value instead of rolling over to max value, since BigInteger does not have a max value.
    /// </summary>
    public readonly struct UBigInteger : IComparable<UBigInteger>, IComparable<BigInteger>, IEquatable<UBigInteger>, IEquatable<BigInteger>, IFormattable
    {
        /// <summary>
        /// The wrapped BigInteger value.
        /// </summary>
        public readonly BigInteger Value;

        /// <summary>
        /// Whether the calling UBigInteger is even or not.
        /// </summary>
        public bool IsEven { get => Value.IsEven; }

        /// <summary>
        /// Whether the calling UBigInteger equals 0.
        /// </summary>
        public bool IsZero { get => Value.IsZero; }

        /// <summary>
        /// Whether the calling UBigInteger equals 1.
        /// </summary>
        public bool IsOne { get => Value.IsOne; }

        /// <summary>
        /// Whether the calling UBigInteger is divisible by 2.
        /// </summary>
        public bool IsPowerOfTwo { get => Value.IsPowerOfTwo; }

        /// <summary>
        /// Returns an integer representing the calling UbigInteger's sign.
        /// </summary>
        /// <returns>
        /// (-1): calling UBigInteger is negative.
        /// (0): calling UBigInteger equals 0.
        /// (1): calling UBigInteger is positive.
        /// </returns>
        public int Sign { get => Value.Sign; }

        // o---------------o
        // | Constructors: |
        // o---------------o
        public UBigInteger(BigInteger value)
        {
            Value = BigInteger.Abs(value);
        }

        public UBigInteger(ulong ul)
        {
            // No need for abs.
            Value = new BigInteger(ul);
        }

        public UBigInteger(uint ui)
        {
            // No need for abs.
            Value = new BigInteger(ui);
        }

        public UBigInteger(ushort us)
        {
            // No need for abs.
            Value = new BigInteger(us);
        }

        public UBigInteger(byte b)
        {
            // No need for abs (Byte is unsigned).
            Value = new BigInteger(b);
        }

        public UBigInteger(long l)
        {
            Value = new BigInteger(Math.Abs(l));
        }

        public UBigInteger(int i)
        {
            Value = new BigInteger(Math.Abs(i));
        }

        public UBigInteger(short s)
        {
            Value = new BigInteger(Math.Abs(s));
        }

        public UBigInteger(sbyte sb)
        {
            Value = new BigInteger(Math.Abs(sb));
        }
        #region Extension Constructors. These Constructors MUST extend another constructor to keep the UBigInteger unsigned.
        public UBigInteger(float f) : this(new BigInteger(f)) { }
        public UBigInteger(double d) : this(new BigInteger(d)) { }
        public UBigInteger(decimal dec) : this(new BigInteger(dec)) { }
        #endregion

        // o------------------------------------------------o
        // | Typical object and interface override methods: |
        // o------------------------------------------------o
        /// <summary>
        /// Returns a string encoding of the calling UBigInteger following the given format.
        /// </summary>
        public string ToString(string format) => Value.ToString(format);

        /// <summary>
        /// Returns a string encoding of the calling UBigInteger following the given culture-specific formating information.
        /// </summary>
        public string ToString(IFormatProvider provider) => Value.ToString(provider);

        /// <summary>
        /// Returns a string encoding of the calling UBigInteger following the given format and the given culture-specific formating information.
        /// </summary>
        public string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);

        /// <summary>
        /// Returns a string encoding of the calling UBigInteger
        /// </summary>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// Returns the hash code for the calling UBigInteger. This is equal to the hash code of the wrapped BigInteger value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Tries to determine if the calling UBigInteger is equal to the given object
        /// </summary>
        /// <returns>true: if the given object is a UBigInteger or a BigInteger and its value is equal to the value of the wrapped BigInteger value; otherwise, false</returns>
        public override bool Equals(object obj) => obj is UBigInteger ubig ? Value.Equals(ubig.Value) : Value.Equals(obj);


        /// <summary>
        /// Returns an integer representing a comparison between the calling UBigInteger and the given UBigInteger.
        /// </summary>
        /// <returns>
        /// (-1): calling UBigInteger &lt; given UBigInteger
        /// (0): calling UBigInteger == given UBigInteger
        /// (1): calling UBigInteger &gt; given UBigInteger
        /// </returns>
        public int CompareTo(UBigInteger other) => Value.CompareTo(other.Value);

        /// <summary>
        /// Returns an integer representing a comparison between the calling UBigInteger and the given BigInteger.
        /// </summary>
        /// <returns>
        /// (-1): calling UBigInteger &lt; given BigInteger
        /// (0): calling UBigInteger == given BigInteger
        /// (1): calling UBigInteger &gt; given BigInteger
        /// </returns>
        public int CompareTo(BigInteger other) => Value.CompareTo(other);

        /// <summary>
        /// Returns whether the calling UBigInteger equals the given UBigInteger.
        /// </summary>
        public bool Equals(UBigInteger other) => Value.Equals(other.Value);

        /// <summary>
        /// Returns whether the calling UBigInteger equals the given BigInteger.
        /// </summary>
        public bool Equals(BigInteger other) => Value.Equals(other);



        // o----------o
        // | Parsing: |
        // o----------o
        /// <summary>
        /// Parses the given string to a UBigInteger. Throws a FormatException if the encoded value is negative.
        /// </summary>
        public static UBigInteger Parse(string s)
        {
            BigInteger value = BigInteger.Parse(s);
            if (value < 0)
            {
                throw new FormatException($"UBigInteger cannot be negative.");
            }
            return new UBigInteger(value);
        }

        /// <summary>
        /// Tries to parse the given string to a UBigInteger.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <param name="result">The result if the parse was successful</param>
        /// <returns>true: if the parse was successful, false otherwise</returns>
        public static bool TryParse(string s, out UBigInteger result)
        {
            if (BigInteger.TryParse(s, out BigInteger big) && big >= 0)
            {
                result = new UBigInteger(big);
                return true;
            }
            else
            {
                result = new UBigInteger(big);
                return false;
            }
        }

        // o----------o
        // | casting: |
        // o----------o

        // from:
        public static explicit operator UBigInteger(BigInteger big) => new UBigInteger(big);

        public static implicit operator UBigInteger(ulong ul) => new UBigInteger(ul);
        public static implicit operator UBigInteger(uint ui) => new UBigInteger(ui);
        public static implicit operator UBigInteger(ushort ui) => new UBigInteger(ui);
        public static implicit operator UBigInteger(byte b) => new UBigInteger(b);

        public static explicit operator UBigInteger(long l) => new UBigInteger(l);
        public static explicit operator UBigInteger(int i) => new UBigInteger(i);
        public static explicit operator UBigInteger(short s) => new UBigInteger(s);
        public static explicit operator UBigInteger(sbyte sb) => new UBigInteger(sb);

        public static explicit operator UBigInteger(float f) => new UBigInteger(f);
        public static explicit operator UBigInteger(double d) => new UBigInteger(d);
        public static explicit operator UBigInteger(decimal dec) => new UBigInteger(dec);

        // to:
        public static explicit operator ulong(UBigInteger ubig) => (ulong)ubig.Value;
        public static explicit operator uint(UBigInteger ubig) => (uint)ubig.Value;
        public static explicit operator ushort(UBigInteger ubig) => (ushort)ubig.Value;
        public static explicit operator byte(UBigInteger ubig) => (byte)ubig.Value;

        public static implicit operator BigInteger(UBigInteger ubig) => ubig.Value;
        public static explicit operator long(UBigInteger ubig) => (long)ubig.Value;
        public static explicit operator int(UBigInteger ubig) => (int)ubig.Value;
        public static explicit operator short(UBigInteger ubig) => (short)ubig.Value;
        public static explicit operator sbyte(UBigInteger ubig) => (sbyte)ubig.Value;

        public static explicit operator float(UBigInteger ubig) => (float)ubig.Value;
        public static explicit operator double(UBigInteger ubig) => (double)ubig.Value;
        public static explicit operator decimal(UBigInteger ubig) => (decimal)ubig.Value;

        // o------------o
        // | Operators: |
        // o------------o
        public static UBigInteger operator +(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value + other.Value);
        public static UBigInteger operator +(UBigInteger self, BigInteger other) => new UBigInteger(self.Value + other);
        public static UBigInteger operator -(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value - other.Value);
        public static UBigInteger operator -(UBigInteger self, BigInteger other) => new UBigInteger(self.Value - other);
        public static UBigInteger operator *(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value * other.Value);
        public static UBigInteger operator *(UBigInteger self, BigInteger other) => new UBigInteger(self.Value * other);
        public static UBigInteger operator /(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value / other.Value);
        public static UBigInteger operator /(UBigInteger self, BigInteger other) => new UBigInteger(self.Value / other);
        public static UBigInteger operator %(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value % other.Value);
        public static UBigInteger operator %(UBigInteger self, BigInteger other) => new UBigInteger(self.Value % other);
        public static UBigInteger operator ++(UBigInteger self) => new UBigInteger(self.Value + 1);
        public static UBigInteger operator --(UBigInteger self) => new UBigInteger(self.Value - 1);

        public static BigInteger operator +(UBigInteger self) => self.Value;
        public static BigInteger operator -(UBigInteger self) => -self.Value;

        public static bool operator >(UBigInteger self, UBigInteger other) => self.Value > other.Value;
        public static bool operator >=(UBigInteger self, UBigInteger other) => self.Value >= other.Value;
        public static bool operator <(UBigInteger self, UBigInteger other) => self.Value < other.Value;
        public static bool operator <=(UBigInteger self, UBigInteger other) => self.Value <= other.Value;
        public static bool operator ==(UBigInteger self, UBigInteger other) => self.Value == other.Value;
        public static bool operator !=(UBigInteger self, UBigInteger other) => self.Value != other.Value;

        // o-------------o
        // | Operations: |
        // o-------------o

        /// <summary>
        /// Returns an integer representing the comparison between the two given UBigIntegers
        /// </summary>
        /// <returns>
        /// (-1): left &lt; right,
        /// (0): left == right,
        /// (1): left &gt; right
        /// </returns>
        public static int Compare(UBigInteger left, UBigInteger right) => BigInteger.Compare(left.Value, right.Value);

        /// <summary>
        /// Returns whether the two UBigIntegers are equal.
        /// </summary>
        public static bool Equals(UBigInteger left, UBigInteger right) => BigInteger.Equals(left.Value, right.Value);

        /// <summary>
        /// Returns the largest of the two given UBigIntegers.
        /// </summary>
        public static UBigInteger Max(UBigInteger left, UBigInteger right) => new UBigInteger(BigInteger.Max(left.Value, right.Value));

        /// <summary>
        /// Returns the smallest of the two given UBigIntegers.
        /// </summary>
        public static UBigInteger Min(UBigInteger left, UBigInteger right) => new UBigInteger(BigInteger.Min(left.Value, right.Value));

        /// <summary>
        /// Finds the greatest common divisor of two given UBigIntegers.
        /// </summary>
        public static UBigInteger GreatestCommonDivisor(UBigInteger left, UBigInteger right) => new UBigInteger(BigInteger.GreatestCommonDivisor(left.Value, right.Value));

        /// <summary>
        /// Returns the natural (base e) logarithm of the given UBigInteger.
        /// </summary>
        public static double Log(UBigInteger value) => BigInteger.Log(value);

        /// <summary>
        /// Returns the base 10 logarithm of the given UBigInteger.
        /// </summary>
        public static double Log10(UBigInteger value) => BigInteger.Log10(value);

        /// <summary>
        /// Returns the logarithm of the given UBigInteger in the specified base.
        /// </summary>
        public static double Log(UBigInteger value, double baseValue) => BigInteger.Log(value, baseValue);

        /// <summary>
        /// Returns the sum of the two given UBigIntegers.
        /// </summary>
        public static UBigInteger Add(UBigInteger left, UBigInteger right) => left + right;
        /// <summary>
        /// Returns the difference of the two given UBigIntegers.
        /// </summary>
        public static UBigInteger Subtract(UBigInteger left, UBigInteger right) => left - right;
        /// <summary>
        /// Returns the product of the two given UBigIntegers.
        /// </summary>
        public static UBigInteger Multiply(UBigInteger left, UBigInteger right) => left * right;
        /// <summary>
        /// Returns the quotient of the two given UBigIntegers.
        /// </summary>
        public static UBigInteger Divide(UBigInteger left, UBigInteger right) => left / right;
        /// <summary>
        /// Returns the remainder of the two given UBigIntegers.
        /// </summary>
        public static UBigInteger Remainder(UBigInteger dividend, UBigInteger divisor) => (UBigInteger)BigInteger.Remainder(dividend, divisor);
        /// <summary>
        /// Returns the quotient of the two given UBigIntegers and the remainder.
        /// </summary>
        public static UBigInteger DivRem(UBigInteger dividend, UBigInteger divisor, out UBigInteger remainder) => UBigInteger.DivRem(dividend, divisor, out remainder);
        /// <summary>
        /// Returns the given UBigInteger raised to the given exponent.
        /// </summary>
        public static UBigInteger Pow(UBigInteger value, int exponent) => (UBigInteger)BigInteger.Pow(value, exponent);

        /// <summary>
        /// Performs modulus division on the given value raised to the given exponent and the given modulus
        /// </summary>
        public static UBigInteger ModPow(UBigInteger value, UBigInteger exponent, UBigInteger modulus) => (UBigInteger)BigInteger.ModPow(value, exponent, modulus);

        /// <summary>
        /// A UBigInteger equal to 0.
        /// </summary>
        public static UBigInteger Zero { get => new UBigInteger(BigInteger.Zero); }

        /// <summary>
        /// A UBigInteger equal to 1.
        /// </summary>
        public static UBigInteger One { get => new UBigInteger(BigInteger.One); }

        /// <summary>
        /// The smallest possible value of a UBigInter (zero). There is no MaxValue.
        /// </summary>
        public static UBigInteger MinValue { get => Zero; }
    }
}
