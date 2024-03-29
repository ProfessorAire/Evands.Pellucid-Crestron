﻿#region copyright
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
        /// Backing field for the <see cref="Instance"/> property.
        /// </summary>
        private static Options instance;

        /// <summary>
        /// Backing field for the <see cref="AutoSave"/> property.
        /// </summary>
        private bool autoSave = true;

        /// <summary>
        /// Backing field for the <see cref="DefaultLogTimestampFormat"/> property.
        /// </summary>
        private string defaultLogTimestampFormat = "yy/MM/dd HH:mm:ss";

        /// <summary>
        /// Backing field for the <see cref="MaxDebugMessageLength"/> property.
        /// </summary>
        private int maxDebugMessageLength = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>        
        public Options()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="Options"/> class.
        /// </summary>        
        public static Options Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Load();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the path to save the options file to.
        /// </summary>
        public static string FilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not console output should be colorized.
        /// <para>When using any modern terminal like Putty or PowerShell this should be true.
        /// The only case where this should be set to false is when connecting using a tool like
        /// text console in ToolBox.</para>
        /// </summary>
        [TomlProperty("console-colorizeOutput")]
        public bool ColorizeConsoleOutput { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LogLevels"/> used to determine what items are written to the logs.
        /// </summary>        
        [TomlProperty("logging-levels")]
        public LogLevels LogLevels { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DebugLevels"/> used to determine what debug messages are written to the consoles.
        /// </summary>        
        [TomlProperty("debugging-levels")]
        public DebugLevels DebugLevels { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not debug messages will have timestamps prepended to them.
        /// </summary>        
        [TomlProperty("debugging-useTimestamps")]
        public bool UseTimestamps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dumping items to the console will use full type names by default.
        /// </summary>
        [TomlProperty("dump-useFullTypeNames")]
        public bool UseFullTypeNamesWhenDumping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether debug messages will use shorter 24 hour timestamps, or longer 12 hour timestamps.
        /// </summary>        
        [TomlProperty("debugging-shortTimestamps")]
        public bool Use24HourTime { get; set; }

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
        /// Gets or sets a value indicating whether or not the options will be auto saved.
        /// </summary>
        [TomlProperty("autosave")]
        public bool AutoSave
        {
            get { return autoSave; }

            set
            {
                if (autoSave != value)
                {
                    autoSave = value;

                    if (value)
                    {
                        CrestronEnvironment.ProgramStatusEventHandler += HandleAutoLoad;
                    }
                    else
                    {
                        CrestronEnvironment.ProgramStatusEventHandler -= HandleAutoLoad;
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether or not methods that print to the console will
        /// attempt to parse Pellucid Console Markup tags prior to printing to the console.
        /// <para>View the project Github Wiki for more information about Pellucid Console Markup.</para>
        /// </summary>
        [TomlProperty("enableMarkup")]
        public bool EnableMarkup { get; set; }

        /// <summary>
        /// Gets or sets a value representing the default Date/Time format string to use when writing log
        /// messages to strings, using the <see cref="Terminal.Formatting.Logs.LogMessage"/> class.
        /// <para>
        /// Defaults to "yy/MM/dd HH:mm:ss". Uses standard .Net Date/Time format string characters.
        /// </para>
        /// </summary>
        [TomlProperty("defaultLogTimestampFormat")]
        public string DefaultLogTimestampFormat
        {
            get
            {
                return this.defaultLogTimestampFormat;
            }

            set
            {
                this.defaultLogTimestampFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there is a maximum length to the debug messages
        /// that can be printed to the console. Values greater than or equal to 20 are acceptable. Values lower
        /// than 20 will set this to -1, which indicates no limit.
        /// <para>Defaults to -1.</para>
        /// </summary>
        [TomlProperty("maxDebugMessageLength")]
        public int MaxDebugMessageLength
        {
            get
            {
                return this.maxDebugMessageLength;
            }

            set
            {
                this.maxDebugMessageLength = value >= 20 ? value : -1;
            }
        }

        /// <summary>
        /// Overrides the <see cref="Instance"/> property with a new instance of the <see cref="Options"/> class,
        /// with the default values and auto-save set to <see langword="false"/>. If called before accessing the
        /// <see cref="Instance"/> property this will prevent the program from attempting to load an existing file
        /// from disk.
        /// <para>
        /// This can be useful when writing unit-tests for items that call into this class.
        /// </para>
        /// </summary>
        public static void UseDefault()
        {
            instance = new Options().WithDefaults();
            instance.autoSave = false;
        }

        /// <summary>
        /// Saves the options to a TOML file to the location specified by the <see cref="FilePath"/> property.
        /// <para>This file is automatically saved when the program is shutting down if the <see cref="AutoSave"/> property is true.</para>
        /// </summary>        
        public void Save()
        {
            ValidateFilePath();

            try
            {
                MinimalTomlParser.SerializeToDisk(this, FilePath);
            }
            catch (Exception ex)
            {
                ErrorLog.Exception("Pellucid.Options", ex);
                Debug.WriteException(this, ex, "Exception while saving Pellucid.Options to disk.");
            }
        }

        /// <summary>
        /// Loads the options from a TOML file in the directory specified in the <see cref="FilePath"/> property.
        /// If the <see cref="FilePath"/> property is null or empty will default to "/USER/Pellucid/pellucid.console-options[id].toml",
        /// where the "01" is the application number, padded to 2 places on a processor and the RoomId on a server.
        /// <para>If it exists, this file is automatically loaded when the application first starts.</para>
        /// </summary>
        /// <returns>An <see cref="Options"/> object.</returns>        
        private static Options Load()
        {
            ValidateFilePath();

            if (File.Exists(FilePath))
            {
                try
                {
                    var options = MinimalTomlParser.DeserializeFromDisk<Options>(FilePath);
                    CrestronEnvironment.ProgramStatusEventHandler -= options.HandleAutoLoad;

                    if (options.AutoSave)
                    {
                        CrestronEnvironment.ProgramStatusEventHandler += options.HandleAutoLoad;
                    }

                    return options;
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("Options", "Exception while loading Evands.Pellucid options from disk. Using the defaults instead.\n{0}", ex.ToString());
                    return new Options().WithDefaults();
                }
            }
            else
            {
                return new Options().WithDefaults();
            }
        }

        /// <summary>
        /// Validates that the FilePath is not an empty string, setting it to the default value if it is.
        /// </summary>
        private static void ValidateFilePath()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                string fileName;
                if (CrestronEnvironment.DevicePlatform == eDevicePlatform.Appliance)
                {
                    fileName = string.Format("pellucid.console-options{0}.toml", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'));
                }
                else
                {
                    fileName = string.Format("pellucid.console-options{0}.toml", InitialParametersClass.RoomId);
                }

                FilePath = Path.Combine(Path.Combine("/USER", "Pellucid"), fileName);
            }
        }

        /// <summary>
        /// Returns this <see cref="Options"/> object with all settings set to the defaults.
        /// </summary>
        /// <returns>This <see cref="Options"/> object.</returns>        
        private Options WithDefaults()
        {
            this.UseFullTypeNamesWhenDumping = false;
            this.ColorizeConsoleOutput = true;
            this.UseTimestamps = true;
            this.Use24HourTime = true;
            this.LogLevels = LogLevels.None;
            this.DebugLevels = DebugLevels.All;
            this.Suppressed = new List<string>();
            this.Allowed = new List<string>();
            this.DefaultLogTimestampFormat = "yy/MM/dd HH:mm:ss";
            this.MaxDebugMessageLength = -1;
            return this;
        }

        /// <summary>
        /// Handles auto saving and loading.
        /// </summary>
        /// <param name="status">The status of the program.</param>
        private void HandleAutoLoad(eProgramStatusEventType status)
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
        }
    }
}