using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;


namespace JesseRussell.Numerics
{
    /// <summary>
    /// Represents either IntFloat or Fraction based on context.
    /// </summary>
    public readonly struct Iflac : IComparable<BigInteger>, IComparable<double>, IComparable<float>, IComparable<Doudec>, IComparable<IntFloat>, IComparable<Fraction>, IComparable<Iflac>, IEquatable<BigInteger>, IEquatable<double>, IEquatable<float>, IEquatable<Doudec>, IEquatable<IntFloat>, IEquatable<Fraction>, IEquatable<Iflac>
    {
        #region public Constructors
        public Iflac(IntFloat value)
        {
            intFloatNotFraction = true;
            this.value = value;
        }
        public Iflac(Fraction value)
        {
            intFloatNotFraction = false;
            this.value = value;
        }

        public Iflac(sbyte i) : this(new IntFloat(i)) { }
        public Iflac(short i) : this(new IntFloat(i)) { }
        public Iflac(int i) : this(new IntFloat(i)) { }
        public Iflac(long i) : this(new IntFloat(i)) { }
        public Iflac(BigInteger i) : this(new IntFloat(i)) { }

        public Iflac(byte i) : this(new IntFloat(i)) { }
        public Iflac(ushort i) : this(new IntFloat(i)) { }
        public Iflac(uint i) : this(new IntFloat(i)) { }
        public Iflac(ulong i) : this(new IntFloat(i)) { }
        public Iflac(UBigInteger i) : this(new IntFloat(i)) { }
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
        public Iflac Increment() => this + 1;
        public Iflac Decrement() => this - 1;
        #endregion
        #region Comparison
        #region CompareTo
        public int CompareTo(BigInteger bi) => intFloatNotFraction ? ifloat.CompareTo(bi) : frac.CompareTo(bi);
        public int CompareTo(double d) => intFloatNotFraction ? ifloat.CompareTo(d) : frac.ToDouble().CompareTo(d);
        public int CompareTo(float f) => intFloatNotFraction ? ifloat.CompareTo(f) : frac.ToFloat().CompareTo(f);
        public int CompareTo(Doudec dd) => intFloatNotFraction ? ifloat.CompareTo(dd) : frac.ToDoudec().CompareTo(dd);
        public int CompareTo(IntFloat ift) => intFloatNotFraction ? ifloat.CompareTo(ift) : IntFloat.FromFraction(frac).CompareTo(ift);
        public int CompareTo(Fraction f) => Fraction.CompareTo(f);
        public int CompareTo(Iflac other) => other.intFloatNotFraction ? CompareTo(other.ifloat) : CompareTo(other.frac);
        #endregion

        #region Equals
        public bool Equals(BigInteger bi) => intFloatNotFraction ? ifloat.Equals(bi) : frac.Equals(bi);
        public bool Equals(double d) => intFloatNotFraction ? ifloat.Equals(d) : frac.Equals(d);
        public bool Equals(float f) => intFloatNotFraction ? ifloat.Equals(f) : frac.Equals(f);
        public bool Equals(Doudec dd) => intFloatNotFraction ? ifloat.Equals(dd) : frac.Equals(dd);
        public bool Equals(IntFloat ift) => intFloatNotFraction ? ifloat.Equals(ift) : frac.Equals(ift);
        public bool Equals(Fraction f) => Fraction.Equals(f);
        public bool Equals(Iflac other) => other.intFloatNotFraction ? Equals(other.ifloat) : Equals(other.frac);
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
        public static Iflac Add(Iflac left, Iflac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float + right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction + right.Fraction;
            return left.Int + right.Int;
        }
        public static Iflac Subtract(Iflac left, Iflac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float - right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction - right.Fraction;
            return left.Int - right.Int;
        }
        public static Iflac Multiply(Iflac left, Iflac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float * right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction * right.Fraction;
            return left.Int * right.Int;
        }
        public static Iflac Divide(Iflac left, Iflac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float / right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction / right.Fraction;
            BigInteger l_i = left.Int;
            BigInteger r_i = right.Int;
            if (l_i % r_i == 0) return l_i / r_i;
            return left.Fraction / right.Fraction;
        }

        public static Iflac FloorDivide(Iflac left, Iflac right) => Floor(Divide(left, right));

        public static Iflac Remainder(Iflac left, Iflac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float % right.Float); ;
            if (left.IsFraction || right.IsFraction) return left.Fraction % right.Fraction;
            return left.Int % right.Int;
        }
        public static Iflac Pow(Iflac x, int y)
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

