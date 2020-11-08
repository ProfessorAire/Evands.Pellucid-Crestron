using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Elegant.Pellucid
{
    /// <summary>
    /// Defines the requirements for a class that provides a terminal command.
    /// <para>This is an optional helper that can standardize the discovery of terminal commands when creating devices.</para>
    /// </summary>
    public interface ITerminalCommandProvider
    {
        /// <summary>
        /// Creates the internal terminal command this class provides.
        /// </summary>
        /// <param name="commandNameSuffix">String to add to the end of the command's name as a suffix.
        /// <para>If an empty string is provided the default command name is used with no suffix.</para></param>
        void CreateTerminalCommand(string commandNameSuffix);

        /// <summary>
        /// Registers the provided terminal command or commands with the global command with the specified name.
        /// </summary>
        /// <param name="globalCommandName">The name of the global command to register with.</param>
        RegisterResult RegisterTerminalCommand(string globalCommandName);
    }
}