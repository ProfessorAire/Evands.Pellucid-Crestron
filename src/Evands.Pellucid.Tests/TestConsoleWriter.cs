using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evands.Pellucid
{
    public class TestConsoleWriter : Evands.Pellucid.Terminal.IConsoleWriter
    {
        public TestConsoleWriter()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }

        public void Write(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                Messages.Add(string.Format(message, args));
            }
            else
            {
                Messages.Add(message);
            }
        }

        public void WriteLine(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                Messages.Add(string.Format("{0}\r\n", string.Format(message, args)));
            }
            else
            {
                Messages.Add(string.Format("{0}\r\n", message));
            }
        }

        public void WriteLine()
        {
            Messages.Add("\r\n");
        }

        public void WriteCommandResponse(string message, params object[] args)
        {
            this.Write(message, args);
        }
    }
}
