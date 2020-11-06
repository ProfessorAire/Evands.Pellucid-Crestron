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

namespace Elegant.Pellucid
{
    /// <summary>
    /// Provides methods for managing commands.
    /// </summary>
    internal static class Manager
    {
        private static Dictionary<string, GlobalCommand> consoleCommandNames = new Dictionary<string, GlobalCommand>(1);

        public static bool CreateCrestronConsoleCommand(string commandName, string commandHelp, CommandAccess access)
        {
            if (consoleCommandNames.ContainsKey(commandName))
            {
                return false;
            }
            else
            {
                var cmd = new GlobalCommand(commandName, commandHelp, access);
                if (cmd.AddToConsole())
                {
                    consoleCommandNames.Add(commandName, cmd);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool RemoveCrestronConsoleCommand(string commandName)
        {
            if (consoleCommandNames.ContainsKey(commandName))
            {
                consoleCommandNames[commandName].RemoveFromConsole();
            }

            return consoleCommandNames.Remove(commandName);
        }

        public static RegisterResult Register(string consoleCommandName, TerminalCommandBase command)
        {
            if (consoleCommandNames.ContainsKey(consoleCommandName))
            {
                return consoleCommandNames[consoleCommandName].AddCommand(command);
            }

            return RegisterResult.GlobalCommandNotFound;
        }

        public static RegisterResult Register(string consoleCommandName, TerminalCommandBase command, string name)
        {
            if (consoleCommandNames.ContainsKey(consoleCommandName))
            {
                return consoleCommandNames[consoleCommandName].AddCommand(command, name);
            }

            return RegisterResult.GlobalCommandNotFound;
        }        
    }
}