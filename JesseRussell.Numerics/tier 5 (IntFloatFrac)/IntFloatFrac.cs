﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;


namespace JesseRussell.Numerics
{
    /// <summary>
    /// Represents either IntFloat or Fraction based on context.
    /// </summary>
    public readonly struct IntFloatFrac : IComparable<BigInteger>, IComparable<double>, IComparable<float>, IComparable<Doudec>, IComparable<IntFloat>, IComparable<Fraction>, IComparable<IntFloatFrac>, IEquatable<BigInteger>, IEquatable<double>, IEquatable<float>, IEquatable<Doudec>, IEquatable<IntFloat>, IEquatable<Fraction>, IEquatable<IntFloatFrac>
    {
        #region public Constructors
        public IntFloatFrac(IntFloat value)
        {
            intFloatNotFraction = true;
            this.value = value;
        }
        public IntFloatFrac(Fraction value)
        {
            intFloatNotFraction = false;
            this.value = value;
        }

        public IntFloatFrac(sbyte i) : this(new IntFloat(i)) { }
        public IntFloatFrac(short i) : this(new IntFloat(i)) { }
        public IntFloatFrac(int i) : this(new IntFloat(i)) { }
        public IntFloatFrac(long i) : this(new IntFloat(i)) { }
        public IntFloatFrac(BigInteger i) : this(new IntFloat(i)) { }

        public IntFloatFrac(byte i) : this(new IntFloat(i)) { }
        public IntFloatFrac(ushort i) : this(new IntFloat(i)) { }
        public IntFloatFrac(uint i) : this(new IntFloat(i)) { }
        public IntFloatFrac(ulong i) : this(new IntFloat(i)) { }
        public IntFloatFrac(UBigInteger i) : this(new IntFloat(i)) { }
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
        public IntFloatFrac Increment() => this + 1;
        public IntFloatFrac Decrement() => this - 1;
        #endregion
        #region Comparison
        #region CompareTo
        public int CompareTo(BigInteger bi) => intFloatNotFraction ? ifloat.CompareTo(bi) : frac.CompareTo(bi);
        public int CompareTo(double d) => intFloatNotFraction ? ifloat.CompareTo(d) : frac.ToDouble().CompareTo(d);
        public int CompareTo(float f) => intFloatNotFraction ? ifloat.CompareTo(f) : frac.ToFloat().CompareTo(f);
        public int CompareTo(Doudec dd) => intFloatNotFraction ? ifloat.CompareTo(dd) : frac.ToDoudec().CompareTo(dd);
        public int CompareTo(IntFloat ift) => intFloatNotFraction ? ifloat.CompareTo(ift) : IntFloat.FromFraction(frac).CompareTo(ift);
        public int CompareTo(Fraction f) => Fraction.CompareTo(f);
        public int CompareTo(IntFloatFrac other) => other.intFloatNotFraction ? CompareTo(other.ifloat) : CompareTo(other.frac);
        #endregion

        #region Equals
        public bool Equals(BigInteger bi) => intFloatNotFraction ? ifloat.Equals(bi) : frac.Equals(bi);
        public bool Equals(double d) => intFloatNotFraction ? ifloat.Equals(d) : frac.Equals(d);
        public bool Equals(float f) => intFloatNotFraction ? ifloat.Equals(f) : frac.Equals(f);
        public bool Equals(Doudec dd) => intFloatNotFraction ? ifloat.Equals(dd) : frac.Equals(dd);
        public bool Equals(IntFloat ift) => intFloatNotFraction ? ifloat.Equals(ift) : frac.Equals(ift);
        public bool Equals(Fraction f) => Fraction.Equals(f);
        public bool Equals(IntFloatFrac other) => other.intFloatNotFraction ? Equals(other.ifloat) : Equals(other.frac);
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
        public static IntFloatFrac Add(IntFloatFrac left, IntFloatFrac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float + right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction + right.Fraction;
            return left.Int + right.Int;
        }
        public static IntFloatFrac Subtract(IntFloatFrac left, IntFloatFrac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float - right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction - right.Fraction;
            return left.Int - right.Int;
        }
        public static IntFloatFrac Multiply(IntFloatFrac left, IntFloatFrac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float * right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction * right.Fraction;
            return left.Int * right.Int;
        }
        public static IntFloatFrac Divide(IntFloatFrac left, IntFloatFrac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float / right.Float);
            if (left.IsFraction || right.IsFraction) return left.Fraction / right.Fraction;
            BigInteger l_i = left.Int;
            BigInteger r_i = right.Int;
            if (l_i % r_i == 0) return l_i / r_i;
            return left.Fraction / right.Fraction;
        }

