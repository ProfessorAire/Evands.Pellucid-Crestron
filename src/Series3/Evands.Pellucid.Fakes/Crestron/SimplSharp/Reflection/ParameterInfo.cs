using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.Reflection
{
    public class ParameterInfo
    {
        System.Reflection.ParameterInfo internalInfo;

        public ParameterInfo(System.Reflection.ParameterInfo info)
        {
            internalInfo = info;
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return internalInfo.GetCustomAttributes(inherit);
        }

        public int Position { get { return internalInfo.Position; } }

        public CType ParameterType { get { return new CType(internalInfo.ParameterType); } }
    }
}
