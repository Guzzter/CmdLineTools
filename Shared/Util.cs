using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public static class Util
    {
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (String.Compare(value, otherValue) == 0)
                    return true;

            return false;
        }
    }
}
