using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.Reflection
{
    public class CType : IEquatable<Type>, IEquatable<CType>
    {
        private Type internalType;

        public CType(Type type)
        {
            internalType = type;
        }

        public static Queue<Exception> GetPropertiesExceptions { get; set; }

        public string FullName { get { return internalType.FullName; } }

        public bool IsEnum { get { return internalType.IsEnum; } }

        public bool IsValueType { get { return internalType.IsValueType; } }

        public bool IsArray { get { return internalType.IsArray; } }

        public bool IsClass { get { return internalType.IsClass; } }

        public string Name { get { return internalType.Name; } }

        public Type[] GetGenericArguments()
        {
            return internalType.GetGenericArguments();
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return internalType.GetCustomAttributes(inherit);
        }

        public CType GetElementType()
        {
            return internalType.GetElementType();
        }

        public MethodInfo[] GetMethods(BindingFlags flags)
        {            
            var methods = internalType.GetMethods((System.Reflection.BindingFlags)flags);
            var newMethods = new MethodInfo[methods.Length];

            for (var i = 0; i < newMethods.Length; i++)
            {
                newMethods[i] = new MethodInfo(methods[i]);
            }

            return newMethods;
        }

        public PropertyInfo[] GetProperties(BindingFlags flags)
        {
            if (GetPropertiesExceptions != null && GetPropertiesExceptions.Count > 0)
            {
                var ex = GetPropertiesExceptions.Dequeue();
                if (ex != null)
                {
                    throw (ex);
                }
            }

            var props = internalType.GetProperties((System.Reflection.BindingFlags)flags);
            var newProps = new PropertyInfo[props.Length];

            for (var i = 0; i < props.Length; i++)
            {
                newProps[i] = new PropertyInfo(props[i]);
            }

            return newProps;
        }

        public PropertyInfo[] GetProperties()
        {
            var props = internalType.GetProperties();
            var newProps = new PropertyInfo[props.Length];

            for (var i = 0; i < props.Length; i++)
            {
                newProps[i] = new PropertyInfo(props[i]);
            }

            return newProps;
        }

        public static bool operator ==(CType a, Type b)
        {
            return a.internalType == b;
        }

        public static bool operator !=(CType a, Type b)
        {
            return a.internalType != b;
        }

        public static implicit operator Type(CType val)
        {
            return val.internalType;
        }

        public static implicit operator CType(Type val)
        {
            return new CType(val);
        }

        public override int GetHashCode()
        {
            return internalType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return internalType.Equals(obj);
        }

        public bool Equals(Type other)
        {
            return internalType.Equals(other);
        }

        public bool Equals(CType other)
        {
            return internalType.Equals(other.internalType);
        }

        public override string ToString()
        {
            return internalType.ToString();
        }
    }
}
