#region copyright
// <copyright file="Options.cs" company="Christopher McNeely">
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
using System.Collections.Generic;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Evands.Pellucid.Diagnostics;
using Evands.Pellucid.Internal;

namespace Evands.Pellucid
{
    /// <summary>
    /// Provides access to options for various elements of the Pellucid console, logging, and debug classes.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Initializes static members of the <see cref="Options"/> class.
        /// </summary>        
        static Options()
        {
            Instance = Load();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>        
        public Options()
        {
            CrestronEnvironment.ProgramStatusEventHandler += (status) =>
                {
                    switch (status)
                    {
                        case eProgramStatusEventType.Stopping:
                        case eProgramStatusEventType.Paused:
                            Save();
                            break;
                        case eProgramStatusEventType.Resumed:
                            Load();
                            break;
                    }
                };
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="Options"/> class.
        /// </summary>        
        public static Options Instance { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="LogLevels"/> used to determine what items are written to the logs.
        /// </summary>        
        [TomlProperty("logging-levels")]
        public LogLevels LogLevels { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not console output should be colorized.
        /// <para>When using any modern terminal like Putty or PowerShell this should be true.
        /// The only case where this should be set to false is when connecting using a tool like
        /// text console in ToolBox.</para>
        /// </summary>
        [TomlProperty("console-colorizeOutput")]
        public bool ColorizeConsoleOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not debug messages will have timestamps prepended to them.
        /// </summary>        
        [TomlProperty("debugging-useTimestamps")]
        public bool UseTimestamps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether debug messages will use shorter 24 hour timestamps, or longer 12 hour timestamps.
        /// </summary>        
        [TomlProperty("debugging-shortTimestamps")]
        public bool Use24HourTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DebugLevels"/> used to determine what debug messages are written to the consoles.
        /// </summary>        
        [TomlProperty("debugging-levels")]
        public DebugLevels DebugLevels { get; set; }

        /// <summary>
        /// Gets or sets a list of headers that are suppressed and will not have debug messages of any level print to the console.
        /// </summary>        
        [TomlProperty("suppressed")]
        public List<string> Suppressed { get; set; }

        /// <summary>
        /// Gets or sets a list of headers that are exclusively allowed to print debug messages to the console.
        /// </summary>        
        [TomlProperty("allowed")]
        public List<string> Allowed { get; set; }

        /// <summary>
        /// Saves the options to a TOML file in the application directory.
        /// <para>This file is automatically saved when the program is shutting down.</para>
        /// </summary>        
        public void Save()
        {
            var path = Path.Combine(InitialParametersClass.ProgramDirectory.ToString(), "pellucid.console-options.toml");
            MinimalTomlParser.SerializeToDisk(this, path);
        }

        /// <summary>
        /// Loads the options from a TOML file in the application directory.
        /// <para>If it exists, this file is automatically loaded when the application first starts.</para>
        /// </summary>
        /// <returns>An <see cref="Options"/> object.</returns>        
        private static Options Load()
        {
            var path = Path.Combine(InitialParametersClass.ProgramDirectory.ToString(), "pellucid.console-options.toml");
            if (File.Exists(path))
            {
                try
                {
                    var options = MinimalTomlParser.DeserializeFromDisk<Options>(path);
                    return options;
                }
                catch (Exception ex)
                {
                    ConsoleBase.WriteLine("Exception while loading Evands.Pellucid options from disk. Using the defaults instead.\n{0}", ex.ToString());
                    return new Options().WithDefaults();
                }
            }
            else
            {
                return new Options().WithDefaults();
            }
        }

        /// <summary>
        /// Returns this <see cref="Options"/> object with all settings set to the defaults.
        /// </summary>
        /// <returns>This <see cref="Options"/> object.</returns>        
        private Options WithDefaults()
        {
            ColorizeConsoleOutput = true;
            UseTimestamps = true;
            Use24HourTime = true;
            LogLevels = Evands.Pellucid.Diagnostics.LogLevels.None;
            DebugLevels = DebugLevels.All;
            Suppressed = new List<string>();
            Allowed = new List<string>();
            return this;
        }
    }
}