        public static IntFloatFrac FloorDivide(IntFloatFrac left, IntFloatFrac right) => Floor(Divide(left, right));

        public static IntFloatFrac Remainder(IntFloatFrac left, IntFloatFrac right)
        {
            if (left.IsFloat || right.IsFloat) return new IntFloatFrac(left.Float % right.Float); ;
            if (left.IsFraction || right.IsFraction) return left.Fraction % right.Fraction;
            return left.Int % right.Int;
        }
        public static IntFloatFrac Pow(IntFloatFrac x, int y)
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

        public static IntFloatFrac Pow(IntFloatFrac x, double y) => x.intFloatNotFraction switch
        {
            true => new IntFloatFrac(IntFloat.Pow(x.ifloat, y)),
            false => new IntFloatFrac(Doudec.Pow((Doudec)x.frac, y))
        };

        public static IntFloatFrac Pow(IntFloatFrac x, IntFloat y)
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

        public static IntFloatFrac Abs(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Abs(x.ifloat)) : x.frac.Abs;
        public static IntFloatFrac Neg(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Negate(x.ifloat)) : x.frac.Negate();
        public static IntFloatFrac Floor(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Floor(x.ifloat)) : x.frac.Floor();
        public static IntFloatFrac Ceiling(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Ceiling(x.ifloat)) : x.frac.Ceiling();
        public static IntFloatFrac Truncate(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Truncate(x.ifloat)) : x.frac.Truncate();
        public static int Sign(IntFloatFrac iff) => iff.intFloatNotFraction ? IntFloat.Sign(iff.ifloat) : iff.frac.Sign;
        public static IntFloatFrac Log(IntFloatFrac iff) => Math.Log(iff.Float.Double);
        public static IntFloatFrac Log(IntFloatFrac iff, double newBase) => Math.Log(iff.Float.Double, newBase);
        public static IntFloatFrac Log10(IntFloatFrac iff) => Math.Log10(iff.Float.Double);
        public static IntFloatFrac Min(IntFloatFrac a, IntFloatFrac b) => a > b ? b : a;
        public static IntFloatFrac Max(IntFloatFrac a, IntFloatFrac b) => a < b ? b : a;
        #endregion

