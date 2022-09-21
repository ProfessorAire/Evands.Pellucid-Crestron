using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.Reflection
{
    public class MethodInfo
    {
        System.Reflection.MethodInfo internalInfo;

        public MethodInfo(System.Reflection.MethodInfo info)
        {
            internalInfo = info;
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return internalInfo.GetCustomAttributes(inherit);
        }

        public ParameterInfo[] GetParameters()
        {
            var parameters = internalInfo.GetParameters();
            var newParameters = new ParameterInfo[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                newParameters[i] = new ParameterInfo(parameters[i]);
            }

            return newParameters;
        }

        public void Invoke(object origin, object[] parameterList)
        {
            internalInfo.Invoke(origin, parameterList);
        }
    }
}
