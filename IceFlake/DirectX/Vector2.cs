using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceFlake.DirectX {
    public struct Vector2 {
        public float X;
        public float Y;

        public Vector2(float x, float y) {
            X = x; Y = y;
        }

        public double Distance(Vector2 loc) {
            return Math.Sqrt(Math.Pow(X - loc.X, 2) + Math.Pow(Y - loc.Y, 2));
        }

        public double Length {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
        }

        public Vector2 RotateAround(Vector2 o, float th) {
            double s = Math.Sin(th);
            double c = Math.Cos(th);
            double px = X - o.X;
            double py = Y - o.Y;
            double xn = px * c - py * s;
            double yn = px * s + py * c;
            px = xn + o.X;
            py = yn + o.Y;
            return new Vector2((float)px, (float)py);
        }

        public static Vector2 operator +(Vector2 pt0, Vector2 pt1) {
            return new Vector2(pt0.X + pt1.X, pt0.Y + pt1.Y);
        }

        public static readonly Vector2 UnitX = new Vector2(1f, 0f);
        public static readonly Vector2 UnitY = new Vector2(0f, 1f);
        public static readonly Vector2 Origo = new Vector2(0f, 0f);

        public Vector2 Normalize() {
            double len = Length;
            return new Vector2((float)(X / len), (float)(Y / len));
        }

        public float Angle {
            get { return (float)Math.Atan2(Y, X); }
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var loc = (Vector2)obj;
            if (loc.X != X || loc.Y != Y)
                return false;
            return true;
        }

        public override int GetHashCode() {
            return X.GetHashCode() | Y.GetHashCode();
        }
    }
}
