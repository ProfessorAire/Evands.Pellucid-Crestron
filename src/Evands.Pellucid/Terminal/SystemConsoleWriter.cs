// <copyright file="SystemConsoleWriter.cs">
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

namespace Evands.Pellucid.Terminal
{
    /// <summary>
    /// A <see cref="IConsoleWriter"/> implementation that writes to <see cref="System.Console"/>.
    /// This is the default writer used when Crestron platform services are not available.
    /// </summary>
    public class SystemConsoleWriter : IConsoleWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConsoleWriter"/> class.
        /// </summary>
        public SystemConsoleWriter()
        {
        }

        /// <inheritdoc/>
        public void Write(string message, params object[] args)
        {
            Console.Write(message, args);
        }

        /// <inheritdoc/>
        public void Write(string message)
        {
            Console.Write(message);
        }

        /// <inheritdoc/>
        public void WriteLine(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        /// <inheritdoc/>
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        /// <inheritdoc/>
        public void WriteLine()
        {
            Console.WriteLine();
        }

        /// <inheritdoc/>
        public void WriteCommandResponse(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        /// <inheritdoc/>
        public void WriteCommandResponse(string message)
        {
            Console.WriteLine(message);
        }
    }
}
