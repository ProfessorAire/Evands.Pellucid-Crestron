#region copyright
// <copyright file="Manager.cs" company="Christopher McNeely">
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

using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Provides methods for managing commands.
    /// </summary>
    internal static class Manager
    {
        /// <summary>
        /// Dictionary of <see cref="GlobalCommand"/> objects registered for execution.
        /// </summary>
        private static Dictionary<string, GlobalCommand> consoleCommandNames = new Dictionary<string, GlobalCommand>(1);

        /// <summary>
        /// Creates a global command, adding it to the <see cref="CrestronConsole"/>.
        /// </summary>
        /// <param name="command">The <see cref="GlobalCommand"/> to register.</param>
        /// <returns><see langword="true"/> if the command is newly registered, <see langword="false"/> if it isn't registered because a command with that name already was.</returns>
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

        /// <summary>
        /// Gets an array of <see cref="GlobalCommand"/> objects that the specified <see cref="TerminalCommandBase"/> is registered to.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to check for registrations.</param>
        /// <returns>An array of <see cref="GlobalCommand"/> objects.</returns>
        public static GlobalCommand[] ReturnAllTerminalCommandIsRegisteredTo(TerminalCommandBase command)
        {
            return consoleCommandNames.Values.Where(gc => gc.IsCommandRegistered(command)).ToArray();
        }

        /// <summary>
        /// Removes the command with the specified name from the <see cref="CrestronConsole"/> and the <see cref="Manager"/>'s internal dictionary of commands.
        /// </summary>
        /// <param name="commandName">The name of the <see cref="GlobalCommand"/> to remove.</param>
        /// <returns><see langword="true"/> if the command was removed, otherwise <see langword="false"/>.</returns>
        public static bool RemoveCrestronConsoleCommand(string commandName)
        {
            CrestronConsole.RemoveConsoleCommand(commandName);
            return consoleCommandNames.Remove(commandName);
        }

        /// <summary>
        /// Registers the specified command with a <see cref="GlobalCommand"/> with the specified name.
        /// </summary>
        /// <param name="consoleCommandName">The name of the <see cref="GlobalCommand"/> to register the command with.</param>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to register with the specified <see cref="GlobalCommand"/>.</param>
        /// <returns>A <see cref="RegisterResult"/>.</returns>
        public static RegisterResult Register(string consoleCommandName, TerminalCommandBase command)
        {
            if (consoleCommandNames.ContainsKey(consoleCommandName))
            {
                return consoleCommandNames[consoleCommandName].AddCommand(command, command.Name);
            }

            return RegisterResult.GlobalCommandNotFound;
        }

        /// <summary>
        /// Unregisters a <see cref="TerminalCommandBase"/> from a registered <see cref="GlobalCommand"/> with the specified name.
        /// </summary>
        /// <param name="globalCommandName">The name of the <see cref="GlobalCommand"/> to unregister the command from.</param>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to unregister from the specified <see cref="GlobalCommand"/>.</param>
        /// <returns><see langword="true"/> if the un-registration succeeded, otherwise <see langword="false"/>.</returns>
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