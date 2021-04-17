using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp
{
    public static class CrestronConsole
    {
        public static StringBuilder Messages = new StringBuilder();

        public static void RemoveConsoleCommand(string commandName)
        {
        }

        public static bool AddNewConsoleCommandResult = true;

        public static bool AddNewConsoleCommand(Action<string> action, string name, string help, ConsoleAccessLevelEnum access)
        {
            return AddNewConsoleCommandResult;
        }

        public static void ConsoleCommandResponse(string message)
        {
            Messages.Append(message);
        }

        public static void ConsoleCommandResponse(string message, params object[] args)
        {
            Messages.AppendFormat(message, args);
        }

        public static void Print(string message)
        {
            Messages.Append(message);
        }

        public static void Print(string message, params object[] args)
        {
            Messages.AppendFormat(message, args);
        }

        public static void PrintLine(string message)
        {
            Messages.AppendFormat("{0}\r\n", message);
        }

        public static void PrintLine(string message, params object[] args)
        {
            Messages.AppendFormat("{0}\r\n", string.Format(message, args));
        }
    }
}
