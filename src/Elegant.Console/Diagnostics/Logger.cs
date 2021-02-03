#region copyright
// <copyright file="Logger.cs" company="Christopher McNeely">
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
using System.Text;
using Crestron.SimplSharp;
using Elegant.Pellucid;
using Elegant.Pellucid.Attributes;

namespace Silver.Diagnostics
{
    /// <summary>
    /// Logger used to log messages to the error log and, when debugging is enabled, to the console.
    /// </summary>
    public static class Logger
    {
#if DEBUG
        /// <summary>
        /// Backing field for the <see cref="Levels"/> property.
        /// </summary>        
        private static LogLevels levels = LogLevels.Debug;
#else
        /// <summary>
        /// Backing field for the <see cref="Levels"/> property.
        /// </summary>        
        private static LogLevels levels = LogLevels.All & ~LogLevels.Debug;
#endif

        /// <summary>
        /// Raised when the <see cref="Levels"/> property changes.
        /// </summary>
        public static event Action<LogLevelsChangedEventArgs> LevelsChanged;

        /// <summary>
        /// Gets or sets a value indicating what messages to the log will be suppressed or allowed.
        /// </summary>
        public static LogLevels Levels
        {
            get
            {
                return levels;
            }

            set
            {
                if (levels != value)
                {
                    levels = value;

                    var lc = LevelsChanged;
                    if (lc != null)
                    {
                        lc.Invoke(new LogLevelsChangedEventArgs(value));
                    }
                }
            }
        }

        /// <summary>
        /// Logs a debug message, if debug logging is allowed.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The debug message.</param>
        /// <param name="args">Optional array of formatting arguments.</param>        
        public static void LogDebug(object obj, string message, params object[] args)
        {
            if (Levels.Contains(LogLevels.Debug))
            {
                message = string.Format(message, args);
                var msg = string.Format("{0}{1}", Console.GetMessageHeader(obj), message);
                ErrorLog.Notice(msg);
            }
        }

        /// <summary>
        /// Writes a notice to the error log.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogNotice(object obj, string message, params object[] args)
        {
            if (Levels.Contains(LogLevels.Notices))
            {
                message = string.Format(message, args);
                var msg = string.Format("{0}{1}", Console.GetMessageHeader(obj), message);
                ErrorLog.Notice(msg);
            }
        }

        /// <summary>
        /// Writes a warning to the error log.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogWarning(object obj, string message, params object[] args)
        {
            if (Levels.Contains(LogLevels.Warnings))
            {
                message = string.Format(message, args);
                var msg = string.Format("{0}{1}", Console.GetMessageHeader(obj), message);
                ErrorLog.Warn(msg);
            }
        }

        /// <summary>
        /// Writes an error to the error log.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogError(object obj, string message, params object[] args)
        {
            if (Levels.Contains(LogLevels.Errors))
            {
                message = string.Format(message, args);
                var msg = string.Format("{0}{1}", Console.GetMessageHeader(obj), message);
                ErrorLog.Error(msg);
            }
        }

        /// <summary>
        /// Writes an exception to the error log (as a formatted error, not an exception).
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="ex">The exception to write.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogException(object obj, Exception ex, string message, params object[] args)
        {
            if (Levels.Contains(LogLevels.Exceptions))
            {
                message = string.Format(message, args);
                var msg = string.Format("{0}{1}", Console.GetMessageHeader(obj), message);
                var sb = new StringBuilder(message.Length + (ex.Message.Length * 2));

                sb.AppendLine(msg);

                int exceptionIndex = 0;

                while (ex != null)
                {
                    exceptionIndex++;
                    sb.AppendFormat("--------Exception {0}--------\x0d\x0a", exceptionIndex);
                    sb.AppendLine("          Message            ");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine("-----------------------------");
                    sb.AppendLine(ex.ToString());
                    sb.AppendFormat("-----------------------------\x0d\x0a");
                    sb.AppendLine();

                    ex = ex.InnerException;
                }

                ErrorLog.Error(sb.ToString());
            }
        }

        /// <summary>
        /// Creates all commands intended for terminal usage.
        /// </summary>
        public static void CreateLoggingTerminalCommands()
        {
            var cmd = new LoggerCommand();
            cmd.RegisterCommand("app");
            cmd.RegisterCommand("apa");
        }

        /// <summary>
        /// Terminal commands for Logging.
        /// </summary>        
        [Command("Logging", "Configures logging parameters.")]
        internal class LoggerCommand : TerminalCommandBase
        {
            /// <summary>
            /// Toggles the included terminal logging levels.
            /// </summary>
            /// <param name="debug">Toggles debug logging.</param>
            /// <param name="notices">Toggles notice logging.</param>
            /// <param name="warnings">Toggles warning logging.</param>
            /// <param name="errors">Toggles error logging.</param>
            /// <param name="exceptions">Toggles exception logging.</param>
            [Verb("Toggle", "Configures logging, toggling individual level settings.")]
            [Sample("logging toggle --notices --warnings --errors", "Toggles logging of notices, warnings, and errors.")]
            [Sample("logging toggle", "Prints the current logging configuration to the console.")]
            public void ToggleLevels(
                [Flag("Debug", "Include to toggle logging debug messages.", true)] bool debug,
                [Flag("Notices", "Include to toggle logging notices.", true)] bool notices,
                [Flag("Warnings", "Include to toggle logging warnings.", true)] bool warnings,
                [Flag("Errors", "Include to toggle logging errors.", true)] bool errors,
                [Flag("Exceptions", "Include to toggle logging exceptions.", true)] bool exceptions)
            {
                if (debug)
                {
                    Levels = Levels ^ LogLevels.Debug;
                }

                if (notices)
                {
                    Levels = Levels ^ LogLevels.Notices;
                }

                if (warnings)
                {
                    Levels = Levels ^ LogLevels.Warnings;
                }

                if (errors)
                {
                    Levels = Levels ^ LogLevels.Errors;
                }

                if (exceptions)
                {
                    Levels = Levels ^ LogLevels.Exceptions;
                }

                PrintCurrentConfiguration();
            }

