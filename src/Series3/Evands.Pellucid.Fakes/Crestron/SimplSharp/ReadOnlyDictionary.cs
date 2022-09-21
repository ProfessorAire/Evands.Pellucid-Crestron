using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp
{
    public class ReadOnlyDictionary<T1, T2>
    {
        public Dictionary<T1, T2> internalDictionary;

        public ReadOnlyDictionary(Dictionary<T1, T2> dict)
        {
            internalDictionary = dict;
        }

        public T2 this[T1 key]
        {
            get
            {
                return internalDictionary[key];
            }

            set
            {
                internalDictionary[key] = value;
            }
        }

        public bool ContainsKey(T1 key)
        {
            return internalDictionary.ContainsKey(key);
        }
    }
}
