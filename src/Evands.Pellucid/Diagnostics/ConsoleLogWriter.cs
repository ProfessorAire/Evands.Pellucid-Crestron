// <copyright file="ConsoleLogWriter.cs">
// The MIT License
// Copyright © Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// A <see cref="ILogWriter"/> implementation that writes to <see cref="System.Console.Error"/>.
    /// This is the default log writer used when Crestron platform services are not available.
    /// </summary>
    public class ConsoleLogWriter : ILogWriter
    {
        /// <summary>
        /// Writes a debug message to the standard error stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteDebug(string message)
        {
            Console.Error.WriteLine("[DEBUG] {0}", message);
        }

        /// <summary>
        /// Writes a notice to the standard error stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteNotice(string message)
        {
            Console.Error.WriteLine("[NOTICE] {0}", message);
        }

        /// <summary>
        /// Writes a warning to the standard error stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteWarning(string message)
        {
            Console.Error.WriteLine("[WARNING] {0}", message);
        }

        /// <summary>
        /// Writes an error to the standard error stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteError(string message)
        {
            Console.Error.WriteLine("[ERROR] {0}", message);
        }
    }
}
