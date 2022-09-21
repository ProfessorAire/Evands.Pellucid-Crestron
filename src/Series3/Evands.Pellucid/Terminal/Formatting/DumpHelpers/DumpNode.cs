#region copyright
// <copyright file="DumpNode.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    /// <summary>
    /// Base class for dumping an object. If not overwritten this simply dumps the
    /// name and value of the object.
    /// </summary>
    internal class DumpNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DumpNode"/> class.
        /// </summary>
        /// <param name="value">The object to associated with the node.</param>
        /// <param name="name">The name of the object. Can be an empty string.</param>
        public DumpNode(object value, string name)
            : this(value, name, value != null ? value.GetType() : null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpNode"/> class.
        /// </summary>
        /// <param name="value">The object to associated with the node.</param>
        /// <param name="name">The name of the object. Can be an empty string.</param>
        /// <param name="valueType">The <see langword="Type"/> of the node's value.</param>
        public DumpNode(object value, string name, Type valueType)
        {
            this.ValueType = valueType;
            this.Name = name ?? string.Empty;
            if (value is string)
            {
                this.Value = string.Format("\"{0}\"", value);
            }
            else
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets value of the node.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Gets the <see langword="Type"/> of the value.
        /// </summary>
        public Type ValueType { get; private set; }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <returns>A console formatted string representing the node value.</returns>
        public override string ToString()
        {
            return this.ToString(0);
        }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <param name="maxDepth">The maximum depth that child objects will be recursed in order to dump.</param>
        /// <returns>A console formatted string representing the node value.</returns>
        public string ToString(int maxDepth)
        {
            return this.ToString(maxDepth, 0);
        }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's full type name, otherwise <see langword="false"/>.</param>
        /// <returns>A console formatted string representing the node value.</returns>
        public string ToString(bool useFullTypeNames)
        {
            return this.ToString(0, 0, useFullTypeNames);
        }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <param name="maxDepth">The maximum depth that child objects will be recursed in order to dump.</param>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's full type name, otherwise <see langword="false"/>.</param>
        /// <returns>A console formatted string representing the node value.</returns>
        public string ToString(int maxDepth, bool useFullTypeNames)
        {
            return this.ToString(maxDepth, 0, useFullTypeNames);
        }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <param name="maxDepth">The maximum depth that child objects will be recursed in order to dump.</param>
        /// <param name="currentDepth">The current depth of the dump process.</param>
        /// <returns>A console formatted string representing the node value.</returns>
        internal string ToString(int maxDepth, int currentDepth)
        {
            return this.ToString(maxDepth, currentDepth, Options.Instance.UseFullTypeNamesWhenDumping);
        }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <param name="maxDepth">The maximum depth that child objects will be recursed in order to dump.</param>
        /// <param name="currentDepth">The current depth of the dump process.</param>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's full type name, otherwise <see langword="false"/>.</param>
        /// <returns>A console formatted string representing the node value.</returns>
        internal virtual string ToString(int maxDepth, int currentDepth, bool useFullTypeNames)
        {
            if (currentDepth < 0)
            {
                currentDepth = 0;
            }

            if (maxDepth > 0 && currentDepth > maxDepth)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                return ConsoleBase.Colors.DumpPropertyValue.FormatText(
                    this.Value != null ? this.Value.ToString() : "<null>");
            }
            else
            {
                return string.Format(
                    "{0} {1} {2}",
                    ConsoleBase.Colors.DumpPropertyName.FormatText(this.Name),
                    ConsoleBase.Colors.DumpObjectChrome.FormatText("="),
                    ConsoleBase.Colors.DumpPropertyValue.FormatText(
                    this.Value != null ? this.Value.ToString() : "<null>"));
            }
        }
    }
}