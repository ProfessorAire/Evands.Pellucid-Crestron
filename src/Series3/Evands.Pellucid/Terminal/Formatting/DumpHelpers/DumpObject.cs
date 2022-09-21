#region copyright
// <copyright file="DumpObject.cs" company="Christopher McNeely">
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
#if SERIES4
using System.Reflection;
#else
using Crestron.SimplSharp.Reflection;
#endif

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    /// <summary>
    /// The DumpObject Class.
    /// </summary>
    internal class DumpObject : DumpNode
    {
        /// <summary>
        /// Backing field for the <see cref="Children"/> property.
        /// </summary>
        private List<DumpNode> children = new List<DumpNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpObject"/> class.
        /// </summary>
        /// <param name="value">The object to associate with this node.</param>
        public DumpObject(object value)
            : this(value, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpObject"/> class.
        /// </summary>
        /// <param name="value">The object to associate with this node.</param>
        /// <param name="name">The name of this node. Can be an empty string.</param>
        public DumpObject(object value, string name)
            : this(value, name, value != null ? value.GetType() : null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpObject"/> class.
        /// </summary>
        /// <param name="value">The object to associate with this node.</param>
        /// <param name="name">The name of this node. Can be an empty string.</param>
        /// <param name="objectType">The <see langword="Type"/> of the object's value.</param>
        public DumpObject(object value, string name, Type objectType)
            : base(value, name, objectType)
        {
            this.Children = new ReadOnlyCollection<DumpNode>(this.children);
            this.PopulateChildren();
        }

        /// <summary>
        /// Gets the children associated with this object node.
        /// </summary>
        public ReadOnlyCollection<DumpNode> Children { get; private set; }

        /// <summary>
        /// Returns the node's name and value formatted for displaying in the console.
        /// </summary>
        /// <param name="maxDepth">The maximum depth that child objects will be recursed in order to dump.</param>
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

            var padding = 0;
            var sb = new StringBuilder();

            var typeName = string.Empty;

            if (useFullTypeNames)
            {
#if SERIES4
                typeName = this.ValueType != null ? this.ValueType.FullName : "<unknown type>";
#else
                typeName = this.ValueType != null ? this.ValueType.GetCType().FullName : "<unknown type>";
#endif
            }
            else
            {
#if SERIES4
                typeName = this.ValueType != null ? this.ValueType.Name : "<unknown type>";
#else
                typeName = this.ValueType != null ? this.ValueType.GetCType().Name : "<unknown type>";
#endif
            }

            var underscoreLength = typeName.Length + 10 + this.Children.Count.ToString().Length + (this.Children.Count == 1 ? 1 : 3);

            typeName = string.Format(
                "{0} {1}",
                typeName,
                ConsoleBase.Colors.DumpObjectInfo.FormatText("({0} Propert{1})", this.Children.Count, this.Children.Count == 1 ? "y" : "ies"));

            if (!string.IsNullOrEmpty(this.Name))
            {
                sb.AppendFormat(
                    "{0} {1} ",
                    ConsoleBase.Colors.DumpPropertyName.FormatText(this.Name),
                    ConsoleBase.Colors.DumpObjectChrome.FormatText("="));
                padding += this.Name.Length + 3;
            }
            else if (this.Children.Count > 0)
            {
                sb.Append(' ', padding);
            }

            var padSb = new StringBuilder(padding);
            padSb.Append(' ', padding);
            padSb.Append(string.Format(ConsoleBase.Colors.DumpObjectChrome.FormatText("{0} ", Formatters.Chrome.BodyLeft)));

            sb.Append(ConsoleBase.Colors.DumpObjectDetail.FormatText(typeName));
            if ((maxDepth == 0 || currentDepth < maxDepth) && this.Children.Count > 0)
            {
                sb.Append(ConsoleBase.NewLine);
                sb.Append(' ', padding);
                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, Formatters.Chrome.BodyTopLeft));
                for (var i = 0; i < underscoreLength; i++)
                {
                    sb.Append(Formatters.Chrome.BodyTop);
                }

                sb.Append(ColorFormat.CloseTextFormat(ConsoleBase.NewLine));
            }

            var needLine = false;
            foreach (var child in this.Children)
            {
                if (needLine)
                {
                    sb.Append(ConsoleBase.NewLine);
                }

                var str = child.ToString(maxDepth, currentDepth + 1, useFullTypeNames);
                if (!string.IsNullOrEmpty(str))
                {
                    sb.Append(' ', padding);
                    sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, "{0} ", Formatters.Chrome.BodyLeft));
                    sb.Append(str.Replace(
                                ConsoleBase.NewLine,
                                string.Format(
                                    "{0}{1}",
                                    ConsoleBase.NewLine,
                                    padSb.ToString())));
                    needLine = true;
                }
            }

            if ((maxDepth == 0 || currentDepth < maxDepth) && this.Children.Count > 0)
            {
                sb.Append(ConsoleBase.NewLine);
                sb.Append(' ', padding);
                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, Formatters.Chrome.BodyBottomLeft));
                for (var i = 0; i < underscoreLength - 1; i++)
                {
                    sb.Append(Formatters.Chrome.BodyBottom);
                }

                sb.Append(ColorFormat.CloseTextFormat(Formatters.Chrome.BodyBottom));
            }

            if (currentDepth == 0)
            {
                sb.Append(ConsoleBase.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Populates the node's children.
        /// </summary>
        private void PopulateChildren()
        {
            var failures = new List<DumpObjectFailure>();
            try
            {
                if (this.Value != null)
                {
                    PropertyInfo[][] props = new PropertyInfo[2][];

                    try
                    {
#if SERIES4
                        props[0] = this.Value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
#else
                        props[0] = this.Value.GetType().GetCType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
#endif
                    }
                    catch (Exception ex)
                    {
                        failures.Add(
                            new DumpObjectFailure()
                            {
                                ErrorMessage = "Exception while attempting to get the public instance properties of this object.",
                                ExceptionMessage = ex.Message,
                                ExceptionType = ex.GetType().FullName
                            });
                    }

                    try
                    {
#if SERIES4
                        props[1] = this.Value.GetType().GetProperties(BindingFlags.Static | BindingFlags.Public);
#else
                        props[1] = this.Value.GetType().GetCType().GetProperties(BindingFlags.Static | BindingFlags.Public);
#endif
                    }
                    catch (Exception ex)
                    {
                        failures.Add(
                            new DumpObjectFailure()
                            {
                                ErrorMessage = "Exception while attempting to get the public static properties of this object.",
                                ExceptionMessage = ex.Message,
                                ExceptionType = ex.GetType().FullName
                            });
                    }

                    var max = 0;
                    if (props[0] != null && props[0].Length > 0)
                    {
                        max = props[0].Max(p => p.Name.Length);
                    }

                    if (props[1] != null && props[1].Length > 0)
                    {
                        max = Math.Max(max, props[1].Max(p => p.Name.Length));
                    }

                    this.children.Clear();

                    for (var i = 0; i < props.Length; i++)
                    {
                        if (props[i] != null)
                        {
                            foreach (var prop in props[i])
                            {
                                try
                                {
                                    object value;
                                    if (i == 1)
                                    {
                                        value = prop.GetValue(null, null);
                                    }
                                    else
                                    {
                                        value = prop.GetValue(this.Value, null);
                                    }

                                    if (this.Value != value)
                                    {
                                        this.children.Add(DumpFactory.GetNode(value, prop.Name.PadRight(max), prop.PropertyType));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    failures.Add(
                                            new DumpObjectFailure()
                                            {
                                                PropertyName = prop.Name,
                                                ErrorMessage = "Exception encountered while dumping property value.",
                                                ExceptionMessage = ex.Message,
                                                ExceptionType = ex.GetType().FullName
                                            });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                failures.Add(
                    new DumpObjectFailure()
                    {
                        ErrorMessage = "Uncaught exception while dumping object properties.",
                        ExceptionMessage = ex.Message,
                        ExceptionType = ex.GetType().FullName
                    });
            }

            if (failures.Count > 0)
            {
                this.children.Add(DumpFactory.GetNode(failures, "DumpFailures"));
            }
        }
    }
}