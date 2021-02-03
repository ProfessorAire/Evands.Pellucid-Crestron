#region copyright
// <copyright file="ControlSystem.cs" company="Christopher McNeely">
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
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;
using Elegant.Terminal.Commands;

namespace Elegant.Console.ProDemo
{
    /// <summary>
    /// Console Demo Control System
    /// </summary>
    public class ControlSystem : CrestronControlSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSystem"/> class.
        /// </summary>
        public ControlSystem()
            : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;

                // Setup the static instance reference for the control system.
                Instance = this;

                // Setup the global command(s).
                var appCommand = new GlobalCommand("app", "Application commands.", Access.Programmer);
                appCommand.AddToConsole();

                // Initialize specific global commands.
                ProConsole.InitializeCommands("app");

                var csc = new ControlSystemCommands();
                csc.RegisterCommand("app");

                // Subscribe to the controller events (System, Program, and Ethernet)
                CrestronEnvironment.SystemEventHandler += new SystemEventHandler(ControlSystem_ControllerSystemEventHandler);
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(ControlSystem_ControllerProgramEventHandler);
                CrestronEnvironment.EthernetEventHandler += new EthernetEventHandler(ControlSystem_ControllerEthernetEventHandler);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }
        
        /// <summary>
        /// Gets the program's <see cref="ControlSystem"/> class.
        /// </summary>
        public static ControlSystem Instance { get; private set; }

        /// <summary>
        /// InitializeSystem - this method gets called after the constructor has finished.
        /// </summary>
        public override void InitializeSystem()
        {
            CrestronInvoke.BeginInvoke(o =>
                {
                    try
                    {
                        ConsoleBase.WriteProgress("Initializing Application Logic.");

                        ConsoleBase.WriteProgress("Application Initialized.");
                    }
                    catch (Exception e)
                    {
                        ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
                    }
                });
        }

        /// <summary>
        /// Event Handler for Ethernet events: Link Up and Link Down. 
        /// Use these events to close / re-open sockets, etc. 
        /// </summary>
        /// <param name="ethernetEventArgs">This parameter holds the values 
        /// such as whether it's a Link Up or Link Down event. It will also indicate 
        /// which Ethernet adapter this event belongs to.
        /// </param>
        private void ControlSystem_ControllerEthernetEventHandler(EthernetEventArgs ethernetEventArgs)
        {
            // Determine the event type Link Up or Link Down
            switch (ethernetEventArgs.EthernetEventType)
            {
                case eEthernetEventType.LinkDown:
                    // Next need to determine which adapter the event is for. 
                    // LAN is the adapter is the port connected to external networks.
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {
                    }

                    break;
                case eEthernetEventType.LinkUp:
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {
                    }

                    break;
            }
        }

        /// <summary>
        /// Event Handler for Programmatic events: Stop, Pause, Resume.
        /// Use this event to clean up when a program is stopping, pausing, and resuming.
        /// This event only applies to this SIMPL#Pro program, it doesn't receive events
        /// for other programs stopping
        /// </summary>
        /// <param name="programStatusEventType">The event type.</param>
        private void ControlSystem_ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            switch (programStatusEventType)
            {
                case eProgramStatusEventType.Paused:
                    // The program has been paused.  Pause all user threads/timers as needed.
                    break;
                case eProgramStatusEventType.Resumed:
                    // The program has been resumed. Resume all the user threads/timers as needed.
                    break;
                case eProgramStatusEventType.Stopping:
                    // The program has been stopped.
                    // Close all threads. 
                    // Shutdown all Client/Servers in the system.
                    // General cleanup.
                    // Unsubscribe to all System Monitor events
                    break;
            }
        }

        /// <summary>
        /// Event Handler for system events, Disk Inserted/Ejected, and Reboot
        /// Use this event to clean up when someone types in reboot, or when your SD /USB
        /// removable media is ejected / re-inserted.
        /// </summary>
        /// <param name="systemEventType">The event type.</param>
        private void ControlSystem_ControllerSystemEventHandler(eSystemEventType systemEventType)
        {
            switch (systemEventType)
            {
                case eSystemEventType.DiskInserted:
                    // Removable media was detected on the system
                    break;
                case eSystemEventType.DiskRemoved:
                    // Removable media was detached from the system
                    break;
                case eSystemEventType.Rebooting:
                    // The system is rebooting. 
                    // Very limited time to preform clean up and save any settings to disk.
                    break;
            }
        }
    }
}