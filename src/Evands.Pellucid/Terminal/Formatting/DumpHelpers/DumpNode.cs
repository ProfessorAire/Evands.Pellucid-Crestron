using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    public class DumpNode
    {
        public DumpNode(object value, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.Name = name;
            if (value is string)
            {
                this.Value = string.Format("\"{0}\"", value);
            }
            else
            {
                this.Value = value;
            }
        }

        public string Name { get; private set; }

        public object Value { get; private set; }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public string ToString(int maxDepth)
        {
            return this.ToString(maxDepth, 0);
        }

        public string ToString(bool useFullTypeNames)
        {
            return this.ToString(0, 0, useFullTypeNames);
        }

        public string ToString(int maxDepth, bool useFullTypeNames)
        {
            return this.ToString(maxDepth, 0, useFullTypeNames);
        }

        internal string ToString(int maxDepth, int currentDepth)
        {
            return this.ToString(maxDepth, currentDepth, true);
        }

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
                    "{0} = {1}",
                    ConsoleBase.Colors.DumpPropertyName.FormatText(this.Name),
                    ConsoleBase.Colors.DumpPropertyValue.FormatText(
                    this.Value != null ? this.Value.ToString() : "<null>"));
            }
        }
    }
}