            /// <summary>
            /// Configures the logging settings, using the included levels for enabled and using excluded levels for disabled.
            /// </summary>
            /// <param name="debug">Enables debug logging.</param>
            /// <param name="notices">Enables notice logging.</param>
            /// <param name="warnings">Enables warning logging.</param>
            /// <param name="errors">Enables error logging.</param>
            /// <param name="exceptions">Enables exception logging.</param>
            /// <param name="all">Enables all logging.</param>
            /// <param name="none">Disables all logging.</param>
            [Verb("Levels", "Configures logging, enabling or disabling individual levels.")]
            [Sample("logging levels --notices --warnings --errors", "Enables logging of notices, warnings, and errors. Disables logging of debug messages and exceptions.")]
            [Sample("logging levels --debug", "Enable logging of debug messages and disables all other logging.")]
            [Sample("logging levels", "Prints the current logging configuration to the console.")]
            public void ConfigureLevels(
                [Flag("Debug", "Include to enable logging debug messages.", true)] bool debug,
                [Flag("Notices", "Include to enable logging notices.", true)] bool notices,
                [Flag("Warnings", "Include to enable logging warnings.", true)] bool warnings,
                [Flag("Errors", "Include to enable logging errors.", true)] bool errors,
                [Flag("Exceptions", "Include to enable logging exceptions.", true)] bool exceptions,
                [Flag("All", "Include to enable logging all messages.", true)] bool all,
                [Flag("None", "Include to disable logging all messages.", true)] bool none)
            {
                if (all && none)
                {
                    Console.WriteCommandResponse(Console.Colors.BrightRed, "The '--all' and '--none' flags are mutually exclusive. You cannot include both of these.{0}", CrestronEnvironment.NewLine);
                    return;
                }
                else if (all)
                {
                    Levels = LogLevels.All;
                }
                else if (none)
                {
                    Levels = LogLevels.None;
                }
                else if (!debug && !notices && !warnings && !errors && !exceptions)
                {
                    PrintCurrentConfiguration();
                    return;
                }
                else
                {
                    if (debug)
                    {
                        Levels = Levels | LogLevels.Debug;
                    }
                    else
                    {
                        Levels = Levels & ~LogLevels.Debug;
                    }

                    if (notices)
                    {
                        Levels = Levels | LogLevels.Notices;
                    }
                    else
                    {
                        Levels = Levels & ~LogLevels.Notices;
                    }

                    if (warnings)
                    {
                        Levels = Levels | LogLevels.Warnings;
                    }
                    else
                    {
                        Levels = Levels & ~LogLevels.Warnings;
                    }

                    if (errors)
                    {
                        Levels = Levels | LogLevels.Errors;
                    }
                    else
                    {
                        Levels = Levels & ~LogLevels.Errors;
                    }

                    if (exceptions)
                    {
                        Levels = Levels | LogLevels.Exceptions;
                    }
                    else
                    {
                        Levels = Levels & ~LogLevels.Exceptions;
                    }
                }

                PrintCurrentConfiguration();
            }

            /// <summary>
            /// Prints the current logging configuration to the console.
            /// </summary>
            public void PrintCurrentConfiguration()
            {
                Console.WriteCommandResponse(
                    "Logging of {0} is {1}\r\n",
                    Console.GetColorFormattedString(Console.Colors.Debug, "Debug Messages"),
                    Levels.Contains(LogLevels.Debug) ? Console.GetColorFormattedString(Console.Colors.BrightGreen, "Enabled") : Console.GetColorFormattedString(Console.Colors.BrightRed, "Disabled"));

                Console.WriteCommandResponse(
                    "Logging of {0} are {1}\r\n",
                    Console.GetColorFormattedString(Console.Colors.Notice, "Notices"),
                    Levels.Contains(LogLevels.Notices) ? Console.GetColorFormattedString(Console.Colors.BrightGreen, "Enabled") : Console.GetColorFormattedString(Console.Colors.BrightRed, "Disabled"));

                Console.WriteCommandResponse(
                    "Logging of {0} are {1}\r\n",
                    Console.GetColorFormattedString(Console.Colors.Warning, "Warnings"),
                    Levels.Contains(LogLevels.Warnings) ? Console.GetColorFormattedString(Console.Colors.BrightGreen, "Enabled") : Console.GetColorFormattedString(Console.Colors.BrightRed, "Disabled"));

                Console.WriteCommandResponse(
                    "Logging of {0} are {1}\r\n",
                    Console.GetColorFormattedString(Console.Colors.Error, "Errors"),
                    Levels.Contains(LogLevels.Errors) ? Console.GetColorFormattedString(Console.Colors.BrightGreen, "Enabled") : Console.GetColorFormattedString(Console.Colors.BrightRed, "Disabled"));

                Console.WriteCommandResponse(
                    "Logging of {0} are {1}\r\n",
                    Console.GetColorFormattedString(Console.Colors.Exception, "Exceptions"),
                    Levels.Contains(LogLevels.Exceptions) ? Console.GetColorFormattedString(Console.Colors.BrightGreen, "Enabled") : Console.GetColorFormattedString(Console.Colors.BrightRed, "Disabled"));
            }
        }
    }
}