/*
  This file is part of ppather.

    PPather is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PPather is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with ppather.  If not, see <http://www.gnu.org/licenses/>.

*/
using System;
using System.Collections.Generic;
using System.Text;
namespace PatherPath.Graph
{
    public class Location
    {
        public readonly float X, Y, Z;
        public Location(Location l)
        {
            X = l.X;
            Y = l.Y;
            Z = l.Z;
        }
        public Location(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float GetDistanceTo(Location l)
        {
            float dx = X - l.X;
            float dy = Y - l.Y;
            float dz = Z - l.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        public float GetDistanceTo2D(Location l)
        {
            float dx = X - l.X;
            float dy = Y - l.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        public Location InFrontOf(float heading, float d)
        {
            float nx = X + (float)Math.Cos(heading) * d;
            float ny = Y + (float)Math.Sin(heading) * d;
            float nz = Z;
            return new Location(nx, ny, nz);
        }

        public override String ToString()
        {
            //String s = continent+":"+description + "[" + (int)x + "," + (int)y + "," + (int)z + "]";
            return string.Format("({0} {1} {2})", X, Y, Z);
        }
    }
}
