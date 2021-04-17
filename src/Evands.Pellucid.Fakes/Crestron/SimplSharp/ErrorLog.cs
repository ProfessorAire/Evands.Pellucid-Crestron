using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp
{
    public static class ErrorLog
    {
        public static StringBuilder logMessages = new StringBuilder();

        public static void Notice(string message)
        {
            logMessages.AppendFormat("Notice: {0}\r\n", message);
        }

        public static void Warn(string message)
        {
            logMessages.AppendFormat("Warning: {0}\r\n", message);
        }

        public static void Error(string message)
        {
            logMessages.AppendFormat("Error: {0}\r\n", message);
        }
    }
}
