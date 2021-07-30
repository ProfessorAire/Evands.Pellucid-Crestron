using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evands.Pellucid
{
    public class TestLogWriter : Evands.Pellucid.Diagnostics.ILogWriter
    {
        public TestLogWriter()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }

        public bool Contains(string value)
        {
            return Messages.Any(m => m.Contains(value));
        }

        public string First() { return Messages.FirstOrDefault(); }

        public string Last() { return Messages.LastOrDefault(); }

        private void WriteLine(string message)
        {
            Messages.Add(message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var m in Messages)
            {
                sb.Append(m);
            }

            return sb.ToString();
        }

        public void WriteDebug(string message)
        {
            this.WriteLine(string.Format("[Debug]{0}", message));
        }

        public void WriteNotice(string message)
        {
            this.WriteLine(string.Format("[Notice]{0}", message));
        }

        public void WriteWarning(string message)
        {
            this.WriteLine(string.Format("[Warning]{0}", message));
        }

        public void WriteError(string message)
        {
            this.WriteLine(string.Format("[Error]{0}", message));
        }
    }
}
