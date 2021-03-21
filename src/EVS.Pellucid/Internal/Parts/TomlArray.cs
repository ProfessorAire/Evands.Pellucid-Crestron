#region copyright
// <copyright file="TomlArray.cs" company="Christopher McNeely">
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
using System.Collections.Generic;
using System.Text;

namespace EVS.Pellucid.Internal.Parts
{
    /// <summary>
    /// Provides functions for serializing arrays to TOML.
    /// </summary>
    internal class TomlArray : INamedToml
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TomlArray"/> class.
        /// </summary>
        /// <param name="items">The items in the array.</param>        
        public TomlArray(List<IPrintToml> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            Items = items;
        }

        /// <summary>
        /// Gets or sets the name of the array.
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// Gets the contents of the array.
        /// </summary>        
        public List<IPrintToml> Items { get; private set; }

        /// <summary>
        /// Formats the object as a string for writing to a file.
        /// </summary>
        /// <param name="forArray">If true will format the value for writing inside an array.</param>
        /// <returns>A string formatted for writing to a file.</returns>
        public string PrintLines(bool forArray)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendFormat("{0} = ", Name);
            }

            sb.AppendFormat("[ ");
            for (var i = 0; i < Items.Count; i++)
            {
                if (i > 0)
                {
                    if (Items[i - 1] is TomlClass)
                    {
                        sb.Append(",\n");
                    }
                    else
                    {
                        sb.Append(",");
                    }
                }

                if (Items[i] is TomlClass)
                {
                    sb.Append("{ ");
                    sb.Append(Items[i].PrintLines(true));
                    sb.Append(" }");
                }
                else
                {
                    sb.Append(Items[i].PrintLines(true));
                }
            }

            sb.Append(" ]");

            return sb.ToString();
        }
    }
}