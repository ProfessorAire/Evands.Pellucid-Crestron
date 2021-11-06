#region copyright
// <copyright file="DumpFactory.cs" company="Christopher McNeely">
// The MIT License (MIT)
// Copyright (c) Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
#endregion

using System;
using System.Collections;
using Crestron.SimplSharp.Reflection;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    /// <summary>
    /// Factory for retrieving <see cref="DumpNode"/> objects for dumping to console.
    /// </summary>
    internal static class DumpFactory
    {
        /// <summary>
        /// Gets a node for the provided object.
        /// </summary>
        /// <param name="obj">The object to get a node for.</param>
        /// <returns>A <see cref="DumpNode"/>.</returns>
        public static DumpNode GetNode(object obj)
        {
            return GetNode(obj, string.Empty);
        }

        /// <summary>
        /// Gets a node with the specified name for the provided object.
        /// </summary>
        /// <param name="obj">The object to get a node for.</param>
        /// <param name="name">The name of the node.</param>
        /// <returns>A <see cref="DumpNode"/>.</returns>
        public static DumpNode GetNode(object obj, string name)
        {
            return GetNode(obj, name, obj != null ? obj.GetType() : null);
        }

        /// <summary>
        /// Gets a node with the specified name for the provided object.
        /// </summary>
        /// <param name="obj">The object to get a node for.</param>
        /// <param name="name">The name of the node.</param>
        /// <param name="objectType">The <see langword="Type"/> of the object.</param>
        /// <returns>A <see cref="DumpNode"/>.</returns>
        public static DumpNode GetNode(object obj, string name, Type objectType)
        {
            if (obj == null)
            {
                return new DumpNode(obj, name, objectType);
            }

            if (obj.GetType().GetCType().IsValueType || obj is string)
            {
                return new DumpNode(obj, name, objectType);
            }

            var idict = obj as IDictionary;
            if (idict != null)
            {
                return new DumpCollection(idict, name, objectType);
            }

            var ilist = obj as IList;
            if (ilist != null)
            {
                return new DumpCollection(ilist, name, objectType);
            }

            var ienum = obj as IEnumerable;
            if (ienum != null)
            {
                return new DumpCollection(ienum, name, objectType);
            }

            return new DumpObject(obj, name, objectType);
        }
    }
}