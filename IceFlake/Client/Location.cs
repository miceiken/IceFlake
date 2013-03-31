using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using SlimDX;

//using Microsoft.Xna.Framework;

namespace IceFlake.Client
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Location
    {
        public float X;
        public float Y;
        public float Z;

        public Location(float x, float y, float z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Location(XElement xml)
            : this(Convert.ToSingle(xml.Attribute("X").Value, CultureInfo.InvariantCulture),
                   Convert.ToSingle(xml.Attribute("Y").Value, CultureInfo.InvariantCulture),
                   Convert.ToSingle(xml.Attribute("Z").Value, CultureInfo.InvariantCulture))
        {
        }


        public double DistanceTo(Location loc)
        {
            return Math.Sqrt(Math.Pow(X - loc.X, 2) + Math.Pow(Y - loc.Y, 2) + Math.Pow(Z - loc.Z, 2));
        }

        public double Distance2D(Location loc)
        {
            return Math.Sqrt(Math.Pow(X - loc.X, 2) + Math.Pow(Y - loc.Y, 2));
        }

        public double Length
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }
        }

        public Location Normalize()
        {
            double len = Length;
            return new Location((float) (X/len), (float) (Y/len), (float) (Z/len));
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        //public Microsoft.Xna.Framework.Vector3 ToXNA()
        //{
        //    return new Microsoft.Xna.Framework.Vector3(X, Y, Z);
        //}

        public float[] ToFloatArray(bool xyz = false)
        {
            if (xyz)
                return new[] {X, Y, Z};
            return new[] {X, Z, Y};
        }

        public float Angle
        {
            get { return (float) Math.Atan2(Y, X); }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var loc = (Location) obj;
            if (loc.X != X || loc.Y != Y || loc.Z != Z)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() | Y.GetHashCode() | Z.GetHashCode();
        }

        public override string ToString()
        {
            return "[" + (int) X + ", " + (int) Y + ", " + (int) Z + "]";
        }

        public XElement GetXml()
        {
            return new XElement("Location", new XAttribute("X", X), new XAttribute("Y", Y), new XAttribute("Z", Z));
        }

        public static bool operator ==(Location a, Location b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Location a, Location b)
        {
            return !a.Equals(b);
        }

        public static Location Invalid
        {
            get { return default(Location); }
        }
    }

    public struct Blackspot
    {
        public Location Location;
        public float Radius;

        public Blackspot(float x, float y, float z, float r)
            : this()
        {
            Location = new Location(x, y, z);
            Radius = r;
        }

        public Blackspot(XElement xml)
            : this(Convert.ToSingle(xml.Attribute("X").Value, CultureInfo.InvariantCulture),
                   Convert.ToSingle(xml.Attribute("Y").Value, CultureInfo.InvariantCulture),
                   Convert.ToSingle(xml.Attribute("Z").Value, CultureInfo.InvariantCulture),
                   Convert.ToSingle(xml.Attribute("Radius").Value, CultureInfo.InvariantCulture))
        {
        }
    }
}