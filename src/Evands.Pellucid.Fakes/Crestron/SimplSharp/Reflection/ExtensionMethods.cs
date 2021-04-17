using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.Reflection
{
    public static class ExtensionMethods
    {
        public static CType GetCType(this Type type)
        {
            return new CType(type);
        }
    }
}
