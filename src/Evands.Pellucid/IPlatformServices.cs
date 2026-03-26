// <copyright file="IPlatformServices.cs">
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
    /// Defines platform-specific services used by the Pellucid framework.
    /// Implementations provide platform-specific behavior for console writing,
    /// logging, configuration, lifecycle hooks, and console command registration.
    /// </summary>
    public interface IPlatformServices
    {
        /// <summary>
        /// Creates the default <see cref="IConsoleWriter"/> for this platform.
        /// </summary>
        /// <returns>An <see cref="IConsoleWriter"/> instance.</returns>
        IConsoleWriter CreateDefaultConsoleWriter();

        /// <summary>
        /// Creates the default <see cref="ILogWriter"/> for this platform.
        /// </summary>
        /// <returns>An <see cref="ILogWriter"/> instance.</returns>
        ILogWriter CreateDefaultLogWriter();

        /// <summary>
        /// Gets the default directory path for storing configuration files on this platform.
        /// </summary>
        /// <returns>The default configuration directory path.</returns>
        string GetDefaultConfigDirectory();

        /// <summary>
        /// Gets the default configuration file name on this platform.
        /// </summary>
        /// <returns>The default configuration file name.</returns>
        string GetDefaultConfigFileName();

        /// <summary>
        /// Registers a handler to be called when the program is stopping or pausing.
        /// </summary>
        /// <param name="handler">The handler to invoke on program stop/pause.</param>
        void RegisterProgramStoppingHandler(Action handler);

        /// <summary>
        /// Registers a handler to be called when the program is resuming.
        /// </summary>
        /// <param name="handler">The handler to invoke on program resume.</param>
        void RegisterProgramResumingHandler(Action handler);

        /// <summary>
        /// Unregisters the stopping and resuming handlers that were previously registered.
        /// </summary>
        /// <param name="stoppingHandler">The stopping handler to unregister.</param>
        /// <param name="resumingHandler">The resuming handler to unregister.</param>
        void UnregisterProgramHandlers(Action stoppingHandler, Action resumingHandler);

        /// <summary>
        /// Attempts to register a console command with the platform's console.
        /// </summary>
        /// <param name="action">The action to execute when the command is invoked.</param>
        /// <param name="name">The name of the console command.</param>
        /// <param name="help">The help text for the console command.</param>
        /// <param name="accessLevel">The access level required (cast from the Access enum).</param>
        /// <returns><see langword="true"/> if the command was registered successfully; otherwise <see langword="false"/>.</returns>
        bool AddConsoleCommand(Action<string> action, string name, string help, int accessLevel);

        /// <summary>
        /// Removes a console command from the platform's console.
        /// </summary>
        /// <param name="commandName">The name of the console command to remove.</param>
        void RemoveConsoleCommand(string commandName);

        /// <summary>
        /// Gets the current local time from the platform.
        /// </summary>
        /// <returns>The current local <see cref="DateTime"/>.</returns>
        DateTime GetLocalTime();

        /// <summary>
        /// Sends a command to the platform's control system and retrieves the response.
        /// On Crestron, this sends a command to the CrestronConsole.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <param name="response">The response from the control system.</param>
        void SendControlSystemCommand(string command, ref string response);

        /// <summary>
        /// Gets whether the current platform is an appliance (as opposed to a server/virtual platform).
        /// </summary>
        bool IsAppliance { get; }

        /// <summary>
        /// Gets whether the current platform is a 3-Series system.
        /// </summary>
        bool IsSeries3 { get; }

        /// <summary>
        /// Gets whether the current platform is a 4-Series system.
        /// </summary>
        bool IsSeries4 { get; }
    }
}
