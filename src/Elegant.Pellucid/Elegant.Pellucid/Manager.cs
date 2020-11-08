#region copyright
// <copyright file="Manager.cs" company="Christopher McNeely">
// The MIT License (MIT)
// Copyright (c) Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
#endregion

using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;

namespace Elegant.Pellucid
{
    /// <summary>
    /// Provides methods for managing commands.
    /// </summary>
    internal static class Manager
    {
        private static Dictionary<string, GlobalCommand> consoleCommandNames = new Dictionary<string, GlobalCommand>(1);

        /// <summary>
        /// Creates a global command, adding it to the Crestron console.
        /// </summary>
        /// <param name="command">The command to register.</param>
        public static bool RegisterCrestronConsoleCommand(GlobalCommand command)
        {
            if (consoleCommandNames.ContainsKey(command.Name))
            {
                return false;
            }
            else
            {
                consoleCommandNames.Add(command.Name, command);
                return true;
            }
        }

        public static GlobalCommand[] ReturnAllTerminalCommandIsRegisteredTo(TerminalCommandBase command)
        {
            return consoleCommandNames.Values.Where(gc => gc.IsCommandRegistered(command)).ToArray();
        }

        public static bool RemoveCrestronConsoleCommand(string commandName)
        {
            return consoleCommandNames.Remove(commandName);
        }

        public static RegisterResult Register(string consoleCommandName, TerminalCommandBase command)
        {
            if (consoleCommandNames.ContainsKey(consoleCommandName))
            {
                return consoleCommandNames[consoleCommandName].AddCommand(command, command.Name);
            }

            return RegisterResult.GlobalCommandNotFound;
        }

        public static bool Unregister(string globalCommandName, TerminalCommandBase command)
        {
            if (consoleCommandNames.ContainsKey(globalCommandName))
            {
                return consoleCommandNames[globalCommandName].RemoveCommand(command.Name);
            }

            return false;
        }
    }
}