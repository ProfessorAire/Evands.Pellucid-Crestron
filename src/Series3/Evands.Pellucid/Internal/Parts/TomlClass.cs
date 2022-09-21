#region copyright
// <copyright file="TomlClass.cs" company="Christopher McNeely">
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
using System.Linq;
using System.Text;

namespace Evands.Pellucid.Internal.Parts
{
    /// <summary>
    /// Provides functions for serializing classes to TOML.
    /// </summary>
    internal class TomlClass : INamedToml
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TomlClass"/> class.
        /// </summary>
        /// <param name="members">A list of <see cref="IPrintToml"/> objects that are members (properties) of the class.</param>        
        public TomlClass(List<IPrintToml> members)
        {
            if (members == null)
            {
                throw new ArgumentNullException("members");
            }

            Members = members;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TomlClass"/> class.
        /// </summary>
        /// <param name="name">The name of the class.</param>
        /// <param name="members">A list of <see cref="IPrintToml"/> objects that are members (properties) of the class.</param>        
        public TomlClass(string name, List<IPrintToml> members)
            : this(members)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the class as it should be serialized.
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// Gets list of <see cref="IPrintToml"/> members of the class.
        /// </summary>        
        public List<IPrintToml> Members { get; private set; }

        /// <summary>
        /// Formats the object as a string for writing to a file.
        /// </summary>
        /// <param name="forArray">If true will format the value for writing inside an array.</param>
        /// <returns>A string formatted for writing to a file.</returns>    
        public string PrintLines(bool forArray)
        {
            var sb = new StringBuilder();

            foreach (var member in Members)
            {
                if (member is TomlClass)
                {
                    sb.Append("\n");
                }

                if (string.IsNullOrEmpty(Name))
                {
                    sb.AppendFormat("{0}", member.PrintLines(forArray));
                }
                else
                {
                    sb.AppendFormat("{0}.{1}", Name, member.PrintLines(forArray));
                }

                if (forArray)
                {
                    if (!(member == Members.Last()))
                    {
                        sb.Append(", ");
                    }
                }
                else
                {
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
    }
}