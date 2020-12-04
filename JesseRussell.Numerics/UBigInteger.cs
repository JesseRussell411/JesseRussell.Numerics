using System;
using System.Numerics;

namespace JesseRussell.Numerics
{
    /// <summary>
    /// Unsigned BigInteger. Not really unsigned of course; just artificially forced to be positive; effectively making it unsigned for most purposes. However, unlike other unsigned types, it's overflow behavior (or rather underflow) is to become it's own absolute value instead of rolling over to max value, since BigInteger does not have a max value.
    /// </summary>
    public struct UBigInteger : IComparable, IComparable<UBigInteger>, IComparable<BigInteger>, IEquatable<UBigInteger>, IEquatable<BigInteger>, IFormattable
    {
        #region public Properties
        public readonly BigInteger Value;

        public bool IsEven { get => Value.IsEven; }
        public bool IsZero { get => Value.IsZero; }
        public bool IsOne { get => Value.IsOne; }
        public bool IsPowerOfTwo { get => Value.IsPowerOfTwo; }
        public int Sign { get => Value.Sign; }
        #endregion

        #region public Constructors
        #region Primary Constructors. These constructors MUST ensure that Value is not negative.
        /// <summary>
        /// Constructs new uBigInteger and sets Value to the absolute value of value.
        /// </summary>
        /// <param name="value"></param>
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
            // *Byte is unsigned
            // No need for abs.
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

        #endregion
        #region Extension Constructors. These Constructors MUST extend another constructor
        public UBigInteger(float f) : this(new BigInteger(f)) { }
        public UBigInteger(double d) : this(new BigInteger(d)) { }
        public UBigInteger(decimal dec) : this(new BigInteger(dec)) { }
        #endregion
        #endregion

        #region public Methods
        public string ToString(string format) => Value.ToString(format);
        public string ToString(IFormatProvider provider) => Value.ToString(provider);
        public string ToString(string format, IFormatProvider provider) => Value.ToString(format, provider);
        public override string ToString() => Value.ToString();
        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object other) => other is UBigInteger ubig ? Value.Equals(ubig.Value) : Value.Equals(other);

        public int CompareTo(object obj)
        {
            if (obj is UBigInteger ubig) { return Value.CompareTo((object)ubig.Value); }

            return Value.CompareTo(obj);
        }

        public int CompareTo(UBigInteger other) => Value.CompareTo(other.Value);
        public int CompareTo(BigInteger other) => Value.CompareTo(other);

        public bool Equals(UBigInteger other) => Value.Equals(other.Value);
        public bool Equals(BigInteger other) => Value.Equals(other);
        #endregion

        #region public static Methods
        #region parse
        public static UBigInteger Parse(string s)
        {
            BigInteger value = BigInteger.Parse(s);
            if (value < 0)
            {
                throw new FormatException($"UBigInteger cannot be negative.");
            }
            return new UBigInteger(value);
        }
        public static bool TryParse(string s, out UBigInteger result)
        {
            if (BigInteger.TryParse(s, out BigInteger big) && big >= 0)
            {
                result = (UBigInteger)big;
                return true;
            }
            else
            {
                result = (UBigInteger)big;
                return false;
            }
        }
        #endregion

        #region casts
        public static implicit operator BigInteger(UBigInteger ubig) => ubig.Value;
        public static explicit operator UBigInteger(BigInteger big) => new UBigInteger(big);

        public static implicit operator UBigInteger(ulong ul) => new UBigInteger(ul);
        public static implicit operator UBigInteger(uint ui) => new UBigInteger(ui);
        public static implicit operator UBigInteger(ushort ui) => new UBigInteger(ui);
        public static implicit operator UBigInteger(byte b) => new UBigInteger(b); // *byte is unsigned.

        public static explicit operator UBigInteger(long l) => new UBigInteger(l);
        public static explicit operator UBigInteger(int i) => new UBigInteger(i);
        public static explicit operator UBigInteger(short s) => new UBigInteger(s);
        public static explicit operator UBigInteger(sbyte sb) => new UBigInteger(sb);

