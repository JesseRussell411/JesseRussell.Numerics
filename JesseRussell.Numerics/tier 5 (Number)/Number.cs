using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;


namespace JesseRussell.Numerics
{
    /// <summary>
    /// Represents either IntFloat or Fraction based on context.
    /// </summary>
    public readonly struct Number : IComparable<BigInteger>, IComparable<double>, IComparable<float>, IComparable<Doudec>, IComparable<IntFloat>, IComparable<Fraction>, IComparable<Number>, IEquatable<BigInteger>, IEquatable<double>, IEquatable<float>, IEquatable<Doudec>, IEquatable<IntFloat>, IEquatable<Fraction>, IEquatable<Number>
    {
        #region public Constructors
        public Number(IntFloat value)
        {
            intFloatNotFraction = true;
            this.value = value;
        }
        public Number(Fraction value)
        {
            intFloatNotFraction = false;
            this.value = value;
        }

        public Number(sbyte i) : this(new IntFloat(i)) { }
        public Number(short i) : this(new IntFloat(i)) { }
        public Number(int i) : this(new IntFloat(i)) { }
        public Number(long i) : this(new IntFloat(i)) { }
        public Number(BigInteger i) : this(new IntFloat(i)) { }

        public Number(byte i) : this(new IntFloat(i)) { }
        public Number(ushort i) : this(new IntFloat(i)) { }
        public Number(uint i) : this(new IntFloat(i)) { }
        public Number(ulong i) : this(new IntFloat(i)) { }
        public Number(UBigInteger i) : this(new IntFloat(i)) { }
        #endregion

        #region public Properties { get; }
        public BigInteger Int => intFloatNotFraction ? ifloat.Int : frac.ToBigInteger();
        public Doudec Float => intFloatNotFraction ? ifloat.Float : frac.ToDoudec();
        public IntFloat IntFloat => intFloatNotFraction ? ifloat : (IntFloat)frac;
        public Fraction Fraction
        {
            get
            {
                if (intFloatNotFraction)
                {
                    if (ifloat.IsInt)
                    {
                        return new Fraction(ifloat.Int);
                    }
                    else
                    {
                        return Fraction.FromDoudec(ifloat.Float);
                    }
                }
                else
                {
                    return frac;
                }
            }
        }

        public bool IsInt => intFloatNotFraction && ifloat.IsInt;
        public bool IsFloat => intFloatNotFraction && ifloat.IsFloat;
        public bool IsFraction => !intFloatNotFraction;
        public bool IsIntFloat => intFloatNotFraction;

        public bool IsNegative => intFloatNotFraction ? ifloat.IsNegative : frac.IsNegative;
        public bool IsZero => intFloatNotFraction ? ifloat.IsZero : frac.IsZero;
        #endregion

        #region public Methods
        #region Math
        public Number Increment() => this + 1;
        public Number Decrement() => this - 1;
        #endregion
        #region Comparison
        #region CompareTo
        public int CompareTo(BigInteger bi) => intFloatNotFraction ? ifloat.CompareTo(bi) : frac.CompareTo(bi);
        public int CompareTo(double d) => intFloatNotFraction ? ifloat.CompareTo(d) : frac.ToDouble().CompareTo(d);
        public int CompareTo(float f) => intFloatNotFraction ? ifloat.CompareTo(f) : frac.ToFloat().CompareTo(f);
        public int CompareTo(Doudec dd) => intFloatNotFraction ? ifloat.CompareTo(dd) : frac.ToDoudec().CompareTo(dd);
        public int CompareTo(IntFloat ift) => intFloatNotFraction ? ifloat.CompareTo(ift) : IntFloat.FromFraction(frac).CompareTo(ift);
        public int CompareTo(Fraction f) => Fraction.CompareTo(f);
        public int CompareTo(Number other) => other.intFloatNotFraction ? CompareTo(other.ifloat) : CompareTo(other.frac);
        #endregion

        #region Equals
        public bool Equals(BigInteger bi) => intFloatNotFraction ? ifloat.Equals(bi) : frac.Equals(bi);
        public bool Equals(double d) => intFloatNotFraction ? ifloat.Equals(d) : frac.Equals(d);
        public bool Equals(float f) => intFloatNotFraction ? ifloat.Equals(f) : frac.Equals(f);
        public bool Equals(Doudec dd) => intFloatNotFraction ? ifloat.Equals(dd) : frac.Equals(dd);
        public bool Equals(IntFloat ift) => intFloatNotFraction ? ifloat.Equals(ift) : frac.Equals(ift);
        public bool Equals(Fraction f) => Fraction.Equals(f);
        public bool Equals(Number other) => other.intFloatNotFraction ? Equals(other.ifloat) : Equals(other.frac);
        public override bool Equals(object obj) => obj switch
        {
            sbyte i => Equals((BigInteger)i),
            short i => Equals((BigInteger)i),
            int i => Equals((BigInteger)i),
            long i => Equals((BigInteger)i),
            BigInteger i => Equals((BigInteger)i),

            byte i => Equals((BigInteger)i),
            ushort i => Equals((BigInteger)i),
            uint i => Equals((BigInteger)i),
            ulong i => Equals((BigInteger)i),
            UBigInteger i => Equals((BigInteger)i),

            double f => Equals(f),
            float f => Equals(f),
            Doudec f => Equals(f),
            Fraction f => Equals(f),

            IntFloat ift => Equals(ift),
            _ => throw new ArgumentException("The parameter must be an integer, floating point number, or fraction")
        };
        #endregion
        public override int GetHashCode() => intFloatNotFraction ? ifloat.GetHashCode() : frac.GetHashCode();
        #endregion
        public override string ToString() => intFloatNotFraction ? ifloat.ToString() : frac.ToString();
        #endregion

        #region public static Methods
        #region Math
        public static Number Add(Number left, Number right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float + right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction + right.Fraction;
            return left.Int + right.Int;
        }
        public static Number Subtract(Number left, Number right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float - right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction - right.Fraction;
            return left.Int - right.Int;
        }
        public static Number Multiply(Number left, Number right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float * right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction * right.Fraction;
            return left.Int * right.Int;
        }
        public static Number Divide(Number left, Number right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float / right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction / right.Fraction;
            BigInteger l_i = left.Int;
            BigInteger r_i = right.Int;
            if (l_i % r_i == 0) return l_i / r_i;
            return left.Fraction / right.Fraction;
        }

        public static Number FloorDivide(Number left, Number right) => Floor(Divide(left, right));

        public static Number Remainder(Number left, Number right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float % right.Float); ;
            if (left.IsFraction || right.IsFraction) return left.Fraction % right.Fraction;
            return left.Int % right.Int;
        }
        public static Number Pow(Number x, int y)
        {
            if (y < 0 && (x.IsInt || x.IsFraction))
            {
                return x.Fraction.Power(y);
            }
            else if (x.intFloatNotFraction)
            {
                return IntFloat.Pow(x.ifloat, y);
            }
            else
            {
                return x.frac.Power(y);
            }
        }