        #region Parse
        public static bool TryParse(string s, out IntFloatFrac result)
        {
            if (Fraction.TryParse(s, out Fraction f))
            {
                result = f;
                return true;
            }
            if (IntFloat.TryParse(s, out IntFloat i))
            {
                result = new IntFloatFrac(i);
                return true;
            }
            result = default;
            return false;
        }
        public static IntFloatFrac Parse(string s)
        {
            if (TryParse(s, out IntFloatFrac result))
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
        public static IntFloatFrac operator -(IntFloatFrac iff) => Neg(iff);
        public static IntFloatFrac operator +(IntFloatFrac iff) => iff;

        public static IntFloatFrac operator +(IntFloatFrac left, IntFloatFrac right) => Add(left, right);
        public static IntFloatFrac operator -(IntFloatFrac left, IntFloatFrac right) => Subtract(left, right);
        public static IntFloatFrac operator *(IntFloatFrac left, IntFloatFrac right) => Multiply(left, right);
        public static IntFloatFrac operator /(IntFloatFrac left, IntFloatFrac right) => Divide(left, right);
        public static IntFloatFrac operator %(IntFloatFrac left, IntFloatFrac right) => Remainder(left, right);

        public static IntFloatFrac operator ++(IntFloatFrac iff) => iff.Increment();
        public static IntFloatFrac operator --(IntFloatFrac iff) => iff.Decrement();

        public static bool operator ==(IntFloatFrac left, IntFloatFrac right) => left.Equals(right);
        public static bool operator !=(IntFloatFrac left, IntFloatFrac right) => !left.Equals(right);

        public static bool operator >(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) > 0;
        public static bool operator >=(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) >= 0;
        public static bool operator <(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) < 0;
        public static bool operator <=(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) <= 0;
        #endregion

        #region Casts
        #region from
        // integer -> IntFloatFrac
        public static implicit operator IntFloatFrac(sbyte sb) => new IntFloatFrac((BigInteger)sb);
        public static implicit operator IntFloatFrac(short s) => new IntFloatFrac((BigInteger)s);
        public static implicit operator IntFloatFrac(int i) => new IntFloatFrac((BigInteger)i);
        public static implicit operator IntFloatFrac(long l) => new IntFloatFrac((BigInteger)l);
        public static implicit operator IntFloatFrac(BigInteger bi) => new IntFloatFrac(bi);

        public static implicit operator IntFloatFrac(byte b) => new IntFloatFrac((BigInteger)b);
        public static implicit operator IntFloatFrac(ushort us) => new IntFloatFrac((BigInteger)us);
        public static implicit operator IntFloatFrac(uint ui) => new IntFloatFrac((BigInteger)ui);
        public static implicit operator IntFloatFrac(ulong ul) => new IntFloatFrac((BigInteger)ul);
        public static implicit operator IntFloatFrac(UBigInteger ubi) => new IntFloatFrac(ubi);

        // floating point -> IntFloatFrac
        public static implicit operator IntFloatFrac(float f) => FromDouble(f);
        public static implicit operator IntFloatFrac(double f) => FromDouble(f);
        public static implicit operator IntFloatFrac(decimal f) => FromDecimal(f);
        public static implicit operator IntFloatFrac(Doudec f) => FromDoudec(f);

        // Fraction -> IntFloatFrac
        public static implicit operator IntFloatFrac(Fraction f) => FromFraction(f);
        public static implicit operator IntFloatFrac(FractionOperation fo) => FromFraction(fo);

        // IntFloat -> IntFloatFrac
        public static implicit operator IntFloatFrac(IntFloat ift) => FromIntFloat(ift);
        #endregion

        #region to
        // IntFloatFrac -> integer
        public static explicit operator sbyte(IntFloatFrac iff) => (sbyte)iff.Int;
        public static explicit operator short(IntFloatFrac iff) => (short)iff.Int;
        public static explicit operator int(IntFloatFrac iff) => (int)iff.Int;
        public static explicit operator long(IntFloatFrac iff) => (long)iff.Int;
        public static explicit operator BigInteger(IntFloatFrac iff) => iff.Int;

        public static explicit operator byte(IntFloatFrac iff) => (byte)iff.Int;
        public static explicit operator ushort(IntFloatFrac iff) => (ushort)iff.Int;
        public static explicit operator uint(IntFloatFrac iff) => (uint)iff.Int;
        public static explicit operator ulong(IntFloatFrac iff) => (ulong)iff.Int;
        public static explicit operator UBigInteger(IntFloatFrac iff) => (UBigInteger)iff.Int;

        // IntFloatFrac -> floating point
        public static explicit operator float(IntFloatFrac iff) => (float)iff.Float;
        public static explicit operator double(IntFloatFrac iff) => (double)iff.Float;
        public static explicit operator decimal(IntFloatFrac iff) => (decimal)iff.Float;
        public static explicit operator Doudec(IntFloatFrac iff) => iff.Float;

        // IntFloatFrac -> Fraction
        public static explicit operator Fraction(IntFloatFrac iff) => iff.Fraction;

        // IntFloatFract -> IntFloat
        public static explicit operator IntFloat(IntFloatFrac iff) => iff.IntFloat;
        #endregion
        #endregion

        #region Conversion
        /// <summary>
        /// Conversion from double. Does not prioritize fraction.
        /// </summary>
        public static IntFloatFrac FromDouble(double d)
        {
            return new IntFloatFrac((Doudec)d);
        }

        /// <summary>
        /// Conversion that prioritizes Fraction.
        /// </summary>
        public static IntFloatFrac FromDecimal(decimal dec) => Fraction.FromDecimal(dec);

        /// <summary>
        /// Conversion that prioritizes Fraction.
        /// </summary>
        public static IntFloatFrac FromDoudec(Doudec d)
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
        public static IntFloatFrac FromIntFloat(IntFloat i)
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
        public static IntFloatFrac FromFraction(Fraction f)
        {
            if (f.IsWhole)
            {
                return f.ToBigInteger();
            }
            else
            {
                return new IntFloatFrac(f);
            }
        }
        #endregion
        #endregion

        #region public static Properties { get; }
        public static IntFloatFrac NaN => new IntFloatFrac(double.NaN);
        public static IntFloatFrac PositiveInfinity => new IntFloatFrac(double.PositiveInfinity);
        public static IntFloatFrac NegativeInfinity => new IntFloatFrac(double.NegativeInfinity);
        public static IntFloatFrac Epsilon => new IntFloatFrac(double.Epsilon);
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


    public static class IntFloatFracUtils
    {
        public static IntFloatFrac Sum(this IEnumerable<IntFloatFrac> items) => items.Aggregate((total, next) => total + next);
    }
}