        public static explicit operator UBigInteger(float f) => new UBigInteger(f);
        public static explicit operator UBigInteger(double d) => new UBigInteger(d);
        public static explicit operator UBigInteger(decimal dec) => new UBigInteger(dec);

        public static explicit operator ulong(UBigInteger ubig) => (ulong)ubig.Value;
        public static explicit operator uint(UBigInteger ubig) => (uint)ubig.Value;
        public static explicit operator ushort(UBigInteger ubig) => (ushort)ubig.Value;
        public static explicit operator byte(UBigInteger ubig) => (byte)ubig.Value;

        public static explicit operator long(UBigInteger ubig) => (long)ubig.Value;
        public static explicit operator int(UBigInteger ubig) => (int)ubig.Value;
        public static explicit operator short(UBigInteger ubig) => (short)ubig.Value;
        public static explicit operator sbyte(UBigInteger ubig) => (sbyte)ubig.Value;

        public static explicit operator float(UBigInteger ubig) => (float)ubig.Value;
        public static explicit operator double(UBigInteger ubig) => (double)ubig.Value;
        public static explicit operator decimal(UBigInteger ubig) => (decimal)ubig.Value;
        #endregion

        #region Operators
        public static UBigInteger operator +(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value + other.Value);
        public static UBigInteger operator -(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value - other.Value);
        public static UBigInteger operator *(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value * other.Value);
        public static UBigInteger operator /(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value / other.Value);
        public static UBigInteger operator %(UBigInteger self, UBigInteger other) => new UBigInteger(self.Value % other.Value);
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
        #endregion

        public static UBigInteger Abs(UBigInteger value) => value; //* What's the point! I love it!

        public static int Compare(UBigInteger left, UBigInteger right) => BigInteger.Compare(left.Value, right.Value);
        public static bool Equals(UBigInteger left, UBigInteger right) => BigInteger.Equals(left.Value, right.Value);
        public static UBigInteger Max(UBigInteger left, UBigInteger right) => new UBigInteger(BigInteger.Max(left.Value, right.Value));
        public static UBigInteger Min(UBigInteger left, UBigInteger right) => new UBigInteger(BigInteger.Min(left.Value, right.Value));

        /// <summary>
        /// Finds the greatest common divisor of two values.
        /// </summary>
        /// <param name="left">The first value</param>
        /// <param name="right">The second value</param>
        /// <returns>The greatest common divisor of left and right.</returns>
        public static UBigInteger GreatestCommonDivisor(UBigInteger left, UBigInteger right) => new UBigInteger(BigInteger.GreatestCommonDivisor(left.Value, right.Value));
        public static double Log(UBigInteger value) => BigInteger.Log(value);
        public static double Log10(UBigInteger value) => BigInteger.Log10(value);
        public static double Log(UBigInteger value, double baseValue) => BigInteger.Log(value, baseValue);

        public static UBigInteger Add(UBigInteger left, UBigInteger right) => left + right;
        public static UBigInteger Subtract(UBigInteger left, UBigInteger right) => left - right;
        public static UBigInteger Multiply(UBigInteger left, UBigInteger right) => left * right;
        public static UBigInteger Divide(UBigInteger left, UBigInteger right) => left / right;
        public static UBigInteger Remainder(UBigInteger dividend, UBigInteger divisor) => (UBigInteger)BigInteger.Remainder(dividend, divisor);
        public static UBigInteger DivRem(UBigInteger dividend, UBigInteger divisor, out UBigInteger remainder) => UBigInteger.DivRem(dividend, divisor, out remainder);
        public static UBigInteger Pow(UBigInteger value, int exponent) => (UBigInteger)BigInteger.Pow(value, exponent);


        public static UBigInteger ModPow(UBigInteger value, UBigInteger exponent, UBigInteger modulus) => (UBigInteger)BigInteger.ModPow(value, exponent, modulus);
        #endregion

        #region public static Properties
        public static UBigInteger Zero { get => new UBigInteger(BigInteger.Zero); }
        public static UBigInteger One { get => new UBigInteger(BigInteger.One); }
        public static UBigInteger MinValue { get => Zero; }
        #endregion
    }
}
