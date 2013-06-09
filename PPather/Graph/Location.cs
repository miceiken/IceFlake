using System;

namespace PatherPath.Graph
{
    public class Location
    {
        private readonly float x;
        private readonly float y;
        private readonly float z;

        public Location(Location l)
        {
            x = l.X;
            y = l.Y;
            z = l.Z;
        }

        public Location(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float X
        {
            get { return x; }
        }

        public float Y
        {
            get { return y; }
        }

        public float Z
        {
            get { return z; }
        }

        public float GetDistanceTo(Location l)
        {
            float dx = x - l.X;
            float dy = y - l.Y;
            float dz = z - l.Z;
            return (float) Math.Sqrt(dx*dx + dy*dy + dz*dz);
        }

        public override String ToString()
        {
            //String s = String.Format(
            String s = "[" + x + "," + y + "," + z + "]";
            return s;
        }

        public Location InFrontOf(float heading, float d)
        {
            float nx = x + (float) Math.Cos(heading)*d;
            float ny = y + (float) Math.Sin(heading)*d;
            float nz = z;
            return new Location(nx, ny, nz);
        }
    }
}