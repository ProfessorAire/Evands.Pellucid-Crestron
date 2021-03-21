#region copyright
// <copyright file="TerminalCommandExceptionEventArgs.cs" company="Christopher McNeely">
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

namespace EVS.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Provides details about the command being executed and the exception encountered.
    /// </summary>
    public class TerminalCommandExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalCommandExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The <see cref="TerminalCommandException"/> to provide to the event.</param>
        /// <param name="commandContent">The content of the command that was being executed when the exception occurred.</param>
        public TerminalCommandExceptionEventArgs(TerminalCommandException exception, string commandContent)
        {
            Exception = exception;
            CommandContent = commandContent;
        }

        /// <summary>
        /// Gets the exception that occurred.
        /// </summary>
        public TerminalCommandException Exception { get; private set; }

        /// <summary>
        /// Gets the contents of the command that was being executed when the exception occurred.
        /// </summary>
        public string CommandContent { get; private set; }
    }
}