        public static Number Pow(Number x, double y) => x.intFloatNotFraction switch
        {
            true => new IntFloatFrac(IntFloat.Pow(x.ifloat, y)),
            false => new IntFloatFrac(Doudec.Pow((Doudec)x.frac, y))
        };

        public static Number Pow(Number x, IntFloat y)
        {
            if (y.IsInt)
            {
                BigInteger bi = y.Int;
                if (bi < int.MinValue) return Pow(x, double.NegativeInfinity);
                if (bi > int.MaxValue) return Pow(x, double.PositiveInfinity);

                return Pow(x, (int)bi);
            }
            else
            {
                var result =  Pow(x, y.Float.Double);
                return result;
            }
        }

        public static Number Abs(Number x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Abs(x.ifloat)) : x.frac.Abs;
        public static Number Neg(Number x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Negate(x.ifloat)) : x.frac.Negate();
        public static Number Floor(Number x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Floor(x.ifloat)) : x.frac.Floor();
        public static Number Ceiling(Number x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Ceiling(x.ifloat)) : x.frac.Ceiling();
        public static Number Truncate(Number x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Truncate(x.ifloat)) : x.frac.Truncate();
        public static int Sign(Number iff) => iff.intFloatNotFraction ? IntFloat.Sign(iff.ifloat) : iff.frac.Sign;
        public static Number Log(Number iff) => Math.Log(iff.Float.Double);
        public static Number Log(Number iff, double newBase) => Math.Log(iff.Float.Double, newBase);
        public static Number Log10(Number iff) => Math.Log10(iff.Float.Double);
        public static Number Min(Number a, Number b) => a > b ? b : a;
        public static Number Max(Number a, Number b) => a < b ? b : a;
        #endregion

        #region Parse
        public static bool TryParse(string s, out Number result)
        {
            if (Fraction.TryParse(s, out Fraction f))
            {
                result = f;
                return true;
            }
            if (IntFloat.TryParse(s, out IntFloat i))
            {
                result = new Number(i);
                return true;
            }
            result = default;
            return false;
        }
        public static Number Parse(string s)
        {
            if (TryParse(s, out Number result))
            {
                return result;
            }
            else
            {
                throw new FormatException("The string is not a valid integer, floating point number, or fraction.");
            }
        }
        #endregion

        #region Operators
        public static Number operator -(Number iff) => Neg(iff);
        public static Number operator +(Number iff) => iff;

        public static Number operator +(Number left, Number right) => Add(left, right);
        public static Number operator -(Number left, Number right) => Subtract(left, right);
        public static Number operator *(Number left, Number right) => Multiply(left, right);
        public static Number operator /(Number left, Number right) => Divide(left, right);
        public static Number operator %(Number left, Number right) => Remainder(left, right);

        public static Number operator ++(Number iff) => iff.Increment();
        public static Number operator --(Number iff) => iff.Decrement();

        public static bool operator ==(Number left, Number right) => left.Equals(right);
        public static bool operator !=(Number left, Number right) => !left.Equals(right);

