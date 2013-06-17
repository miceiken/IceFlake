using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Patchables;

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
    }
}
