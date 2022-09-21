using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.Reflection
{
    public static class Activator
    {
        public static object CreateInstance(CType type)
        {
            return System.Activator.CreateInstance(type);
        }
    }
}