        public static bool operator >(Number left, Number right) => left.CompareTo(right) > 0;
        public static bool operator >=(Number left, Number right) => left.CompareTo(right) >= 0;
        public static bool operator <(Number left, Number right) => left.CompareTo(right) < 0;
        public static bool operator <=(Number left, Number right) => left.CompareTo(right) <= 0;
        #endregion

        #region Casts
        #region from
        // integer -> IntFloatFrac
        public static implicit operator Number(sbyte sb) => new Number((BigInteger)sb);
        public static implicit operator Number(short s) => new Number((BigInteger)s);
        public static implicit operator Number(int i) => new Number((BigInteger)i);
        public static implicit operator Number(long l) => new Number((BigInteger)l);
        public static implicit operator Number(BigInteger bi) => new Number(bi);

        public static implicit operator Number(byte b) => new Number((BigInteger)b);
        public static implicit operator Number(ushort us) => new Number((BigInteger)us);
        public static implicit operator Number(uint ui) => new Number((BigInteger)ui);
        public static implicit operator Number(ulong ul) => new Number((BigInteger)ul);
        public static implicit operator Number(UBigInteger ubi) => new Number(ubi);

        // floating point -> IntFloatFrac
        public static implicit operator Number(float f) => FromDouble(f);
        public static implicit operator Number(double f) => FromDouble(f);
        public static implicit operator Number(decimal f) => FromDecimal(f);
        public static implicit operator Number(Doudec f) => FromDoudec(f);

        // Fraction -> IntFloatFrac
        public static implicit operator Number(Fraction f) => FromFraction(f);
        public static implicit operator Number(FractionOperation fo) => FromFraction(fo);

        // IntFloat -> IntFloatFrac
        public static implicit operator Number(IntFloat ift) => FromIntFloat(ift);
        #endregion

        #region to
        // IntFloatFrac -> integer
        public static explicit operator sbyte(Number iff) => (sbyte)iff.Int;
        public static explicit operator short(Number iff) => (short)iff.Int;
        public static explicit operator int(Number iff) => (int)iff.Int;
        public static explicit operator long(Number iff) => (long)iff.Int;
        public static explicit operator BigInteger(Number iff) => iff.Int;

        public static explicit operator byte(Number iff) => (byte)iff.Int;
        public static explicit operator ushort(Number iff) => (ushort)iff.Int;
        public static explicit operator uint(Number iff) => (uint)iff.Int;
        public static explicit operator ulong(Number iff) => (ulong)iff.Int;
        public static explicit operator UBigInteger(Number iff) => (UBigInteger)iff.Int;

        // IntFloatFrac -> floating point
        public static explicit operator float(Number iff) => (float)iff.Float;
        public static explicit operator double(Number iff) => (double)iff.Float;
        public static explicit operator decimal(Number iff) => (decimal)iff.Float;
        public static explicit operator Doudec(Number iff) => iff.Float;

        // IntFloatFrac -> Fraction
        public static explicit operator Fraction(Number iff) => iff.Fraction;

        // IntFloatFract -> IntFloat
        public static explicit operator IntFloat(Number iff) => iff.IntFloat;
        #endregion
        #endregion

        #region Conversion
        /// <summary>
        /// Conversion from double. Does not prioritize fraction.
        /// </summary>
        public static Number FromDouble(double d)
        {
            return new IntFloatFrac((Doudec)d);
        }

        /// <summary>
        /// Conversion that prioritizes Fraction.
        /// </summary>
        public static Number FromDecimal(decimal dec) => Fraction.FromDecimal(dec);

        /// <summary>
        /// Conversion that prioritizes Fraction.
        /// </summary>
        public static Number FromDoudec(Doudec d)
        {
            if (d.IsDouble)
            {
                return FromDouble(d.Double);
            }
            else
            {
                return FromDecimal(d.Decimal);
            }
        }

        /// <summary>
        /// Conversion that prioritizes Fraction.
        /// </summary>
        public static Number FromIntFloat(IntFloat i)
        {
            if (i.IsInt)
            {
                return i.Int;
            }
            else
            {
                return FromDoudec(i.Float);
            }
        }


        /// <summary>
        /// Conversion that prioritizes BigInteger.
        /// </summary>
        public static Number FromFraction(Fraction f)
        {
            if (f.IsWhole)
            {
                return f.ToBigInteger();
            }
            else
            {
                return new Number(f);
            }
        }
        #endregion
        #endregion

        #region public static Properties { get; }
        public static Number NaN => new IntFloatFrac(double.NaN);
        public static Number PositiveInfinity => new IntFloatFrac(double.PositiveInfinity);
        public static Number NegativeInfinity => new IntFloatFrac(double.NegativeInfinity);
        public static Number Epsilon => new IntFloatFrac(double.Epsilon);
        #endregion

        #region private readonly Fields
        private readonly bool intFloatNotFraction;
        private readonly object value;
        #endregion

        #region private Properties { get; }
        private IntFloat ifloat => (IntFloat)(value ?? default(IntFloat));
        private Fraction frac => (Fraction)(value ?? default(Fraction));
        #endregion
    }


    public static class NumberUtils
    {
        public static Number Sum(this IEnumerable<Number> items) => items.Aggregate((total, next) => total + next);
    }
}