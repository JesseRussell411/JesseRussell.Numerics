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

        #region public Methods
        public Vector2 Add(Vector2 other) => new Vector2(X + other.X, Y + other.Y);
        public Vector2 Multiply(IntFloatFrac scaler) => new Vector2(X * scaler, Y * scaler);
        public Vector2 Transform(Vector2 xTransform, Vector2 yTransform) => xTransform.Multiply(X).Add(yTransform.Multiply(Y));

        public override string ToString()
        {
            return $"[ {X} {Y} ]";
        }
        #endregion
    }
}
