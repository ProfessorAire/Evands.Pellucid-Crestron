using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using System.Collections;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    internal static class DumpFactory
    {
        public static DumpNode GetNode(object obj)
        {
            return GetNode(string.Empty, obj);
        }

        public static DumpNode GetNode(string name, object obj)
        {
            if (obj == null)
            {
                return new DumpNode(obj, name);
            }

            if (obj.GetType().GetCType().IsValueType || obj is string)
            {
                return new DumpNode(obj, name);
            }

            var idict = obj as IDictionary;
            if (idict != null)
            {
                return new DumpCollection(idict, name);
            }

            var ilist = obj as IList;
            if (ilist != null)
            {
                return new DumpCollection(ilist, name);
            }

            var icoll = obj as ICollection;
            if (icoll != null)
            {
                return new DumpCollection(icoll, name);
            }

            var ienum = obj as IEnumerable;
            if (ienum != null)
            {
                return new DumpCollection(ienum, name);
            }

            return new DumpObject(obj, name);
        }
    }
}