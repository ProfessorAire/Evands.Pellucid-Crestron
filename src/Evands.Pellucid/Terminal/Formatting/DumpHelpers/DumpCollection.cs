#region copyright
// <copyright file="DumpCollection.cs" company="Christopher McNeely">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp.Reflection;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    /// <summary>
    /// Extension of the <see cref="DumpNode"/> class for dumping collections.
    /// </summary>
    internal class DumpCollection : DumpNode
    {
        /// <summary>
        /// The collection of individual nodes associated with this node.
        /// </summary>
        private List<DumpNode> items = new List<DumpNode>();

        /// <summary>
        /// The number of individual nodes associated with this node, where Dictionary entries are counted as 1, though they're displayed as two.
        /// </summary>
        private int count = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpCollection"/> class.
        /// </summary>
        /// <param name="value">The <see cref="IEnumerable"/> collection to associate with this node.</param>
        public DumpCollection(IEnumerable value)
            : this(value, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpCollection"/> class.
        /// </summary>
        /// <param name="value">The <see cref="IEnumerable"/> collection to associate with this node.</param>
        /// <param name="name">The name of this node. Can be an empty string.</param>
        public DumpCollection(IEnumerable value, string name)
            : this(value, name, value != null ? value.GetType() : null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpCollection"/> class.
        /// </summary>
        /// <param name="value">The <see cref="IEnumerable"/> collection to associate with this node.</param>
        /// <param name="name">The name of this node. Can be an empty string.</param>
        /// <param name="collectionType">The <see langword="Type"/> of the collection.</param>
        public DumpCollection(IEnumerable value, string name, Type collectionType)
            : base(value, name, collectionType)
        {
            this.PopulateItems();
        }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <param name="maxDepth">The maximum depth that child objects will recurse in order to dump.</param>
        /// <param name="currentDepth">The current depth of the dump process.</param>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's full type name, otherwise <see langword="false"/>.</param>
        /// <returns>A console formatted string representing the node value.</returns>
        internal override string ToString(int maxDepth, int currentDepth, bool useFullTypeNames)
        {
            if (currentDepth < 0)
            {
                currentDepth = 0;
            }

            if (maxDepth > 0 && currentDepth > maxDepth)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            var typeName = string.Empty;

            if (useFullTypeNames)
            {
                typeName = this.ValueType != null ? this.ValueType.FullName : "<unknown null collection>";
            }
            else
            {
                typeName = this.ValueType != null ? this.ValueType.Name : "<unknown null collection>";
            }

            var underscoreLength = typeName.Length + 7 + this.count.ToString().Length + (this.count != 1 ? 1 : 0);

            typeName = string.Format(
                "{0} {1}",
                typeName,
                ConsoleBase.Colors.DumpObjectInfo.FormatText("({0} Item{1})", this.count, this.count != 1 ? "s" : string.Empty));

            var rootPadding = 0;
            var itemPadding = 0;

            if (!string.IsNullOrEmpty(this.Name))
            {
                rootPadding = this.Name.Length + 3;
            }

            if (this.items.Count > 0)
            {
                itemPadding = this.items.Count.ToString().Length;

                if (!(this.Value is IDictionary))
                {
                    itemPadding += 3;
                }
            }

            if (!string.IsNullOrEmpty(this.Name))
            {
                sb.AppendFormat(
                    "{0} {1} ",
                    ConsoleBase.Colors.DumpPropertyName.FormatText(this.Name),
                    ConsoleBase.Colors.DumpObjectChrome.FormatText("="));
            }

            var padSb = new StringBuilder();

            padSb.Append(' ', rootPadding);
            padSb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(Formatters.Chrome.BodyLeft));
            padSb.Append(' ', itemPadding);
            
            sb.Append(ConsoleBase.Colors.DumpObjectDetail.FormatText(typeName));
            if (maxDepth == 0 || currentDepth < maxDepth)
            {
                sb.Append(ConsoleBase.NewLine);
                sb.Append(' ', rootPadding);
                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, Formatters.Chrome.BodyTopLeft.ToString()));
                for (var i = 0; i < underscoreLength; i++)
                {
                    sb.Append(Formatters.Chrome.BodyTop);
                }

                sb.Append(Formatting.ColorFormat.CloseTextFormat(ConsoleBase.NewLine));
            }

            var needLine = false;
            var index = 0;
            foreach (var item in this.items)
            {
                if (needLine)
                {
                    sb.Append(ConsoleBase.NewLine);
                }

                var str = item.ToString(maxDepth, currentDepth + 1, useFullTypeNames);
                if (!string.IsNullOrEmpty(str))
                {
                    sb.Append(' ', rootPadding);
                    sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, "{0} ",
                        Formatters.Chrome.BodyLeft));
                    if (!(this.Value is IDictionary))
                    {
                        sb.AppendFormat(
                            ConsoleBase.Colors.DumpObjectInfo.FormatText(
                                "{0}: ", index.ToString().PadLeft(this.count.ToString().Length)));
                    }

                    sb.Append(str.TrimStart(' ').Replace(
                                ConsoleBase.NewLine,
                                string.Format(
                                    "{0}{1}",
                                    ConsoleBase.NewLine,
                                    padSb.ToString())));
                    needLine = true;
                    index++;
                }
            }

            if (maxDepth == 0 || currentDepth < maxDepth)
            {
                if (this.count > 0)
                {
                    sb.Append(ConsoleBase.NewLine);
                }

                sb.Append(' ', rootPadding);
                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, Formatters.Chrome.BodyBottomLeft));
                for (var i = 0; i < underscoreLength - 1; i++)
                {
                    sb.Append(Formatters.Chrome.BodyBottom);
                }

                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(Formatters.Chrome.BodyBottom));
            }

            if (currentDepth == 0)
            {
                sb.Append(ConsoleBase.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Populates the items of the node.
        /// </summary>
        private void PopulateItems()
        {
            this.items.Clear();

            if (this.Value is IDictionary)
            {
                this.PopulateDictionary();
            }
            else
            {
                this.PopulateEnumerable();
            }
        }

        /// <summary>
        /// Populates the item nodes from a dictionary.
        /// </summary>
        private void PopulateDictionary()
        {
            var idict = this.Value as IDictionary;
            if (idict.Count == 0)
            {
                return;
            }

            var index = 0;
            var padding = idict.Count.ToString().Length + 5;
            foreach (var key in idict.Keys)
            {
                this.items.Add(DumpFactory.GetNode(key, string.Format("Key{0}", index).PadRight(padding, ' ')));
                this.items.Add(DumpFactory.GetNode(idict[key], string.Format("Value{0}", index).PadRight(padding, ' ')));
                index++;
            }

            this.count = index;
        }

        /// <summary>
        /// Populates the items nodes from a standard enumerable object.
        /// </summary>
        private void PopulateEnumerable()
        {
            var ienum = this.Value as IEnumerable;
            if (ienum == null)
            {
                return;
            }

            foreach (var item in ienum)
            {
                this.items.Add(DumpFactory.GetNode(item));
            }

            this.count = this.items.Count;
        }
    }
}