#region copyright
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
using EVS.Pellucid.Terminal.Commands;
using EVS.Pellucid.Terminal.Commands.Attributes;

namespace EVS.Pellucid.Diagnostics
{
    /// <summary>
    /// Terminal commands for Logging.
    /// </summary>        
    [Command("Logging", "Configures logging parameters.")]
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
            [Flag("Debug", "Include to toggle logging debug messages.", true)] bool debug,
            [Flag("Notices", "Include to toggle logging notices.", true)] bool notices,
            [Flag("Warnings", "Include to toggle logging warnings.", true)] bool warnings,
            [Flag("Errors", "Include to toggle logging errors.", true)] bool errors,
            [Flag("Exceptions", "Include to toggle logging exceptions.", true)] bool exceptions)
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
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightRed, "The '--all' and '--none' flags are mutually exclusive. You cannot include both of these.{0}", CrestronEnvironment.NewLine);
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
            }

            PrintCurrentConfiguration();
        }

        /// <summary>
        /// Prints the current logging configuration to the console.
        /// </summary>
        private void PrintCurrentConfiguration()
        {
            ConsoleBase.WriteCommandResponse(
                "Logging of {0} is {1}\r\n",
                ConsoleBase.Colors.Debug.FormatText("Debug Messages"),
                Options.Instance.LogLevels.Contains(LogLevels.Debug) ? ConsoleBase.Colors.BrightGreen.FormatText("Enabled") : ConsoleBase.Colors.BrightRed.FormatText("Disabled"));

            ConsoleBase.WriteCommandResponse(
                "Logging of {0} are {1}\r\n",
                ConsoleBase.Colors.Notice.FormatText("Notices"),
                Options.Instance.LogLevels.Contains(LogLevels.Notice) ? ConsoleBase.Colors.BrightGreen.FormatText("Enabled") : ConsoleBase.Colors.BrightRed.FormatText("Disabled"));

            ConsoleBase.WriteCommandResponse(
                "Logging of {0} are {1}\r\n",
                ConsoleBase.Colors.Warning.FormatText("Warnings"),
                Options.Instance.LogLevels.Contains(LogLevels.Warning) ? ConsoleBase.Colors.BrightGreen.FormatText("Enabled") : ConsoleBase.Colors.BrightRed.FormatText("Disabled"));

            ConsoleBase.WriteCommandResponse(
                "Logging of {0} are {1}\r\n",
                ConsoleBase.Colors.Error.FormatText("Errors"),
                Options.Instance.LogLevels.Contains(LogLevels.Error) ? ConsoleBase.Colors.BrightGreen.FormatText("Enabled") : ConsoleBase.Colors.BrightRed.FormatText("Disabled"));

            ConsoleBase.WriteCommandResponse(
                "Logging of {0} are {1}\r\n",
                ConsoleBase.Colors.Exception.FormatText("Exceptions"),
                Options.Instance.LogLevels.Contains(LogLevels.Exception) ? ConsoleBase.Colors.BrightGreen.FormatText("Enabled") : ConsoleBase.Colors.BrightRed.FormatText("Disabled"));
        }
    }
}