        public static Iflac Pow(Iflac x, double y) => x.intFloatNotFraction switch
        {
            true => new IntFloatFrac(IntFloat.Pow(x.ifloat, y)),
            false => new IntFloatFrac(Doudec.Pow((Doudec)x.frac, y))
        };

        public static Iflac Pow(Iflac x, IntFloat y)
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

        public static Iflac Abs(Iflac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Abs(x.ifloat)) : x.frac.Abs;
        public static Iflac Neg(Iflac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Negate(x.ifloat)) : x.frac.Negate();
        public static Iflac Floor(Iflac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Floor(x.ifloat)) : x.frac.Floor();
        public static Iflac Ceiling(Iflac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Ceiling(x.ifloat)) : x.frac.Ceiling();
        public static Iflac Truncate(Iflac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Truncate(x.ifloat)) : x.frac.Truncate();
        public static int Sign(Iflac iff) => iff.intFloatNotFraction ? IntFloat.Sign(iff.ifloat) : iff.frac.Sign;
        public static Iflac Log(Iflac iff) => Math.Log(iff.Float.Double);
        public static Iflac Log(Iflac iff, double newBase) => Math.Log(iff.Float.Double, newBase);
        public static Iflac Log10(Iflac iff) => Math.Log10(iff.Float.Double);
        public static Iflac Min(Iflac a, Iflac b) => a > b ? b : a;
        public static Iflac Max(Iflac a, Iflac b) => a < b ? b : a;
        #endregion

