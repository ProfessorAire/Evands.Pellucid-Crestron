// <copyright file="DefaultPlatformServices.cs">
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
using Evands.Pellucid.Diagnostics;
using Evands.Pellucid.Terminal;

namespace Evands.Pellucid
{
    /// <summary>
    /// Default platform services implementation that uses standard .NET APIs.
    /// Console output is written to <see cref="System.Console"/>, log output
    /// is written to <see cref="System.Console.Error"/>, and configuration
    /// defaults to the application base directory.
    /// </summary>
    internal class DefaultPlatformServices : IPlatformServices
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly DefaultPlatformServices Instance = new DefaultPlatformServices();

        /// <inheritdoc/>
        public IConsoleWriter CreateDefaultConsoleWriter()
        {
            return new SystemConsoleWriter();
        }

        /// <inheritdoc/>
        public ILogWriter CreateDefaultLogWriter()
        {
            return new ConsoleLogWriter();
        }

        /// <inheritdoc/>
        public string GetDefaultConfigDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <inheritdoc/>
        public string GetDefaultConfigFileName()
        {
            return "pellucid.console-options.toml";
        }

        /// <inheritdoc/>
        public void RegisterProgramStoppingHandler(Action handler)
        {
            // No program lifecycle events on the default platform.
        }

        /// <inheritdoc/>
        public void RegisterProgramResumingHandler(Action handler)
        {
            // No program lifecycle events on the default platform.
        }

        /// <inheritdoc/>
        public void UnregisterProgramHandlers(Action stoppingHandler, Action resumingHandler)
        {
            // No program lifecycle events on the default platform.
        }

        /// <inheritdoc/>
        public bool AddConsoleCommand(Action<string> action, string name, string help, int accessLevel)
        {
            // Console command registration is not supported on the default platform.
            return false;
        }

        /// <inheritdoc/>
        public void RemoveConsoleCommand(string commandName)
        {
            // Console command removal is not supported on the default platform.
        }

        /// <inheritdoc/>
        public DateTime GetLocalTime()
        {
            return DateTime.Now;
        }
    }
}
