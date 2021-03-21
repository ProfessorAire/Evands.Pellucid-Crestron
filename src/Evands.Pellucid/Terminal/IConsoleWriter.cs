#region copyright
// <copyright file="IConsoleWriter.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal
{
    /// <summary>
    /// Defines the requirements for an object that allows writing to a console of some sort.
    /// <para>Typically this will be the default <see cref="CrestronConsoleWriter"/>, but additional writers can be
    /// created for writing to something like an open network port, for example.</para>
    /// </summary>
    public interface IConsoleWriter
    {
        /// <summary>
        /// Writes a message to the console, without a line termination.
        /// </summary>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        void Write(string message, params object[] args);

        /// <summary>
        /// Writes a message to the console, with a line termination.
        /// </summary>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        void WriteLine(string message, params object[] args);

        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes a console command response to the console.
        /// <para>Most writers will recycle their normal console writer commands for this.
        /// This is provided specifically to accommodate the Crestron console class.</para>
        /// </summary>
        /// <param name="message">The message response to write to the console.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        void WriteCommandResponse(string message, params object[] args);
    }
}