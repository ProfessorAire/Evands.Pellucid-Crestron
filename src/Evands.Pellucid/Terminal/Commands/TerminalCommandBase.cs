#region copyright
// <copyright file="TerminalCommandBase.cs" company="Christopher McNeely">
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

using System;

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// The base class for terminal commands that have no internal object needed.
    /// </summary>
    public abstract class TerminalCommandBase
    {
        /// <summary>
        /// Backing field for the <see cref="Name"/> property.
        /// </summary>
        private string name;

        /// <summary>
        /// Backing field for the <see cref="Alias"/> property.
        /// </summary>
        private string alias;

        /// <summary>
        /// Tracks the suffix provided with the command.
        /// </summary>
        private string suffix = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalCommandBase"/> class.
        /// </summary>
        public TerminalCommandBase()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalCommandBase"/> class.
        /// </summary>
        /// <param name="suffix">The suffix to provide this command's name with.</param>
        public TerminalCommandBase(string suffix)
        {
            if (!string.IsNullOrEmpty(suffix))
            {
                this.suffix = suffix;
                this.name = string.Format("{0}{1}", Name, suffix);
            }
        }

        /// <summary>
        /// Gets or sets the name of the command.
        /// <para>
        /// When setting the name this must be set prior to registering the command.
        /// </para>
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = Helpers.GetCommandNameFromAttribute(this);
                }

                return name;
            }

            protected internal set
            {
                if (GetCommandsRegisteredWith().Length == 0)
                {
                    name = value;
                }
                else
                {
                    throw new InvalidOperationException("You cannot set the name of a terminal command after it has been registered with a global command.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the alias for the command, if it exists.
        /// </summary>
        public string Alias
        {
            get
            {
                if (string.IsNullOrEmpty(alias))
                {
                    alias = Helpers.GetCommandAliasFromAttribute(this);
                    if (!string.IsNullOrEmpty(alias))
                    {
                        alias = string.Format("{0}{1}", alias, suffix);
                    }
                }

                return alias;
            }

            protected internal set
            {
                if (GetCommandsRegisteredWith().Length == 0)
                {
                    alias = value;
                }
                else
                {
                    throw new InvalidOperationException("You cannot set the alias of a terminal command after it has been registered with a global command.");
                }
            }
        }

        /// <summary>
        /// Gets an array of all the global commands this command is registered with.
        /// </summary>
        /// <returns>An array of global commands that this is registered with.</returns>
        public GlobalCommand[] GetCommandsRegisteredWith()
        {
            return Manager.ReturnAllTerminalCommandIsRegisteredTo(this);
        }

        /// <summary>
        /// Attempts to register the command with the specified global command.
        /// </summary>
        /// <param name="globalCommandName">The name of the global command to register this command with.</param>
        /// <returns>A value indicating whether the registration succeeded or the reason it failed.</returns>
        public RegisterResult RegisterCommand(string globalCommandName)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return RegisterResult.NoCommandAttributeFound;
            }

            return Manager.Register(globalCommandName, this);
        }

        /// <summary>
        /// Attempts to remove the command from all the global commands it is registered with.
        /// </summary>
        public void UnregisterCommand()
        {
            foreach (var gc in GetCommandsRegisteredWith())
            {
                Manager.Unregister(gc.Name, this);
            }
        }

        /// <summary>
        /// Attempts to remove the command from the specified global command.
        /// </summary>
        /// <param name="globalCommandName">The name of the global command to register this command with.</param>
        /// <returns><see langword="true"/> if the command was unregistered, <see langword="false"/> if the un-registration failed.</returns>
        public bool UnregisterCommand(string globalCommandName)
        {
            return Manager.Unregister(globalCommandName, this);
        }
    }
}