using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.Reflection
{
    public class PropertyInfo
    {
        public System.Reflection.PropertyInfo internalInfo;

        public PropertyInfo(System.Reflection.PropertyInfo info)
        {
            internalInfo = info;
        }

        public string Name { get { return internalInfo.Name; } }

        public CType PropertyType { get { return internalInfo.PropertyType; } }

        public object GetValue(object obj, object[] index)
        {
            return internalInfo.GetValue(obj, index);
        }

        public bool IsDefined(CType type, bool inherit)
        {
            return internalInfo.IsDefined(type, inherit);
        }

        public object[] GetCustomAttributes(CType type, bool inherit)
        {
            return internalInfo.GetCustomAttributes(type, inherit);
        }

        public void SetValue(object obj, object value, object[] index)
        {
            internalInfo.SetValue(obj, value, index);
        }
    }
}
