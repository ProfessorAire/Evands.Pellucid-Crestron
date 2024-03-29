﻿#region copyright
// <copyright file="LoggerCommands.cs" company="Christopher McNeely">
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

using Crestron.SimplSharp;
using Evands.Pellucid.Terminal.Commands;
using Evands.Pellucid.Terminal.Commands.Attributes;
using Evands.Pellucid.Terminal.Formatting.Tables;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Terminal commands for Logging.
    /// </summary>        
    [Command("Logging", 3, "Configures logging parameters.")]
    public class LoggerCommands : TerminalCommandBase
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
            [Flag("Debug", 'd', "Include to toggle logging debug messages.", true)] bool debug,
            [Flag("Notices", 'n', "Include to toggle logging notices.", true)] bool notices,
            [Flag("Warnings", 'w', "Include to toggle logging warnings.", true)] bool warnings,
            [Flag("Errors", 'e', "Include to toggle logging errors.", true)] bool errors,
            [Flag("Exceptions", 'x', "Include to toggle logging exceptions.", true)] bool exceptions)
        {
            if (debug)
            {
                Options.Instance.LogLevels = Options.Instance.LogLevels ^ LogLevels.Debug;
            }

            if (notices)
            {
                Options.Instance.LogLevels = Options.Instance.LogLevels ^ LogLevels.Notice;
            }

            if (warnings)
            {
                Options.Instance.LogLevels = Options.Instance.LogLevels ^ LogLevels.Warning;
            }

            if (errors)
            {
                Options.Instance.LogLevels = Options.Instance.LogLevels ^ LogLevels.Error;
            }

            if (exceptions)
            {
                Options.Instance.LogLevels = Options.Instance.LogLevels ^ LogLevels.Exception;
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
            [Flag("Debug", 'd', "Include to enable logging debug messages.", true)] bool debug,
            [Flag("Notices", 'n', "Include to enable logging notices.", true)] bool notices,
            [Flag("Warnings", 'w', "Include to enable logging warnings.", true)] bool warnings,
            [Flag("Errors", 'e', "Include to enable logging errors.", true)] bool errors,
            [Flag("Exceptions", 'x', "Include to enable logging exceptions.", true)] bool exceptions,
            [Flag("All", 'a', "Include to enable logging all messages.", true)] bool all,
            [Flag("None", "Include to disable logging all messages.", true)] bool none)
        {
            if (all && none)
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightRed, "The '--all' and '--none' flags are mutually exclusive. You cannot include both of these.{0}", ConsoleBase.NewLine);
                return;
            }
            else if (all)
            {
                Options.Instance.LogLevels = LogLevels.All;
            }
            else if (none)
            {
                Options.Instance.LogLevels = LogLevels.None;
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
                    Options.Instance.LogLevels = Options.Instance.LogLevels | LogLevels.Debug;
                }
                else
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels & ~LogLevels.Debug;
                }

                if (notices)
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels | LogLevels.Notice;
                }
                else
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels & ~LogLevels.Notice;
                }

                if (warnings)
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels | LogLevels.Warning;
                }
                else
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels & ~LogLevels.Warning;
                }

                if (errors)
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels | LogLevels.Error;
                }
                else
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels & ~LogLevels.Error;
                }

                if (exceptions)
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels | LogLevels.Exception;
                }
                else
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels & ~LogLevels.Exception;
                }

                if ((int)Options.Instance.LogLevels > 1)
                {
                    Options.Instance.LogLevels = Options.Instance.LogLevels & ~LogLevels.None;
                }
                else
                {
                    Options.Instance.LogLevels = LogLevels.None;
                }
            }

            PrintCurrentConfiguration();
        }

        /// <summary>
        /// Prints the current logging configuration to the console.
        /// </summary>
        private void PrintCurrentConfiguration()
        {
            var dis = "Disabled";
            var ena = "Enabled";

            var table = Table.Create()
                .AddRow("Debug Messages", Options.Instance.LogLevels.Contains(LogLevels.Debug) ? ena : dis)
                .AddRow("Notices", Options.Instance.LogLevels.Contains(LogLevels.Notice) ? ena : dis)
                .AddRow("Warnings", Options.Instance.LogLevels.Contains(LogLevels.Warning) ? ena : dis)
                .AddRow("Errors", Options.Instance.LogLevels.Contains(LogLevels.Error) ? ena : dis)
                .AddRow("Exceptions", Options.Instance.LogLevels.Contains(LogLevels.Exception) ? ena : dis)
                .ForEachCellInColumn(1, c => c.Color = c.Contents == ena ? ConsoleBase.Colors.BrightGreen : ConsoleBase.Colors.BrightRed)
                .FormatColumn(0, HorizontalAlignment.Right)
                .FormatColumn(1, HorizontalAlignment.Left);

            table.Columns[0][0].Color = ConsoleBase.Colors.Debug;
            table.Columns[0][1].Color = ConsoleBase.Colors.Notice;
            table.Columns[0][2].Color = ConsoleBase.Colors.Warning;
            table.Columns[0][3].Color = ConsoleBase.Colors.Error;
            table.Columns[0][4].Color = ConsoleBase.Colors.Exception;

            ConsoleBase.WriteCommandResponse(table.ToString());
        }
    }
}