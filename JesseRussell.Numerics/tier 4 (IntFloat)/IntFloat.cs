using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace JesseRussell.Numerics
{
    /// <summary>
    /// Combination BigInteger and Doudec. Automatically chooses best way of storing value based on context.
    /// </summary>
    public readonly struct IntFloat : IComparable, IComparable<IntFloat>, IEquatable<IntFloat>
    {
        #region public Constructors
        public IntFloat(BigInteger value)
        {
            this.value = value;
            floatNotInt = false;
        }

        public IntFloat(Doudec value)
        {
            this.value = value;
            floatNotInt = true;
        }

        public IntFloat(sbyte sb) : this((BigInteger)sb) { }
        public IntFloat(short s) : this((BigInteger)s) { }
        public IntFloat(int i) : this((BigInteger)i) { }
        public IntFloat(long l) : this((BigInteger)l) { }

        public IntFloat(byte b) : this((BigInteger)b) { }
        public IntFloat(ushort us) : this((BigInteger) us) { }
        public IntFloat(uint ui) : this((BigInteger)ui) { }
        public IntFloat(ulong ul) : this((BigInteger)ul) { }
        public IntFloat(UBigInteger ubig) : this((BigInteger)ubig) { }
        #endregion

        #region private readonly Fields
        private readonly object value;
        private readonly bool floatNotInt;
        #endregion

        #region private readonly Properties
        //private Doudec floating => (Doudec)(Value);
        //private BigInteger integer => (BigInteger)(Value);
        private Doudec floating => (Doudec)(value ?? 0);
        private BigInteger integer => (BigInteger)(value ?? default(BigInteger));
        #endregion

        #region public Properties
        /// <summary>
        /// The value in the form of a Doudec. If the value is not a Doudec, a conversion will be provided.
        /// </summary>
        public Doudec Float => floatNotInt ? floating : (Doudec)integer;
        /// <summary>
        /// The value in the form of a BigInteger. If the value is not a BigInteger, a conversion will be provided.
        /// </summary>
        public BigInteger Int => floatNotInt ? (BigInteger)floating : integer;
        public bool IsFloat => floatNotInt;
        public bool IsInt => !floatNotInt;

        public bool IsNegative => floatNotInt ? Doudec.IsNegative(floating) : integer < 0;
        public bool IsPositive => !IsNegative;

        public bool IsFinite => !floatNotInt || Doudec.IsFinite(floating);
        public bool IsPositiveInfinity { get => floatNotInt && Doudec.IsPositiveInfinity(floating); }
        public bool IsNegativeInfinity { get => floatNotInt && Doudec.IsNegativeInfinity(floating); }
        public bool IsNaN { get => floatNotInt && Doudec.IsNaN(floating); }
        public bool IsNormal { get => floatNotInt && floating.IsDouble && double.IsNormal(floating.Double); }
        public bool IsSubnormal { get => floatNotInt && floating.IsDouble && double.IsSubnormal(floating.Double); }

        public bool IsZero => floatNotInt ? floating == 0 : integer.IsZero;
        #endregion

        #region public Methods
        #region Comparison
        #region Equals
        public bool Equals(IntFloat other)
        {
            if (floatNotInt && other.floatNotInt)
            {
                return floating == other.floating;
            }
            else if (floatNotInt && !other.floatNotInt)
            {
                if (Doudec.IsFinite(floating))
                {
                    return floating == other.Float;
                }
                else
                {
                    return false;
                }
            }
            else if (!floatNotInt && other.floatNotInt)
            {
                if (Doudec.IsFinite(other.floating))
                {
                    return Float == other.floating;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return integer == other.integer;
            }
        }

        public override bool Equals(object obj)
        {
            return floatNotInt ? floating.Equals(obj) : integer.Equals(obj);
        }
        #endregion
        #region CompareTo
        public int CompareTo(IntFloat other)
        {
            if (floatNotInt)
            {
                return floating.CompareTo(other.Float);
            }
            else
            {
                return integer.CompareTo(other.Int);
            }
        }


        public int CompareTo(Doudec d)
        {
            if (floatNotInt)
            {
                return floating.CompareTo(d);
            }
            else if (Doudec.IsFinite(d))
            {
                return ((Doudec)integer).CompareTo(d);
            }
            else if (Doudec.IsPositiveInfinity(d))
            {
                return -1;
            }
            else if (Doudec.IsNegativeInfinity(d))
            {
                return 1;
            }
            else if (Doudec.IsNaN(d))
            {
                return 1;
            }
            else
            {
                return 1;
            }
        }

        public int CompareTo(BigInteger i)
        {
            if (floatNotInt)
            {
                return floating.CompareTo((Doudec)i);
            }
            else
            {
                return integer.CompareTo(i);
            }
        }

        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case IntFloat inf:
                    return CompareTo(inf);
                case Doudec d:
                    return CompareTo(d);
                case float f:
                    return CompareTo(f);
                case BigInteger bi:
                    return CompareTo(bi);
                case long l:
                    return CompareTo((BigInteger)l);
                case int i:
                    return CompareTo((BigInteger)i);
                case short s:
                    return CompareTo((BigInteger)s);
                case sbyte sb:
                    return CompareTo((BigInteger)sb);
                case UBigInteger ubi:
                    return CompareTo((BigInteger)ubi);
                case byte b:
                    return CompareTo((BigInteger)b);
                case ushort us:
                    return CompareTo((BigInteger)us);
                case uint ui:
                    return CompareTo((BigInteger)ui);
                case ulong ul:
                    return CompareTo((BigInteger)ul);
                default:
                    throw new ArgumentException("The parameter must be a float or integer");
            }
            //if (obj is IntFloat inf) { return CompareTo(inf); }

            //if (obj is Doudec d) { return CompareTo(d); }
            //if (obj is BigInteger big) { return CompareTo(big); }
            ////if (obj is decimal dec) { return CompareTo((Doudec)dec); } *could lead to problems
            //if (obj is float f) { return CompareTo((Doudec)f); }

            //if (obj is ulong ul) { return CompareTo((BigInteger)ul); }
            //if (obj is long l) { return CompareTo((BigInteger)l); }
            //if (obj is uint ui) { return CompareTo((BigInteger)ui); }
            //if (obj is int i) { return CompareTo((BigInteger)i); }
            //if (obj is Int16 i16) { return CompareTo((BigInteger)i16); }

            throw new ArgumentException("The parameter must be a float, Doudec, or integer type. (Parameter 'obj')");
        }
        #endregion
        public override int GetHashCode()
        {
            return floatNotInt ? HashCode.Combine(floating) : HashCode.Combine(integer);
        }
        #endregion
        #region Conversion
        public override string ToString()
        {
            return floatNotInt ? floating.ToString() : integer.ToString();
        }
        public Fraction ToFraction() => floatNotInt switch
        {
            true => Fraction.FromDoudec(floating),
            false => new Fraction(integer)
        };
        #endregion
        #endregion

        #region public static Methods
        #region Math
        public static IntFloat Add(IntFloat left, IntFloat right)
        {
            if (left.floatNotInt || right.floatNotInt)
            {
                return left.Float + right.Float;
            }
            else
            {
                return left.integer + right.integer;
            }
        }

        public static IntFloat Subtract(IntFloat left, IntFloat right)
        {
            if (left.floatNotInt || right.floatNotInt)
            {
                return left.Float - right.Float;
            }
            else
            {
                return left.integer - right.integer;
            }
        }

        public static IntFloat Multiply(IntFloat left, IntFloat right)
        {
            if (left.floatNotInt || right.floatNotInt)
            {
                return left.Float * right.Float;
            }
            else
            {
                return left.integer * right.integer;
            }
        }

        public static IntFloat Divide(IntFloat left, IntFloat right)
        {
            if (left.floatNotInt || right.floatNotInt)
            {
                return left.Float / right.Float;
            }
            else
            {
                if (left.integer % right.integer == 0)
                {
                    return left.integer / right.integer;
                }
                else
                {
                    return left.Float / right.Float;
                }
            }
        }

        public static IntFloat FloorDivide(IntFloat left, IntFloat right)
        {
            if (left.floatNotInt || right.floatNotInt)
            {
                return (BigInteger)Doudec.Floor(left.Float / right.Float);
            }
            else
            {
                return BigInteger.Divide(left.integer, right.integer);
            }
        }

        public static IntFloat Remainder(IntFloat left, IntFloat right)
        {
            if (left.floatNotInt || right.floatNotInt)
            {
                return left.Float % right.Float;
            }
            else
            {
                return left.integer % right.integer;
            }
        }

        public static IntFloat Pow(IntFloat x, IntFloat y)
        {
            if (x.floatNotInt || y.floatNotInt || y.IsNegative)
            {
                return Doudec.Pow(x.Float, y.Float.Double);
            }
            else if (y.integer > int.MaxValue)
            {
                // *Exponent is too big

                if (y.integer.IsEven)
                {
                    return Doudec.PositiveInfinity;
                }
                else
                {
                    return x.integer.IsEven ? Doudec.PositiveInfinity : Doudec.NegativeInfinity;
                }
            }
            else
            {
                return BigInteger.Pow(x.integer, (int)y.integer);
            }
        }

        public static IntFloat Pow(IntFloat x, Doudec y) => Doudec.Pow(x.Float, y.Double);
        public static IntFloat Pow(IntFloat x, double y)
        {
            var result = Doudec.Pow(x.Float, y);
            return result;
        }
        public static IntFloat Pow(IntFloat x, int y)
        {
            return x.floatNotInt switch
            {
                true => (IntFloat)Doudec.Pow(x.floating, y),
                false => (IntFloat)BigInteger.Pow(x.integer, y)
            };
        }
            


        public static IntFloat Negate(IntFloat value)
        {
            return value.floatNotInt ? (IntFloat)(-value.floating) : (IntFloat)(-value.integer);
        }

        public static IntFloat Floor(IntFloat value)
        {
            if (value.floatNotInt)
            {
                return (BigInteger) Doudec.Floor(value.floating);
            }
            else
            {
                return value;
            }
        }
        public static IntFloat Ceiling(IntFloat value)
        {
            if (value.floatNotInt)
            {
                return (BigInteger) Doudec.Ceiling(value.floating);
            }
            else
            {
                return value;
            }
        }
        public static IntFloat Truncate(IntFloat value)
        {
            if (value.floatNotInt)
            {
                return (BigInteger)Doudec.Truncate(value.floating);
            }
            else
            {
                return value;
            }
        }
        public static IntFloat Abs(IntFloat value)
        {
            if (value.floatNotInt)
            {
                return Doudec.Abs(value.floating);
            }
            else
            {
                return BigInteger.Abs(value.integer);
            }
        }
        public static IntFloat Log(IntFloat value)
        {
            if (value.floatNotInt)
            {
                return Doudec.Log(value.floating);
            }
            else
            {
                return BigInteger.Log(value.integer);
            }
        }
        public static IntFloat Log(IntFloat value, double baseValue)
        {
            if (value.floatNotInt)
            {
                return Doudec.Log(value.floating, baseValue);
            }
            else
            {
                return BigInteger.Log(value.integer, baseValue);
            }
        }
        public static IntFloat Log10(IntFloat value)
        {
            if (value.floatNotInt)
            {
                return Doudec.Log10(value.floating);
            }
            else
            {
                return BigInteger.Log10(value.integer);
            }
        }
        public static int Sign(IntFloat value)
        {
            if (value.floatNotInt)
            {
                return Doudec.Sign(value.floating);
            }
            else
            {
                return value.integer.Sign;
            }
        }
        public static IntFloat Min(IntFloat val1, IntFloat val2)
        {
            if (!val2.floatNotInt)
            {
                return val1 < val2 ? val1 : val2;
            }
            else
            {
                return val2 < val1 ? val2 : val1;
            }
        }
        public static IntFloat Max(IntFloat val1, IntFloat val2)
        {
            if (!val2.floatNotInt)
            {
                return val1 > val2 ? val1 : val2;
            }
            else
            {
                return val2 > val1 ? val2 : val1;
            }
        }
        #endregion
        #region Parse
        public static bool TryParse(string s, out IntFloat result)
        {
            if (BigInteger.TryParse(s, out BigInteger big))
            {
                result = big;
                return true;
            }

            if (Doudec.TryParse(s, out Doudec d))
            {
                result = d;
                return true;
            }

            result = default;
            return false;
        }

        public static IntFloat Parse(string s)
        {
            if (TryParse(s, out IntFloat result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"{s} is not a valid Doudec or BigInteger");
            }
        }
        #endregion
        #region Casts
        #region From
        // integer -> IntFloat
        public static implicit operator IntFloat(sbyte i) => new IntFloat(i);
        public static implicit operator IntFloat(short i) => new IntFloat(i);
        public static implicit operator IntFloat(int i) => new IntFloat(i);
        public static implicit operator IntFloat(long i) => new IntFloat(i);
        public static implicit operator IntFloat(BigInteger big) => new IntFloat(big);

        public static implicit operator IntFloat(byte i) => new IntFloat(i);
        public static implicit operator IntFloat(ushort i) => new IntFloat(i);
        public static implicit operator IntFloat(uint i) => new IntFloat(i);
        public static implicit operator IntFloat(ulong i) => new IntFloat(i);
        public static implicit operator IntFloat(UBigInteger ubig) => new IntFloat(ubig);

        // floating point -> IntFloat
        public static implicit operator IntFloat(float f) => FromDouble(f);
        public static implicit operator IntFloat(double d) => FromDouble(d);
        public static implicit operator IntFloat(decimal d) => FromDecimal(d);
        public static implicit operator IntFloat(Doudec dd) => FromDoudec(dd);

        // Fraction -> IntFloat
        public static explicit operator IntFloat(Fraction f) => FromFraction(f);
        public static explicit operator IntFloat(FractionOperation f) => FromFraction(f.Unsimplified);
        #endregion

        #region to
        // IntFloat -> integer
        public static explicit operator BigInteger(IntFloat iflt) => iflt.Int;
        public static explicit operator sbyte(IntFloat iflt) => (sbyte)iflt.Int;
        public static explicit operator short(IntFloat iflt) => (short)iflt.Int;
        public static explicit operator int(IntFloat iflt) => (int)iflt.Int;
        public static explicit operator long(IntFloat iflt) => (long)iflt.Int;

        public static explicit operator byte(IntFloat iflt) => (byte)iflt.Int;
        public static explicit operator ushort(IntFloat iflt) => (ushort)iflt.Int;
        public static explicit operator uint(IntFloat iflt) => (uint)iflt.Int;
        public static explicit operator ulong(IntFloat iflt) => (ulong)iflt.Int;
        public static explicit operator UBigInteger(IntFloat iflt) => (UBigInteger)iflt.Int;

        // IntFloat -> floating point
        public static explicit operator float(IntFloat i) => (float)i.Float;
        public static explicit operator double(IntFloat iflt) => (double)iflt.Float;
        public static explicit operator decimal(IntFloat iflt) => (decimal)iflt.Float;
        public static explicit operator Doudec(IntFloat iflt) => iflt.Float;

        // IntFloat -> Fraction
        public static explicit operator Fraction(IntFloat i) => i.ToFraction();

        #endregion

        #endregion
        #region Operators
        public static IntFloat operator +(IntFloat left, IntFloat right) => IntFloat.Add(left, right);
        public static IntFloat operator -(IntFloat left, IntFloat right) => IntFloat.Subtract(left, right);
        public static IntFloat operator *(IntFloat left, IntFloat right) => IntFloat.Multiply(left, right);
        public static IntFloat operator /(IntFloat left, IntFloat right) => IntFloat.Divide(left, right);
        public static IntFloat operator %(IntFloat left, IntFloat right) => IntFloat.Remainder(left, right);
        public static IntFloat operator -(IntFloat value) => IntFloat.Negate(value);
        public static IntFloat operator +(IntFloat value) => value;
        public static IntFloat operator ++(IntFloat value) => value + 1;
        public static IntFloat operator --(IntFloat value) => value - 1;

        public static bool operator >(IntFloat left, IntFloat right) => left.CompareTo(right) > 0;
        public static bool operator >=(IntFloat left, IntFloat right) => left > right || left == right;
        public static bool operator <(IntFloat left, IntFloat right) => left.CompareTo(right) < 0;
        public static bool operator <=(IntFloat left, IntFloat right) => left < right || left == right;
        public static bool operator ==(IntFloat left, IntFloat right) => left.Equals(right);
        public static bool operator !=(IntFloat left, IntFloat right) => !left.Equals(right);
        #endregion
        #region Conversion
        /// <summary>
        /// Conversion function that prioritizes BigInteger if possible;
        /// </summary>
        public static IntFloat FromDoudec(Doudec d)
        {
            return new IntFloat(d);
        }

        /// <summary>
        /// Conversion function that prioritizes BigInteger if possible;
        /// </summary>
        public static IntFloat FromDouble(double d)
        {
            return new IntFloat(d);
        }

        /// <summary>
        /// Conversion function that prioritizes BigInteger if possible;
        /// </summary>
        public static IntFloat FromDecimal(decimal d)
        {
            return new IntFloat(d);
        }

        /// <summary>
        /// Conversion function that prioritizes BigInteger if possible;
        /// </summary>
        public static IntFloat FromFraction(Fraction f)
        {
            if (f.IsWhole)
            {
                return new IntFloat(f.ToBigInteger());
            }
            else
            {
                return new IntFloat((Doudec)f);
            }
        }
        #endregion
        #endregion

        #region public static Properties
        public static IntFloat PositiveInfinity { get => Doudec.PositiveInfinity; }
        public static IntFloat NegativeInfinity { get => Doudec.NegativeInfinity; }
        public static IntFloat NaN { get => Doudec.NaN; }
        #endregion
    }

    public static class IntFloatUtils
    {
        public static IntFloat Sum(this IEnumerable<IntFloat> items) => items.Aggregate((total, next) => total + next);
    }
}
