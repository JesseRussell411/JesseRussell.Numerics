using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using System.Runtime.InteropServices;


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
        public int CompareTo(double d) => intFloatNotFraction ? ifloat.CompareTo(d) : frac.CompareTo(d);
        public int CompareTo(float f) => intFloatNotFraction ? ifloat.CompareTo(f) : frac.CompareTo(f);
        public int CompareTo(Doudec dd) => intFloatNotFraction ? ifloat.CompareTo(dd) : frac.CompareTo(dd);
        public int CompareTo(IntFloat ift) => intFloatNotFraction ? ifloat.CompareTo(ift) : frac.CompareTo(ift);
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
                return Pow(x, y.Float.Double);
            }
        }

        public static IntFloatFrac Abs(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Abs(x.ifloat)) : Fraction.Abs(x.frac);
        public static IntFloatFrac Neg(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Negate(x.ifloat)) : x.frac.Negate();
        public static IntFloatFrac Floor(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Floor(x.ifloat)) : x.frac.Floor();
        public static IntFloatFrac Ceiling(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Ceiling(x.ifloat)) : x.frac.Ceiling();
        public static IntFloatFrac Truncate(IntFloatFrac x) => x.intFloatNotFraction ? new IntFloatFrac(IntFloat.Truncate(x.ifloat)) : x.frac.Truncate();
        public static int Sign(IntFloatFrac iff) => iff.intFloatNotFraction ? IntFloat.Sign(iff.ifloat) : iff.frac.Sign;
        public static IntFloatFrac Log(IntFloatFrac iff) => new IntFloatFrac(iff.intFloatNotFraction ? IntFloat.Log(iff.ifloat) : Fraction.Log(iff.frac));
        public static IntFloatFrac Log(IntFloatFrac iff, double newBase) => new IntFloatFrac(iff.intFloatNotFraction ? IntFloat.Log(iff.ifloat, newBase) : Fraction.Log(iff.frac, newBase));
        public static IntFloatFrac Log10(IntFloatFrac iff) => new IntFloatFrac(iff.intFloatNotFraction ? IntFloat.Log10(iff.ifloat) : Fraction.Log10(iff.frac));
        public static IntFloatFrac Min(IntFloatFrac a, IntFloatFrac b) => a > b ? b : a;
        public static IntFloatFrac Max(IntFloatFrac a, IntFloatFrac b) => a < b ? b : a;
        #endregion

        #region Parse
        public static bool TryParse(string s, out IntFloatFrac result)
        {
            if (BigInteger.TryParse(s, out BigInteger bi))
            {
                result = bi;
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
        public static explicit operator FractionOperation(IntFloatFrac iff) => iff.Fraction;

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


    // old:
    //public readonly struct IntFloatFrac : IComparable<BigInteger>, IComparable<double>, IComparable<float>, IComparable<Doudec>, IComparable<IntFloat>, IComparable<Fraction>, IComparable<IntFloatFrac>, IEquatable<BigInteger>, IEquatable<double>, IEquatable<float>, IEquatable<Doudec>, IEquatable<IntFloat>, IEquatable<Fraction>, IEquatable<IntFloatFrac>
    //{
    //    #region public Constructors
    //    public IntFloatFrac(IntFloat value)
    //    {
    //        intFloatNotFraction = true;
    //        frac = default;
    //        ifloat = value;
    //    }
    //    public IntFloatFrac(Fraction value)
    //    {
    //        intFloatNotFraction = false;
    //        ifloat = default;
    //        frac = value;
    //    }

    //    public IntFloatFrac(sbyte i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(short i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(int i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(long i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(BigInteger i) : this(new IntFloat(i)) { }

    //    public IntFloatFrac(byte i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(ushort i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(uint i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(ulong i) : this(new IntFloat(i)) { }
    //    public IntFloatFrac(UBigInteger i) : this(new IntFloat(i)) { }
    //    #endregion

    //    #region public Properties { get; }
    //    public BigInteger Int => intFloatNotFraction ? ifloat.Int : frac.ToBigInteger();
    //    public Doudec Float => intFloatNotFraction ? ifloat.Float : frac.ToDoudec();
    //    public IntFloat IntFloat => intFloatNotFraction ? ifloat : (IntFloat)frac;
    //    public Fraction Fraction
    //    {
    //        get
    //        {
    //            if (intFloatNotFraction)
    //            {
    //                if (ifloat.IsInt)
    //                {
    //                    return new Fraction(ifloat.Int);
    //                }
    //                else
    //                {
    //                    return Fraction.FromDoudec(ifloat.Float);
    //                }
    //            }
    //            else
    //            {
    //                return frac;
    //            }
    //        }
    //    }

    //    public bool IsInt => intFloatNotFraction && ifloat.IsInt;
    //    public bool IsFloat => intFloatNotFraction && ifloat.IsFloat;
    //    public bool IsFraction => !intFloatNotFraction;
    //    public bool IsIntFloat => intFloatNotFraction;
    //    public object Value => intFloatNotFraction ? ifloat.Value : frac;

    //    public bool IsNegative => intFloatNotFraction ? ifloat.IsNegative : frac.IsNegative;
    //    public bool IsZero => intFloatNotFraction ? ifloat.IsZero : frac.IsZero;
    //    #endregion

    //    #region public Methods
    //    #region Math
    //    public IntFloatFrac Increment() => this + 1;
    //    public IntFloatFrac Decrement() => this - 1;
    //    #endregion
    //    #region Comparison
    //    #region CompareTo
    //    public int CompareTo(BigInteger bi) => intFloatNotFraction ? ifloat.CompareTo(bi) : frac.CompareTo(bi);
    //    public int CompareTo(double d) => intFloatNotFraction ? ifloat.CompareTo(d) : frac.CompareTo(d);
    //    public int CompareTo(float f) => intFloatNotFraction ? ifloat.CompareTo(f) : frac.CompareTo(f);
    //    public int CompareTo(Doudec dd) => intFloatNotFraction ? ifloat.CompareTo(dd) : frac.CompareTo(dd);
    //    public int CompareTo(IntFloat ift) => intFloatNotFraction ? ifloat.CompareTo(ift) : frac.CompareTo(ift);
    //    public int CompareTo(Fraction f) => Fraction.CompareTo(f);
    //    public int CompareTo(IntFloatFrac other) => other.intFloatNotFraction ? CompareTo(other.ifloat) : CompareTo(other.frac);
    //    #endregion

    //    #region Equals
    //    public bool Equals(BigInteger bi) => intFloatNotFraction ? ifloat.Equals(bi) : frac.Equals(bi);
    //    public bool Equals(double d) => intFloatNotFraction ? ifloat.Equals(d) : frac.Equals(d);
    //    public bool Equals(float f) => intFloatNotFraction ? ifloat.Equals(f) : frac.Equals(f);
    //    public bool Equals(Doudec dd) => intFloatNotFraction ? ifloat.Equals(dd) : frac.Equals(dd);
    //    public bool Equals(IntFloat ift) => intFloatNotFraction ? ifloat.Equals(ift) : frac.Equals(ift);
    //    public bool Equals(Fraction f) => Fraction.Equals(f);
    //    public bool Equals(IntFloatFrac other) => other.intFloatNotFraction ? Equals(other.ifloat) : Equals(other.frac);
    //    public override bool Equals(object obj) => obj switch
    //    {
    //        sbyte i => Equals((BigInteger)i),
    //        short i => Equals((BigInteger)i),
    //        int i => Equals((BigInteger)i),
    //        long i => Equals((BigInteger)i),
    //        BigInteger i => Equals((BigInteger)i),

    //        byte i => Equals((BigInteger)i),
    //        ushort i => Equals((BigInteger)i),
    //        uint i => Equals((BigInteger)i),
    //        ulong i => Equals((BigInteger)i),
    //        UBigInteger i => Equals((BigInteger)i),

    //        double f => Equals(f),
    //        float f => Equals(f),
    //        Doudec f => Equals(f),
    //        Fraction f => Equals(f),

    //        IntFloat ift => Equals(ift),
    //        _ => throw new ArgumentException("The parameter must be an integer, floating point number, or fraction")
    //    };
    //    #endregion
    //    public override int GetHashCode() => intFloatNotFraction ? ifloat.GetHashCode() : frac.GetHashCode();
    //    #endregion
    //    public override string ToString() => intFloatNotFraction ? ifloat.ToString() : frac.ToString();
    //    #endregion

    //    #region public static Methods
    //    #region Math
    //    public static IntFloatFrac Add(IntFloatFrac left, IntFloatFrac right)
    //    {
    //        if (left.IsFloat || right.IsFloat) return left.Float + right.Float;
    //        if (left.IsFraction || right.IsFraction) return left.Fraction + right.Fraction;
    //        return left.Int + right.Int;
    //    }
    //    public static IntFloatFrac Subtract(IntFloatFrac left, IntFloatFrac right)
    //    {
    //        if (left.IsFloat || right.IsFloat) return left.Float - right.Float;
    //        if (left.IsFraction || right.IsFraction) return left.Fraction - right.Fraction;
    //        return left.Int - right.Int;
    //    }
    //    public static IntFloatFrac Multiply(IntFloatFrac left, IntFloatFrac right)
    //    {
    //        if (left.IsFloat || right.IsFloat) return left.Float * right.Float;
    //        if (left.IsFraction || right.IsFraction) return left.Fraction * right.Fraction;
    //        return left.Int * right.Int;
    //    }
    //    public static IntFloatFrac Divide(IntFloatFrac left, IntFloatFrac right)
    //    {
    //        if (left.IsFloat || right.IsFloat) return left.Float / right.Float;
    //        if (left.IsFraction || right.IsFraction) return left.Fraction / right.Fraction;
    //        BigInteger l_i = left.Int;
    //        BigInteger r_i = right.Int;
    //        if (l_i % r_i == 0) return l_i / r_i;
    //        return left.Fraction / right.Fraction;
    //    }
    //    public static IntFloatFrac FloorDivide(IntFloatFrac left, IntFloatFrac right)
    //    {
    //        if (left.IsFloat || right.IsFloat) return Doudec.Floor(left.Float / right.Float);
    //        if (left.IsFraction || right.IsFraction) return (left.Fraction / right.Fraction).Floor();
    //        return left.Int / right.Int;
    //    }

    //    public static IntFloatFrac Remainder(IntFloatFrac left, IntFloatFrac right)
    //    {
    //        if (left.IsFloat || right.IsFloat) return left.Float % right.Float;
    //        if (left.IsFraction || right.IsFraction) return left.Fraction % right.Fraction;
    //        return left.Int % right.Int;
    //    }
    //    public static IntFloatFrac Pow(IntFloatFrac x, int y)
    //    {
    //        if (y < 0 && (x.IsInt || x.IsFraction))
    //        {
    //            return x.Fraction.Power(y);
    //        }
    //        else if (x.intFloatNotFraction)
    //        {
    //            return IntFloat.Pow(x.ifloat, y);
    //        }
    //        else
    //        {
    //            return x.frac.Power(y);
    //        }
    //    }
    //    public static IntFloatFrac Pow(IntFloatFrac x, double y) => x.intFloatNotFraction ? 
    //        IntFloat.Pow(x.ifloat, y) : 
    //        Doudec.Pow((Doudec)x.frac, y);


    //    public static IntFloatFrac Pow(IntFloatFrac x, IntFloat y)
    //    {
    //        if (y.IsInt)
    //        {
    //            return Pow(x, (int)y);
    //        }
    //        else
    //        {
    //            return Pow(x, (double)y);
    //        }
    //    }

    //    public static IntFloatFrac Abs(IntFloatFrac x) => x.intFloatNotFraction ? (IntFloatFrac)IntFloat.Abs(x.ifloat) : Fraction.Abs(x.frac);
    //    public static IntFloatFrac Neg(IntFloatFrac x) => x.intFloatNotFraction ? (IntFloatFrac)IntFloat.Negate(x.ifloat) : x.frac.Negate();
    //    public static IntFloatFrac Floor(IntFloatFrac x) => x.intFloatNotFraction ? (IntFloatFrac)IntFloat.Floor(x.ifloat) : x.frac.Floor();
    //    public static IntFloatFrac Ceiling(IntFloatFrac x) => x.intFloatNotFraction ? (IntFloatFrac)IntFloat.Ceiling(x.ifloat) : x.frac.Ceiling();
    //    public static IntFloatFrac Truncate(IntFloatFrac x) => x.intFloatNotFraction ? (IntFloatFrac)IntFloat.Truncate(x.ifloat) : x.frac.Truncate();
    //    public static int Sign(IntFloatFrac iff) => iff.intFloatNotFraction ? IntFloat.Sign(iff.ifloat) : iff.frac.Sign;
    //    public static IntFloatFrac Log(IntFloatFrac iff) => iff.intFloatNotFraction ? IntFloat.Log(iff.ifloat) : Fraction.Log(iff.frac);
    //    public static IntFloatFrac Log(IntFloatFrac iff, double newBase) => iff.intFloatNotFraction ? IntFloat.Log(iff.ifloat, newBase) : Fraction.Log(iff.frac, newBase);
    //    public static IntFloatFrac Log10(IntFloatFrac iff) => iff.intFloatNotFraction ? IntFloat.Log10(iff.ifloat) : Fraction.Log10(iff.frac);
    //    public static IntFloatFrac Min(IntFloatFrac a, IntFloatFrac b) => a < b ? a : b;
    //    public static IntFloatFrac Max(IntFloatFrac a, IntFloatFrac b) => a > b ? a : b;
    //    #endregion

    //    #region Parse
    //    public static bool TryParse(string s, out IntFloatFrac result)
    //    {
    //        if (IntFloat.TryParse(s, out IntFloat ift))
    //        {
    //            result = ift;
    //            return true;
    //        }
    //        else if (Fraction.TryParse(s, out Fraction f))
    //        {
    //            result = f;
    //            return true;
    //        }
    //        else
    //        {
    //            result = default;
    //            return false;
    //        }
    //    }
    //    public static IntFloatFrac Parse(string s)
    //    {
    //        if (TryParse(s, out IntFloatFrac result))
    //        {
    //            return result;
    //        }
    //        else
    //        {
    //            throw new FormatException("The string is not a valid integer, floating point number, or fraction.");
    //        }
    //    }
    //    #endregion

    //    #region Operators
    //    public static IntFloatFrac operator -(IntFloatFrac iff) => Neg(iff);
    //    public static IntFloatFrac operator +(IntFloatFrac iff) => iff;

    //    public static IntFloatFrac operator +(IntFloatFrac left, IntFloatFrac right) => Add(left, right);
    //    public static IntFloatFrac operator -(IntFloatFrac left, IntFloatFrac right) => Subtract(left, right);
    //    public static IntFloatFrac operator *(IntFloatFrac left, IntFloatFrac right) => Multiply(left, right);
    //    public static IntFloatFrac operator /(IntFloatFrac left, IntFloatFrac right) => Divide(left, right);
    //    public static IntFloatFrac operator %(IntFloatFrac left, IntFloatFrac right) => Remainder(left, right);

    //    public static IntFloatFrac operator ++(IntFloatFrac iff) => iff.Increment();
    //    public static IntFloatFrac operator --(IntFloatFrac iff) => iff.Decrement();

    //    public static bool operator ==(IntFloatFrac left, IntFloatFrac right) => left.Equals(right);
    //    public static bool operator !=(IntFloatFrac left, IntFloatFrac right) => !left.Equals(right);

    //    public static bool operator >(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) > 0;
    //    public static bool operator >=(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) >= 0;
    //    public static bool operator <(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) < 0;
    //    public static bool operator <=(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) <= 0;
    //    #endregion

    //    #region Casts
    //    public static implicit operator IntFloatFrac(BigInteger bi) => new IntFloatFrac(bi);
    //    public static implicit operator IntFloatFrac(Doudec d) => new IntFloatFrac(d);
    //    public static implicit operator IntFloatFrac(double d) => new IntFloatFrac(d);
    //    public static implicit operator IntFloatFrac(Fraction f) => new IntFloatFrac(f);
    //    public static implicit operator IntFloatFrac(FractionOperation fo) => new IntFloatFrac(fo);

    //    public static implicit operator IntFloatFrac(sbyte sb) => new IntFloatFrac((BigInteger)sb);
    //    public static implicit operator IntFloatFrac(short s) => new IntFloatFrac((BigInteger)s);
    //    public static implicit operator IntFloatFrac(int i) => new IntFloatFrac((BigInteger)i);
    //    public static implicit operator IntFloatFrac(long l) => new IntFloatFrac((BigInteger)l);

    //    public static implicit operator IntFloatFrac(byte b) => new IntFloatFrac((BigInteger)b);
    //    public static implicit operator IntFloatFrac(ushort us) => new IntFloatFrac((BigInteger)us);
    //    public static implicit operator IntFloatFrac(uint ui) => new IntFloatFrac((BigInteger)ui);
    //    public static implicit operator IntFloatFrac(ulong ul) => new IntFloatFrac((BigInteger)ul);

    //    public static implicit operator sbyte(IntFloatFrac iff) => (sbyte)iff.Int;
    //    public static implicit operator short(IntFloatFrac iff) => (short)iff.Int;
    //    public static implicit operator int(IntFloatFrac iff) => (int)iff.Int;
    //    public static implicit operator long(IntFloatFrac iff) => (long)iff.Int;
    //    public static explicit operator BigInteger(IntFloatFrac iff) => iff.Int;

    //    public static implicit operator byte(IntFloatFrac iff) => (byte)iff.Int;
    //    public static implicit operator ushort(IntFloatFrac iff) => (ushort)iff.Int;
    //    public static implicit operator uint(IntFloatFrac iff) => (uint)iff.Int;
    //    public static implicit operator ulong(IntFloatFrac iff) => (ulong)iff.Int;
    //    public static implicit operator UBigInteger(IntFloatFrac iff) => (UBigInteger)iff.Int;

    //    public static explicit operator Doudec(IntFloatFrac iff) => iff.Float;
    //    public static explicit operator Fraction(IntFloatFrac iff) => iff.Fraction;

    //    public static explicit operator double(IntFloatFrac iff) => (double)iff.Float;
    //    public static explicit operator float(IntFloatFrac iff) => (float)iff.Float;
    //    public static explicit operator decimal(IntFloatFrac iff) => (decimal)iff.Float;

    //    public static implicit operator IntFloatFrac(IntFloat ift) => ift.Value switch { BigInteger bi => bi, Doudec d => d, _ => default };
    //    public static explicit operator IntFloat(IntFloatFrac iff) => iff.intFloatNotFraction ? iff.ifloat : iff.IntFloat;
    //    #endregion
    //    #endregion

    //    #region public static Properties { get; }
    //    public static IntFloatFrac NaN => new IntFloatFrac(double.NaN);
    //    public static IntFloatFrac PositiveInfinity => new IntFloatFrac(double.PositiveInfinity);
    //    public static IntFloatFrac NegativeInfinity => new IntFloatFrac(double.NegativeInfinity);
    //    public static IntFloatFrac Epsilon => new IntFloatFrac(double.Epsilon);
    //    #endregion

    //    #region private readonly Fields
    //    private readonly bool intFloatNotFraction;
    //    private readonly IntFloat ifloat;
    //    private readonly Fraction frac;
    //    #endregion
    //}
}



//namespace MathTypes
//{
//    public struct IntFloatFrac : IComparable<BigInteger>, IComparable<Doudec>, IComparable<Fraction>
//    {
//        #region public readonly Fields
//        public readonly object Value;
//        #endregion

//        #region public derived Properties
//        public int Sign => Value switch
//        {
//            BigInteger bi => bi.Sign,
//            Doudec d => Math.Sign(d),
//            Fraction f => f.Sign,
//            _ => throw NotValid
//        };
//        public bool IsInt => Value is BigInteger;
//        public bool IsFloat => Value is Doudec;
//        public bool IsFraction => Value is Fraction;
//        public Type Type => Value.GetType();
//        public BigInteger Int => Value switch
//        {
//            BigInteger bi => bi,
//            Doudec d => (BigInteger)d,
//            Fraction f => (BigInteger)f,
//            _ => throw NotValid
//        };

//        public Doudec Float => Value switch
//        {
//            BigInteger bi => (Doudec)bi,
//            Doudec d => d,
//            Fraction f => f.ToDoudec(),
//            _ => throw NotValid
//        };

//        public Fraction Fraction => Value switch
//        {
//            BigInteger bi => bi,
//            Doudec d => Fraction.FromDoudec(d),
//            Fraction f => f,
//            _ => throw NotValid
//        };
//        #endregion

//        #region public Constructors
//        public IntFloatFrac(BigInteger value)
//        {
//            Value = value;
//        }

//        public IntFloatFrac(Doudec value)
//        {
//            Value = value;
//        }

//        public IntFloatFrac(Fraction value)
//        {
//            Value = value;
//        }
//        #endregion

//        #region public Methods
//        #region Comparison
//        public int CompareTo(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => CompareTo(bi),
//            Doudec d => CompareTo(d),
//            Fraction f => CompareTo(f),
//            _ => throw NotValid
//        };

//        public int CompareTo(BigInteger bi) => Int.CompareTo(bi);
//        public int CompareTo(Doudec d) => Float.CompareTo(d);
//        public int CompareTo(Fraction f) => Fraction.CompareTo(f);

//        public bool Equals(BigInteger bi)
//        {
//            if (Value is BigInteger self) return self.Equals(bi);
//            else if (Value is Fraction f) return f.Equals(bi);
//            else if (Value is Doudec d) return d % 1 == 0 && new BigInteger(d).Equals(bi);
//            else return false;
//        }
//        public bool Equals(Doudec d)
//        {
//            if (Value is Doudec self) return self.Equals(d);
//            else if (Value is Fraction f) return f.ToDoudec().Equals(d);
//            else if (Value is BigInteger bi) return d % 1 == 0 && bi.Equals(new BigInteger(d));
//            else return false;
//        }
//        public bool Equals(Fraction f)
//        {
//            if (Value is Fraction self) return self.Equals(f);
//            else if (Value is BigInteger bi) return new Fraction(bi).Equals(f);
//            else if (Value is Doudec d) return f.ToDoudec().Equals(d);
//            else return false;
//        }

//        public bool Equals(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => Equals(bi),
//            Doudec d => Equals(d),
//            Fraction f => Equals(f),
//            _ => throw NotValid
//        };

//        public override bool Equals(Object obj) => obj switch
//        {
//            BigInteger bi => Equals(bi),
//            Doudec d => Equals(d),
//            Fraction f => Equals(f),
//            float f => Equals((Doudec)f),

//            sbyte sb => Equals((BigInteger)sb),
//            short s => Equals((BigInteger)s),
//            int i => Equals((BigInteger)i),
//            long l => Equals((BigInteger)l),

//            byte b => Equals((BigInteger)b),
//            ushort us => Equals((BigInteger)us),
//            uint ui => Equals((BigInteger)ui),
//            ulong ul => Equals((BigInteger)ul),
//            UBigInteger ubig => Equals((BigInteger)ubig),
//            _ => throw new ArgumentException("Parameter must be a fraction, integer, Doudec, or float.")
//        };
//        public override int GetHashCode() => Value.GetHashCode();
//        #endregion
//        public override string ToString() => Value.ToString();
//        #endregion

//        #region public static Methods
//        #region Parse
//        public static bool TryParse(string s, out IntFloatFrac result)
//        {
//            BigInteger bi;
//            Doudec d;
//            Fraction f;
//            if (BigInteger.TryParse(s, out bi))
//            {
//                result = bi;
//                return true;
//            }
//            else if (Doudec.TryParse(s, out d)){
//                result = d;
//                return true;
//            }
//            else if (Fraction.TryParse(s, out f))
//            {
//                result = f;
//                return true;
//            }
//            else
//            {
//                result = Default;
//                return false;
//            }
//        }

//        public static IntFloatFrac Parse(string s)
//        {
//            if (TryParse(s, out IntFloatFrac result))
//            {
//                return result;
//            }
//            else
//            {
//                throw new FormatException("The string is not a valid BigInteger, Doudec, or Fraction.");
//            }
//        }
//        #endregion
//        #region Doudec Pass Through Methods
//        public bool IsPositiveInfinity(IntFloatFrac iff) => iff.Value is Doudec d && Doudec.IsPositiveInfinity(d);
//        public bool IsNegativeInfinity(IntFloatFrac iff) => iff.Value is Doudec d && Doudec.IsNegativeInfinity(d);
//        public bool IsFinite(IntFloatFrac iff) => iff.Value is BigInteger || iff.Value is Fraction || (iff.Value is Doudec d && Doudec.IsFinite(d));
//        public bool IsInfinity(IntFloatFrac iff) => iff.Value is Doudec d && Doudec.IsInfinity(d);
//        public bool IsNegative(IntFloatFrac iff) => iff.Value is Doudec d && Doudec.IsNegative(d);
//        public bool IsNaN(IntFloatFrac iff) => iff.Value is Doudec d && Doudec.IsNaN(d);
//        #endregion

//        #region Math
//        public static IntFloatFrac Add(IntFloatFrac left, IntFloatFrac right)
//        {
//            if (left.IsFloat || right.IsFloat) return left.Float + right.Float;
//            if (left.IsFraction || right.IsFraction) return left.Fraction + right.Fraction;
//            return left.Int + right.Int;
//        }
//        public static IntFloatFrac Subtract(IntFloatFrac left, IntFloatFrac right)
//        {
//            if (left.IsFloat || right.IsFloat) return left.Float - right.Float;
//            if (left.IsFraction || right.IsFraction) return left.Fraction - right.Fraction;
//            return left.Int - right.Int;
//        }
//        public static IntFloatFrac Multiply(IntFloatFrac left, IntFloatFrac right)
//        {
//            if (left.IsFloat || right.IsFloat) return left.Float * right.Float;
//            if (left.IsFraction || right.IsFraction) return left.Fraction * right.Fraction;
//            return left.Int * right.Int;
//        }
//        public static IntFloatFrac Divide(IntFloatFrac left, IntFloatFrac right)
//        {
//            if (left.IsFloat || right.IsFloat) return left.Float / right.Float;
//            if (left.IsFraction || right.IsFraction) return left.Fraction / right.Fraction;
//            return left.Fraction / right.Fraction;
//        }
//        public static IntFloatFrac FloorDivide(IntFloatFrac left, IntFloatFrac right)
//        {
//            if (left.IsFloat || right.IsFloat) return Math.Floor(left.Float / right.Float);
//            if (left.IsFraction || right.IsFraction) return left.Fraction.Divide(right.Fraction).Floor();
//            return left.Int / right.Int;
//        }
//        public static IntFloatFrac Remainder(IntFloatFrac left, IntFloatFrac right)
//        {
//            if (left.IsFloat || right.IsFloat) return left.Float % right.Float;
//            if (left.IsFraction || right.IsFraction) return left.Fraction % right.Fraction;
//            return left.Int % right.Int;
//        }
//        public static IntFloatFrac Increment(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => bi + 1,
//            Doudec d => d + 1.0,
//            Fraction f => f.Increment(),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Decrement(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => bi - 1,
//            Doudec d => d - 1.0,
//            Fraction f => f.Decrement(),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Pow(IntFloatFrac iff, int e) => iff.Value switch
//        {
//            BigInteger bi => BigInteger.Pow(bi, e),
//            Doudec d => Math.Pow(d, e),
//            Fraction f => Fraction.Pow(f, e),
//            _ => throw NotValid
//        };
//        public static Doudec Pow(IntFloatFrac x, IntFloatFrac y) => Math.Pow(x.Float, y.Float);
//        public static IntFloatFrac Neg(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => -bi,
//            Doudec d => -d,
//            Fraction f => -f,
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Floor(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => bi,
//            Doudec d => Math.Floor(d),
//            Fraction f => f.Floor().ToBigInteger(),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Ceiling(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => bi,
//            Doudec d => Math.Ceiling(d),
//            Fraction f => f.Ceiling().ToBigInteger(),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Truncate(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => bi,
//            Doudec d => Math.Truncate(d),
//            Fraction f => f.ToBigInteger(),
//            _ => throw NotValid
//        };

//        public static IntFloatFrac Log(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => BigInteger.Log(bi),
//            Doudec d => Math.Log(d),
//            Fraction f => Math.Log(f.ToDoudec()),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Log(IntFloatFrac iff, Doudec newBase) => iff.Value switch
//        {
//            BigInteger bi => BigInteger.Log(bi, newBase),
//            Doudec d => Math.Log(d, newBase),
//            Fraction f => Math.Log(f.ToDoudec(), newBase),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Log10(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => BigInteger.Log10(bi),
//            Doudec d => Math.Log10(d),
//            Fraction f => Fraction.Log10(f),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Log2(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => BigInteger.Log(bi, 2),
//            Doudec d => Math.Log2(d),
//            Fraction f => Fraction.Log2(f),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Abs(IntFloatFrac iff) => iff.Value switch
//        {
//            BigInteger bi => BigInteger.Abs(bi),
//            Doudec d => Math.Abs(d),
//            Fraction f => Fraction.Abs(f),
//            _ => throw NotValid
//        };
//        public static IntFloatFrac Min(IntFloatFrac a, IntFloatFrac b) => a < b ? a : b;
//        public static IntFloatFrac Max(IntFloatFrac a, IntFloatFrac b) => a > b ? a : b;
//        #endregion

//        #region Operators
//        public static IntFloatFrac operator +(IntFloatFrac iff) => iff;
//        public static IntFloatFrac operator -(IntFloatFrac iff) => Neg(iff);

//        public static IntFloatFrac operator +(IntFloatFrac left, IntFloatFrac right) => Add(left, right);
//        public static IntFloatFrac operator -(IntFloatFrac left, IntFloatFrac right) => Subtract(left, right);
//        public static IntFloatFrac operator *(IntFloatFrac left, IntFloatFrac right) => Multiply(left, right);
//        public static IntFloatFrac operator /(IntFloatFrac left, IntFloatFrac right) => Divide(left, right);
//        public static IntFloatFrac operator %(IntFloatFrac left, IntFloatFrac right) => Divide(left, right);

//        public static bool operator ==(IntFloatFrac left, IntFloatFrac right) => left.Equals(right);
//        public static bool operator !=(IntFloatFrac left, IntFloatFrac right) => !left.Equals(right);

//        public static bool operator >(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) > 0;
//        public static bool operator >=(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) >= 0;
//        public static bool operator <(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) < 0;
//        public static bool operator <=(IntFloatFrac left, IntFloatFrac right) => left.CompareTo(right) <= 0;

//        #endregion

//        #region Casts
//        public static implicit operator IntFloatFrac(BigInteger bi) => new IntFloatFrac(bi);
//        public static implicit operator IntFloatFrac(Doudec d) => new IntFloatFrac(d);
//        public static implicit operator IntFloatFrac(double d) => new IntFloatFrac(d);
//        public static implicit operator IntFloatFrac(Fraction f) => new IntFloatFrac(f);
//        public static implicit operator IntFloatFrac(FractionOperation fo) => new IntFloatFrac(fo);

//        public static implicit operator IntFloatFrac(sbyte sb) => new IntFloatFrac((BigInteger) sb);
//        public static implicit operator IntFloatFrac(short s) => new IntFloatFrac((BigInteger) s);
//        public static implicit operator IntFloatFrac(int i) => new IntFloatFrac((BigInteger) i);
//        public static implicit operator IntFloatFrac(long l) => new IntFloatFrac((BigInteger) l);

//        public static implicit operator IntFloatFrac(byte b) => new IntFloatFrac((BigInteger) b);
//        public static implicit operator IntFloatFrac(ushort us) => new IntFloatFrac((BigInteger) us);
//        public static implicit operator IntFloatFrac(uint ui) => new IntFloatFrac((BigInteger) ui);
//        public static implicit operator IntFloatFrac(ulong ul) => new IntFloatFrac((BigInteger) ul);


//        public static explicit operator BigInteger(IntFloatFrac iff) => iff.Int;
//        public static implicit operator Doudec(IntFloatFrac iff) => iff.Float;
//        public static explicit operator Fraction(IntFloatFrac iff) => iff.Fraction;

//        public static implicit operator IntFloatFrac(IntFloat ift) => ift.Value switch { BigInteger bi => bi, Doudec d => d, _ => Default };
//        public static explicit operator IntFloat(IntFloatFrac iff) => iff.Value switch { BigInteger bi => bi, Doudec d => d, Fraction f => f.ToDoudec(), _ => throw NotValid };
//        #endregion
//        #endregion

//        #region private static Properties
//        private static Exception NotValid => new Exception("Value is not valid because it is neither a BigInteger, Doudec, nor fraction. If you encounter this exception, and you didn't use the parameterless constructor, please contact the developer of this type.");
//        #endregion

//        #region public static readonly Fields
//        public static readonly IntFloatFrac Default = 0.0;
//        #endregion

//        #region public static get only Properties
//        public static IntFloatFrac PositiveInfinity { get => Doudec.PositiveInfinity; }
//        public static IntFloatFrac NegativeInfinity { get => Doudec.NegativeInfinity; }
//        public static IntFloatFrac NaN { get => Doudec.NaN; }
//        public static IntFloatFrac Epsilon { get => Doudec.Epsilon; }
//        #endregion

//    }
//}
