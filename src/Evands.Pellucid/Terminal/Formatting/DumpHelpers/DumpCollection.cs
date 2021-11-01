using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using System.Collections;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    public class DumpCollection : DumpNode
    {
        private List<DumpNode> items = new List<DumpNode>();

        public DumpCollection(IEnumerable value, string name)
            : base(value, name)
        {
            this.PopulateItems();
        }

        public DumpCollection(IEnumerable value)
            : this(value, string.Empty)
        {
        }

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

            var underscoreLength = typeName.Length + 8 + this.items.Count.ToString().Length;

            typeName = string.Format(
                "{0} {1}",
                typeName,
                ConsoleBase.Colors.DumpObjectChrome.FormatText("({0} Items)", this.items.Count));

            if (!string.IsNullOrEmpty(this.Name))
            {
                sb.AppendFormat(
                    "{0} = ",
                    ConsoleBase.Colors.DumpPropertyName.FormatText(this.Name));
                padding += this.Name.Length + 3;
            }

            var padSb = new StringBuilder(padding);
            padSb.Append(' ', Math.Max(0, padding - (this.items.Count.ToString().Length + 2)));

            sb.Append(ConsoleBase.Colors.DumpObjectDetail.FormatText(typeName));
            if ((maxDepth == 0 || currentDepth < maxDepth) && this.items.Count > 0)
            {
                sb.Append(ConsoleBase.NewLine);
                sb.Append(' ', padding);
                sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, "-"));
                sb.Append('-', underscoreLength);
                sb.Append(Formatting.ColorFormat.None.FormatText(false, ConsoleBase.NewLine));
            }

            var needLine = false;
            var index = 0;
            foreach (var item in this.items)
            {
                if (item != null)
                {
                    if (needLine)
                    {
                        sb.Append(ConsoleBase.NewLine);
                    }

                    var str = item.ToString(maxDepth, currentDepth + 1, useFullTypeNames);
                    if (!string.IsNullOrEmpty(str))
                    {
                        sb.Append(' ', Math.Max(0, padding - (this.items.Count.ToString().Length + 2)));
                        sb.AppendFormat(
                            ConsoleBase.Colors.DumpObjectChrome.FormatText(
                                "{0}: ", index.ToString().PadLeft(this.items.Count.ToString().Length)));
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
            }

            //if ((maxDepth == 0 || currentDepth < maxDepth) && this.items.Count > 0)
            //{
            //    sb.Append(ConsoleBase.NewLine);
            //    sb.Append(' ', padding);
            //    sb.Append(ConsoleBase.Colors.DumpObjectChrome.FormatText(false, "."));
            //    sb.Append('.', typeName.Length - 1);
            //}

            return sb.ToString();
        }

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

        private void PopulateDictionary()
        {
            var idict = this.Value as IDictionary;
            if (idict == null || idict.Count == 0)
            {
                return;
            }

            var index = 0;
            var padding = idict.Count.ToString().Length + 5;
            foreach (var key in idict.Keys)
            {
                this.items.Add(DumpFactory.GetNode(string.Format("Key{0}", index).PadRight(padding, ' '), key));
                this.items.Add(DumpFactory.GetNode(string.Format("Value{0}", index).PadRight(padding, ' '), idict[key]));
                index++;
            }
        }

        private void PopulateEnumerable()
        {
            var ienum = this.Value as IEnumerable;
            if (ienum == null)
            {
                return;
            }

            foreach (var item in ienum)
            {
                items.Add(DumpFactory.GetNode(item));
            }
        }
    }
}