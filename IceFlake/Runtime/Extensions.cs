using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using IceFlake.Client;
#if SLIMDX
using SlimDX;
#else
using IceFlake.DirectX;
#endif

namespace IceFlake.Runtime
{
    public static class Extension
    {
        public static string ToLongString(this Exception self)
        {
            string result = "";

            Exception exception = self;
            do
            {
                result += "Exception " + exception.GetType() + ": " + exception.Message + Environment.NewLine +
                          Environment.NewLine + exception.StackTrace + Environment.NewLine + Environment.NewLine;
                exception = exception.InnerException;
            } while (exception != null);

            return result;
        }

        public static float[] ToFloatArray(this Vector3 v)
        {
            return new[] {v.X, v.Y, v.Z};
        }

        public static IEnumerable<Location> ToLocation(this IEnumerable<Vector3> v)
        {
            return v.Select(l => new Location(l.X, l.Y, l.Z));
        }

        public static Location ToLocation(this Vector3 v)
        {
            return new Location(v.X, v.Y, v.Z);
        }

        public static Vector3 ToWoW(this Vector3 v)
        {
            return new Vector3(-v.Z, -v.X, v.Y);
        }

        public static Vector3 ToRecast(this Vector3 v)
        {
            return new Vector3(-v.Y, v.Z, -v.X);
        }

        public static IntPtr ToPointer(this uint u)
        {
            return new IntPtr(u);
        }

        public static void DumpProperties(this object o)
        {
            Type t = o.GetType();
            Log.WriteLine("Dumping Properties of {0} (Type = {1})", t.Name, t);
            foreach (PropertyInfo p in t.GetProperties())
            {
                try
                {
                    Log.WriteLine("\t{0} = {1}", p.Name, p.GetValue(o, null));
                }
                catch
                {
                    Log.WriteLine("\t{0} = null?", p.Name);
                }
            }
            foreach (FieldInfo p in t.GetFields())
            {
                try
                {
                    Log.WriteLine("\t{0} = {1}", p.Name, p.GetValue(o));
                    if (p.FieldType.IsArray)
                        foreach (object pe in (Array) p.GetValue(o))
                            Log.WriteLine("\t\t{0}", pe);
                }
                catch
                {
                    Log.WriteLine("\t{0} = null?", p.Name);
                }
            }
        }

        public static bool WildcardMatch(this string s, string pattern)
        {
            var regex = new Regex("^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$");
            return regex.IsMatch(s);
        }
    }
}