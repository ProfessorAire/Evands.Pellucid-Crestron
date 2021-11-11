using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.Reflection
{
    public static class ExtensionMethods
    {
        private static int quantityThrown = 0;

        private static int quantityToThrow = 0;

        static ExtensionMethods()
        {
            ExceptionToThrowOnGetCType = null;
            QuantityToThrowOnGetCType = 0;
        }

        public static Exception ExceptionToThrowOnGetCType { get; set; }

        public static int QuantityToThrowOnGetCType
        {
            get
            {
                return quantityToThrow;
            }

            set
            {
                quantityToThrow = value;
                quantityThrown = 0;
            }
        }


        public static CType GetCType(this Type type)
        {
            if (ExceptionToThrowOnGetCType != null && QuantityToThrowOnGetCType > quantityThrown)
            {
                quantityThrown++;
                throw ExceptionToThrowOnGetCType;
            }

            return new CType(type);
        }
    }
}
