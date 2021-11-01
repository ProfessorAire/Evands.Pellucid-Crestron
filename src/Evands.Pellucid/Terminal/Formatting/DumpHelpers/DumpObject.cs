using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using System.Collections.ObjectModel;
using Crestron.SimplSharp.Reflection;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    public class DumpObject : DumpNode
    {
        private List<DumpNode> children = new List<DumpNode>();

        public DumpObject(object value)
            : this(value, string.Empty)
        {
        }

        public DumpObject(object value, string name)
            : base(value, name)
        {
            this.Children = new ReadOnlyCollection<DumpNode>(this.children);
            this.PopulateChildren();
        }

        public ReadOnlyCollection<DumpNode> Children { get; private set; }

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
                typeName = this.Value.GetType().GetCType().FullName;
            }
            else
            {
                typeName = this.Value.GetType().GetCType().Name;
            }

            var underscoreLength = typeName.Length + 13 + this.children.Count.ToString().Length;

            typeName = string.Format(
                "{0} {1}",
                typeName,
                ConsoleBase.Colors.DumpObjectChrome.FormatText("({0} Properties)", this.Children.Count));

            if (!string.IsNullOrEmpty(this.Name))
            {
                sb.AppendFormat(
                    "{0} = ",
                    ConsoleBase.Colors.DumpPropertyName.FormatText(this.Name));
                padding += this.Name.Length + 3;
            }
            else if (this.Children.Count > 0)
            {
                padding += currentDepth * 2;
                sb.Append(' ', padding);
            }

            var padSb = new StringBuilder(padding);
            padSb.Append(' ', padding);

            sb.Append(ConsoleBase.Colors.DumpObjectDetail.FormatText(typeName));
            if ((maxDepth == 0 || currentDepth < maxDepth) && this.Children.Count > 0)
            {
                sb.Append(ConsoleBase.NewLine);
                sb.Append(' ', padding);
                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, "<"));
                sb.Append('-', underscoreLength);
                sb.Append(Formatting.ColorFormat.None.FormatText(false, ConsoleBase.NewLine));
            }

            var needLine = false;
            foreach (var child in this.Children)
            {
                if (child != null)
                {
                    if (needLine)
                    {
                        sb.Append(ConsoleBase.NewLine);
                    }

                    var str = child.ToString(maxDepth, currentDepth + 1, useFullTypeNames);
                    if (!string.IsNullOrEmpty(str))
                    {
                        sb.Append(' ', padding);
                        sb.Append(str.Replace(
                                    ConsoleBase.NewLine,
                                    string.Format(
                                        "{0}{1}",
                                        ConsoleBase.NewLine,
                                        padSb.ToString())));
                        needLine = true;
                    }
                }
            }

            if ((maxDepth == 0 || currentDepth < maxDepth) && this.Children.Count > 0)
            {
                sb.Append(ConsoleBase.NewLine);
                sb.Append(' ', padding);
                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, "-"));
                sb.Append('-', underscoreLength - 1);
                sb.Append(">");
                //sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, "-"));
                //sb.Append('-', underscoreLength);
            }

            if (currentDepth == 0)
            {
                sb.Append(ConsoleBase.NewLine);
            }

            return sb.ToString();
        }

        private void PopulateChildren()
        {
            try
            {
                if (this.Value != null)
                {
                    PropertyInfo[][] props = new PropertyInfo[2][];

                    props[0] = this.Value.GetType().GetCType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    props[1] = this.Value.GetType().GetCType().GetProperties(BindingFlags.Static | BindingFlags.Public);

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

                    try
                    {
                        for (var i = 0; i < props.Length; i++)
                        {
                            try
                            {
                                foreach (var prop in props[i])
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
                                        this.children.Add(DumpFactory.GetNode(prop.Name.PadRight(max), value));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}