// <copyright file="CrestronPlatformServices.cs">
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
using Crestron.SimplSharp;
using Evands.Pellucid.Diagnostics;
using Evands.Pellucid.Terminal;

namespace Evands.Pellucid
{
    /// <summary>
    /// Crestron-specific platform services implementation.
    /// Provides Crestron console writing, error log writing, Crestron-specific
    /// configuration paths, program lifecycle hooks, and console command registration.
    /// </summary>
    public class CrestronPlatformServices : IPlatformServices
    {
        /// <summary>
        /// Tracks registered stopping handlers for cleanup.
        /// </summary>
        private readonly List<Action> stoppingHandlers = new List<Action>();

        /// <summary>
        /// Tracks registered resuming handlers for cleanup.
        /// </summary>
        private readonly List<Action> resumingHandlers = new List<Action>();

        /// <summary>
        /// Initializes the Crestron platform services and registers them as the current platform.
        /// Call this method early in your program initialization (e.g., in InitializeSystem).
        /// </summary>
        public static void Initialize()
        {
            PlatformServices.Current = new CrestronPlatformServices();
        }

        private CrestronPlatformServices()
        {
            CrestronEnvironment.ProgramStatusEventHandler += (args) =>
            {
                switch (args)
                {
                    case eProgramStatusEventType.Stopping or eProgramStatusEventType.Paused:
                        {
                            foreach (var handler in this.stoppingHandlers)
                            {
                                handler();
                            }

                            break;
                        }

                    default:
                        if (args is eProgramStatusEventType.Resumed)
                        {
                            foreach (var handler in this.resumingHandlers)
                            {
                                handler();
                            }
                        }

                        break;
                }
            };
        }

        /// <inheritdoc/>
        public IConsoleWriter CreateDefaultConsoleWriter()
        {
            return new CrestronConsoleWriter();
        }

        /// <inheritdoc/>
        public ILogWriter CreateDefaultLogWriter()
        {
            return new CrestronLogWriter();
        }

        /// <inheritdoc/>
        public string GetDefaultConfigDirectory()
        {
            return Path.Combine("/USER", "Pellucid");
        }

        /// <inheritdoc/>
        public string GetDefaultConfigFileName()
        {
            if (CrestronEnvironment.DevicePlatform == eDevicePlatform.Appliance)
            {
                return string.Format(
                    "pellucid.console-options{0}.toml",
                    InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'));
            }
            else
            {
                return string.Format(
                    "pellucid.console-options{0}.toml",
                    InitialParametersClass.RoomId);
            }
        }

        /// <summary>
        /// Registers a handler to be called when the program is stopping or pausing.
        /// </summary>
        /// <param name="handler">The handler to invoke on program stop/pause.</param>
        public void RegisterProgramStoppingHandler(Action handler)
        {
            this.stoppingHandlers.Add(handler);
        }

        /// <summary>
        /// Registers a handler to be called when the program is resuming.
        /// </summary>
        /// <param name="handler">The handler to invoke on program resume.</param>
        public void RegisterProgramResumingHandler(Action handler)
        {
            this.resumingHandlers.Add(handler);
        }

        /// <summary>
        /// Unregisters the stopping and resuming handlers that were previously registered.
        /// </summary>
        /// <param name="stoppingHandler">The stopping handler to unregister.</param>
        /// <param name="resumingHandler">The resuming handler to unregister.</param>
        public void UnregisterProgramHandlers(Action stoppingHandler, Action resumingHandler)
        {
            this.stoppingHandlers.Remove(stoppingHandler);
            this.resumingHandlers.Remove(resumingHandler);
        }

        /// <inheritdoc/>
        public bool AddConsoleCommand(Action<string> action, string name, string help, int accessLevel)
        {
            var level = (ConsoleAccessLevelEnum)accessLevel;
            return CrestronConsole.AddNewConsoleCommand(action, name, help, level)
                || CrestronEnvironment.DevicePlatform == eDevicePlatform.Server;
        }

        /// <inheritdoc/>
        public void RemoveConsoleCommand(string commandName)
        {
            CrestronConsole.RemoveConsoleCommand(commandName);
        }

        /// <inheritdoc/>
        public DateTime GetLocalTime()
        {
            return CrestronEnvironment.GetLocalTime();
        }

        /// <inheritdoc/>
        public void SendControlSystemCommand(string command, ref string response)
        {
            CrestronConsole.SendControlSystemCommand(command, ref response);
        }

        /// <summary>
        /// Gets whether the current platform is an appliance (as opposed to a server/virtual platform).
        /// </summary>
        public bool IsAppliance
        {
            get { return CrestronEnvironment.DevicePlatform == eDevicePlatform.Appliance; }
        }

        /// <summary>
        /// Gets whether the current platform is a 3-Series system.
        /// </summary>
        public bool IsSeries3
        {
            get { return CrestronEnvironment.ProgramCompatibility == eCrestronSeries.Series3; }
        }

        /// <summary>
        /// Gets whether the current platform is a 4-Series system.
        /// </summary>
        public bool IsSeries4
        {
            get { return (CrestronEnvironment.ProgramCompatibility & eCrestronSeries.Series4) == eCrestronSeries.Series4; }
        }
    }
}
