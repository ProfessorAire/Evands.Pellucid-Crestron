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
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        /// <summary>
        /// Registered console commands.
        /// </summary>
        private readonly Dictionary<string, Action<string>> commands = new Dictionary<string, Action<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The console reader thread.
        /// </summary>
        private Thread consoleReaderThread;

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
        public bool AddConsoleCommand(Action<string> action, string name, string help, int accessLevel)
        {
            if (string.IsNullOrEmpty(name) || action == null)
            {
                return false;
            }

            lock (commands)
            {
                commands[name] = action;
            }

            EnsureConsoleReaderStarted();
            return true;
        }

        /// <inheritdoc/>
        public void RemoveConsoleCommand(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                return;
            }

            lock (commands)
            {
                commands.Remove(commandName);
            }
        }

        /// <inheritdoc/>
        public DateTime GetLocalTime()
        {
            return DateTime.Now;
        }

        /// <inheritdoc/>
        public void SendControlSystemCommand(string command, ref string response)
        {
            response = string.Empty;
        }

        /// <summary>
        /// Starts the console reader thread if it is not already running.
        /// </summary>
        private void EnsureConsoleReaderStarted()
        {
            if (consoleReaderThread != null)
            {
                return;
            }

            if (!Environment.UserInteractive)
            {
                return;
            }

            consoleReaderThread = new Thread(ConsoleReaderLoop)
            {
                IsBackground = true,
                Name = "Pellucid Console Reader",
            };
            consoleReaderThread.Start();
        }

        /// <summary>
        /// Reads lines from the console and dispatches them to registered commands.
        /// </summary>
        private void ConsoleReaderLoop()
        {
            try
            {
                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    var trimmed = line.Trim();
                    if (trimmed.Length == 0)
                    {
                        continue;
                    }

                    var spaceIndex = trimmed.IndexOf(' ');
                    var commandName = spaceIndex >= 0 ? trimmed.Substring(0, spaceIndex) : trimmed;
                    var args = spaceIndex >= 0 ? trimmed.Substring(spaceIndex + 1) : string.Empty;

                    Action<string> action;
                    lock (commands)
                    {
                        commands.TryGetValue(commandName, out action);
                    }

                    if (action != null)
                    {
                        try
                        {
                            action(args);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("Error executing command '{0}': {1}", commandName, ex);
                        }
                    }
                }
            }
            catch (IOException)
            {
                // Non-interactive environment; exit gracefully.
            }
        }
    }
}
