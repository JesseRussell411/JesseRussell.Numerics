using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace JesseRussell.Numerics
{
    public readonly struct Bounded<T> : IComparable<Bounded<T>>, IComparable<T>, IEquatable<Bounded<T>>, IEquatable<T> where T: IComparable<T>, IEquatable<T>
    {
        #region public Properties
        public T Value { get; }
        public IBound Bound { get; }
        #endregion

        #region public Constructors
        public Bounded(T value, IBound bound)
        {
            Bound = bound;
            Value = bound.Bind(value);
        }
        #endregion

        #region public Methods
        public Bounded<T> Copy() => new Bounded<T>(Value, Bound.Copy());
        public Bounded<T> Copy(T value) => new Bounded<T>(value, Bound.Copy());
        public Bounded<T> Revalue(T value) => new Bounded<T>(value, Bound);
        #region Comparison
        public int CompareTo(T other) => Value.CompareTo(other);
        public int CompareTo(Bounded<T> other) => CompareTo(other.Value);
        #endregion

        #region Equality
        public bool Equals(T other) => Value.Equals(other);
        public bool Equals(Bounded<T> other) => Equals(other.Value);

        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object other) => Value.Equals(other);
        #endregion

        #region Math
        public Bounded<T> Add(T value) => new Bounded<T>((dynamic)Value + value, Bound);
        public Bounded<T> Subtract(T value) => new Bounded<T>((dynamic)Value - value, Bound);
        public Bounded<T> Multiply(T value) => new Bounded<T>((dynamic)Value * value, Bound);
        public Bounded<T> Divide(T value) => new Bounded<T>((dynamic)Value / value, Bound);
        public Bounded<T> Remainder(T value) => new Bounded<T>((dynamic)Value % value, Bound);
        #endregion

        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion

        #region public static Methods
        #region Factory Methods
        public static Bounded<T> LoopBounded(T value, T min, T max)
        {
            return new Bounded<T>(value, new LoopBound(min, max));
        }
        #endregion

        #region Casts
        public static implicit operator T(Bounded<T> self) => self.Value;
        #endregion

        #region Operators
        public static Bounded<T> operator +(Bounded<T> a, T b) => new Bounded<T>((dynamic)a.Value + b, a.Bound);

        public static Bounded<T> operator -(Bounded<T> a, Bounded<T> b) => new Bounded<T>((dynamic)a.Value - b.Value, a.Bound);
        public static Bounded<T> operator -(Bounded<T> a, T b) => new Bounded<T>((dynamic)a.Value - b, a.Bound);

        public static Bounded<T> operator *(Bounded<T> a, Bounded<T> b) => new Bounded<T>((dynamic)a.Value * b.Value, a.Bound);
        public static Bounded<T> operator *(Bounded<T> a, T b) => new Bounded<T>((dynamic)a.Value * b, a.Bound);

        public static Bounded<T> operator /(Bounded<T> a, Bounded<T> b) => new Bounded<T>((dynamic)a.Value / b.Value, a.Bound);
        public static Bounded<T> operator /(Bounded<T> a, T b) => new Bounded<T>((dynamic)a.Value / b, a.Bound);

        public static Bounded<T> operator %(Bounded<T> a, Bounded<T> b) => new Bounded<T>((dynamic)a.Value % b.Value, a.Bound);
        public static Bounded<T> operator %(Bounded<T> a, T b) => new Bounded<T>((dynamic)a.Value % b, a.Bound);

        public static Bounded<T> operator ++(Bounded<T> self) => (dynamic)self + 1;
        public static Bounded<T> operator --(Bounded<T> self) => (dynamic)self - 1;

        public static bool operator >(Bounded<T> a, Bounded<T> b) => a.CompareTo(b) > 0;
        public static bool operator >(Bounded<T> a, T b) => a.CompareTo(b) > 0;

        public static bool operator <(Bounded<T> a, Bounded<T> b) => a.CompareTo(b) < 0;
        public static bool operator <(Bounded<T> a, T b) => a.CompareTo(b) < 0;

        public static bool operator >=(Bounded<T> a, Bounded<T> b) => a.CompareTo(b) >= 0;
        public static bool operator >=(Bounded<T> a, T b) => a.CompareTo(b) >= 0;

        public static bool operator <=(Bounded<T> a, Bounded<T> b) => a.CompareTo(b) <= 0;
        public static bool operator <=(Bounded<T> a, T b) => a.CompareTo(b) <= 0;

        public static bool operator ==(Bounded<T> a, Bounded<T> b) => a.Equals(b);
        public static bool operator ==(Bounded<T> a, T b) => a.Equals(b);

        public static bool operator !=(Bounded<T> a, Bounded<T> b) => !a.Equals(b);
        public static bool operator !=(Bounded<T> a, T b) => !a.Equals(b);
        #endregion
        #endregion

        #region public internal Classes
        public interface IBound
        {
            T Bind(T vlaue);
            IBound Copy();
        }

        public class LoopBound : IBound
        {
            #region public Properties
            public T Min { get; set; }
            public T Max { get; set; }
            #endregion

            #region public Constructors
            public LoopBound(T min, T max)
            {
                Min = min; Max = max;
            }
            #endregion

            #region public Methods
            public T Bind(T value)
            {
                if (value.CompareTo(Min) < 0)
                {
                    return Max - (((dynamic)Min - value) % ((dynamic)Max - Min));
                }
                else if (value.CompareTo(Max) >= 0)
                {
                    return Min + (((dynamic)value - Max) % ((dynamic)Max - Min));
                }
                else
                {
                    return value;
                }
            }

            public IBound Copy() => new LoopBound(Min, Max);
            #endregion
        }
        #endregion
    }
}
