#region copyright
// <copyright file="TerminalCommandBase.cs" company="Christopher McNeely">
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

namespace Elegant.Pellucid
{
    /// <summary>
    /// The base class for terminal commands that have no internal object needed.
    /// </summary>
    public abstract class TerminalCommandBase
    {
        private string nameOverride;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalCommandBase"/> class.
        /// </summary>
        public TerminalCommandBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalCommandBase"/> class.
        /// </summary>
        /// <param name="name">The name to override this command with.</param>
        public TerminalCommandBase(string name)
        {
            nameOverride = name;
        }

        /// <summary>
        /// Attempts to register the command with the specified console command.
        /// </summary>
        /// <param name="consoleCommandName">The name of the console command to register this command with.</param>
        /// <returns>A value indicating whether the registration suceeded or the reason it failed.</returns>
        public RegisterResult RegisterCommand(string consoleCommandName)
        {
            if (string.IsNullOrEmpty(nameOverride))
            {
                return Manager.Register(consoleCommandName, this);
            }
            else
            {
                return Manager.Register(consoleCommandName, this, nameOverride);
            }
        }
    }
}