        #region Parse
        public static bool TryParse(string s, out Iflac result)
        {
            if (Fraction.TryParse(s, out Fraction f))
            {
                result = f;
                return true;
            }
            if (IntFloat.TryParse(s, out IntFloat i))
            {
                result = new Iflac(i);
                return true;
            }
            result = default;
            return false;
        }
        public static Iflac Parse(string s)
        {
            if (TryParse(s, out Iflac result))
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
        public static Iflac operator -(Iflac iff) => Neg(iff);
        public static Iflac operator +(Iflac iff) => iff;

        public static Iflac operator +(Iflac left, Iflac right) => Add(left, right);
        public static Iflac operator -(Iflac left, Iflac right) => Subtract(left, right);
        public static Iflac operator *(Iflac left, Iflac right) => Multiply(left, right);
        public static Iflac operator /(Iflac left, Iflac right) => Divide(left, right);
        public static Iflac operator %(Iflac left, Iflac right) => Remainder(left, right);

        public static Iflac operator ++(Iflac iff) => iff.Increment();
        public static Iflac operator --(Iflac iff) => iff.Decrement();

        public static bool operator ==(Iflac left, Iflac right) => left.Equals(right);
        public static bool operator !=(Iflac left, Iflac right) => !left.Equals(right);

        public static bool operator >(Iflac left, Iflac right) => left.CompareTo(right) > 0;
        public static bool operator >=(Iflac left, Iflac right) => left.CompareTo(right) >= 0;
        public static bool operator <(Iflac left, Iflac right) => left.CompareTo(right) < 0;
        public static bool operator <=(Iflac left, Iflac right) => left.CompareTo(right) <= 0;
        #endregion

        #region Casts
        #region from
        // integer -> IntFloatFrac
        public static implicit operator Iflac(sbyte sb) => new Iflac((BigInteger)sb);
        public static implicit operator Iflac(short s) => new Iflac((BigInteger)s);
        public static implicit operator Iflac(int i) => new Iflac((BigInteger)i);
        public static implicit operator Iflac(long l) => new Iflac((BigInteger)l);
        public static implicit operator Iflac(BigInteger bi) => new Iflac(bi);

        public static implicit operator Iflac(byte b) => new Iflac((BigInteger)b);
        public static implicit operator Iflac(ushort us) => new Iflac((BigInteger)us);
        public static implicit operator Iflac(uint ui) => new Iflac((BigInteger)ui);
        public static implicit operator Iflac(ulong ul) => new Iflac((BigInteger)ul);
        public static implicit operator Iflac(UBigInteger ubi) => new Iflac(ubi);

        // floating point -> IntFloatFrac
        public static implicit operator Iflac(float f) => FromDouble(f);
        public static implicit operator Iflac(double f) => FromDouble(f);
        public static implicit operator Iflac(decimal f) => FromDecimal(f);
        public static implicit operator Iflac(Doudec f) => FromDoudec(f);

        // Fraction -> IntFloatFrac
        public static implicit operator Iflac(Fraction f) => FromFraction(f);
        public static implicit operator Iflac(FractionOperation fo) => FromFraction(fo);

        // IntFloat -> IntFloatFrac
        public static implicit operator Iflac(IntFloat ift) => FromIntFloat(ift);
        #endregion

        #region to
        // IntFloatFrac -> integer
        public static explicit operator sbyte(Iflac iff) => (sbyte)iff.Int;
        public static explicit operator short(Iflac iff) => (short)iff.Int;
        public static explicit operator int(Iflac iff) => (int)iff.Int;
        public static explicit operator long(Iflac iff) => (long)iff.Int;
        public static explicit operator BigInteger(Iflac iff) => iff.Int;

        public static explicit operator byte(Iflac iff) => (byte)iff.Int;
        public static explicit operator ushort(Iflac iff) => (ushort)iff.Int;
        public static explicit operator uint(Iflac iff) => (uint)iff.Int;
        public static explicit operator ulong(Iflac iff) => (ulong)iff.Int;
        public static explicit operator UBigInteger(Iflac iff) => (UBigInteger)iff.Int;

        // IntFloatFrac -> floating point
        public static explicit operator float(Iflac iff) => (float)iff.Float;
        public static explicit operator double(Iflac iff) => (double)iff.Float;
        public static explicit operator decimal(Iflac iff) => (decimal)iff.Float;
        public static explicit operator Doudec(Iflac iff) => iff.Float;

        // IntFloatFrac -> Fraction
        public static explicit operator Fraction(Iflac iff) => iff.Fraction;

        // IntFloatFract -> IntFloat
        public static explicit operator IntFloat(Iflac iff) => iff.IntFloat;
        #endregion
        #endregion

        #region Conversion
        /// <summary>
        /// Conversion from double. Does not prioritize fraction.
        /// </summary>
        public static Iflac FromDouble(double d)
        {
            return new IntFloatFrac((Doudec)d);
        }

        /// <summary>
        /// Conversion that prioritizes Fraction.
        /// </summary>
        public static Iflac FromDecimal(decimal dec) => Fraction.FromDecimal(dec);

        /// <summary>
        /// Conversion that prioritizes Fraction.
        /// </summary>
        public static Iflac FromDoudec(Doudec d)
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
        public static Iflac FromIntFloat(IntFloat i)
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
        public static Iflac FromFraction(Fraction f)
        {
            if (f.IsWhole)
            {
                return f.ToBigInteger();
            }
            else
            {
                return new Iflac(f);
            }
        }
        #endregion
        #endregion

        #region public static Properties { get; }
        public static Iflac NaN => new IntFloatFrac(double.NaN);
        public static Iflac PositiveInfinity => new IntFloatFrac(double.PositiveInfinity);
        public static Iflac NegativeInfinity => new IntFloatFrac(double.NegativeInfinity);
        public static Iflac Epsilon => new IntFloatFrac(double.Epsilon);
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


    public static class IflacUtils
    {
        public static Iflac Sum(this IEnumerable<Iflac> items) => items.Aggregate((total, next) => total + next);
    }
}