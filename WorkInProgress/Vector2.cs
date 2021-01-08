using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using JesseRussell.Numerics;

namespace JesseRussell.Numerics.WorkInProgress
{
    public readonly struct Vector2
    {
        #region public Constructors
        public Vector2(IntFloatFrac x, IntFloatFrac y)
        {
            X = x;
            Y = y;
        }
        #endregion

        #region public readonly Fields
        public readonly IntFloatFrac X;
        public readonly IntFloatFrac Y;
        #endregion

        #region public Properties
        public IntFloatFrac MagnitudeSquared => X * X + Y * Y;
        public IntFloatFrac Magnitude => Math.Sqrt(MagnitudeSquared.Float.Double);
        #endregion

        #region public Methods
        public bool Equals(Vector2 other) => X == other.Y && Y == other.Y;
        public override bool Equals(object obj)
        {
            if (obj is Vector2 v)
                return Equals(v);
            else
                throw new ArgumentException("The object given was not a Vector2");
        }
        public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
        public Vector2 Abs() => new Vector2(IntFloatFrac.Abs(X), IntFloatFrac.Abs(Y));
        public Vector2 Add(Vector2 other) => new Vector2(X + other.X, Y + other.Y);
        public Vector2 Subtract(Vector2 other) => new Vector2(X - other.X, Y - other.Y);
        public Vector2 Multiply(IntFloatFrac scaler) => new Vector2(X * scaler, Y * scaler);
        public Vector2 Divide(IntFloatFrac scaler) => new Vector2(X / scaler, Y / scaler);
        public Vector2 DivideFrom(IntFloatFrac scaler) => new Vector2(scaler / X, scaler / Y);
        public Vector2 Transform(Vector2 I, Vector2 J) => I.Multiply(X).Add(J.Multiply(Y));
        public Vector2 Normalized()
        {
            IntFloatFrac magnitude = Magnitude;
            return new Vector2(X * magnitude, Y * magnitude);
        }

        public Vector2 PlusX(Vector2 other) => new Vector2(X + other.X, Y);
        public Vector2 PlusX(IntFloatFrac x) => new Vector2(X + x, Y);
        public Vector2 PlusY(Vector2 other) => new Vector2(X, Y + other.Y);
        public Vector2 PlusY(IntFloatFrac y) => new Vector2(X, Y + y);
        public Vector2 MinusX(Vector2 other) => new Vector2(X - other.X, Y);
        public Vector2 MinusX(IntFloatFrac x) => new Vector2(X - x, Y);
        public Vector2 MinusY(Vector2 other) => new Vector2(X, Y - other.Y);
        public Vector2 MinusY(IntFloatFrac y) => new Vector2(X, Y - y);
        public Vector2 TimesX(Vector2 other) => new Vector2(X * other.X, Y);
        public Vector2 TimesX(IntFloatFrac x) => new Vector2(X * x, Y);
        public Vector2 TimesY(Vector2 other) => new Vector2(X, Y * other.Y);
        public Vector2 TimesY(IntFloatFrac y) => new Vector2(X, Y * y);
        public Vector2 DivbyX(Vector2 other) => new Vector2(X / other.X, Y);
        public Vector2 DivbyX(IntFloatFrac x) => new Vector2(X / x, Y);
        public Vector2 DivbyY(Vector2 other) => new Vector2(X, Y / other.Y);
        public Vector2 DivbyY(IntFloatFrac y) => new Vector2(X, Y / y);
        public Vector2 WithX(Vector2 other) => new Vector2(other.X, Y);
        public Vector2 WithX(IntFloatFrac x) => new Vector2(x, Y);
        public Vector2 WithY(Vector2 other) => new Vector2(X, other.Y);
        public Vector2 WithY(IntFloatFrac y) => new Vector2(X, y);
        public Vector2 SelectX() => new Vector2(X, 0);
        public Vector2 SelectY() => new Vector2(0, Y);

        public override string ToString()
        {
            return $"[ {X} {Y} ]";
        }
        #endregion

        #region public static Methods
        #region operators
        public static Vector2 operator +(Vector2 left, Vector2 right) => left.Add(right);
        public static Vector2 operator -(Vector2 left, Vector2 right) => left.Subtract(right);
        public static Vector2 operator *(Vector2 left, IntFloatFrac right) => left.Multiply(right);
        public static Vector2 operator *(IntFloatFrac left, Vector2 right) => right.Multiply(left);
        public static Vector2 operator /(Vector2 left, IntFloatFrac right) => left.Divide(right);
        public static Vector2 operator /(IntFloatFrac left, Vector2 right) => right.DivideFrom(left);

        public static Vector2 operator -(Vector2 v) => new Vector2(-v.X, -v.Y);
        public static Vector2 operator +(Vector2 v) => v;

        public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);
        public static bool operator !=(Vector2 left, Vector2 right) => !left.Equals(right);

        #endregion
        #endregion
    }
}
