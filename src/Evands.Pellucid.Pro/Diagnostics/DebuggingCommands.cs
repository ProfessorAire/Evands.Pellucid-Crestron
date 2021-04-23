#region copyright
// <copyright file="DebuggingCommands.cs" company="Christopher McNeely">
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

using Evands.Pellucid.Terminal.Commands;
using Evands.Pellucid.Terminal.Commands.Attributes;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Console commands for configuring debugging.
    /// </summary>
    [Command("Debug", "Provides methods for configuring program debugging.")]
    public class DebuggingCommands : TerminalCommandBase
    {
        /// <summary>
        /// Configures debugging.
        /// </summary>
        /// <param name="timestamps">Whether to enable timestamps.</param>
        /// <param name="noStamps">Whether to disable timestamps.</param>
        /// <param name="shortTime">Whether to enable 24 hour timestamps.</param>
        /// <param name="longTime">Whether to disable 24 hour timestamps.</param>
        [Verb("Config", "Configures debugging.")]
        [Sample("debug config --useTimestamps --use24HourTime", "Enables timestamps, and the use of 24 hour timestamps.")]
        [Sample("debug config -tu", "Enables timestamps, and the use of 24 hour timestamps.")]
        [Sample("debug config -n", "Disables timestamps.")]
        [Sample("debug config -l", "Sets timestamps to be printed with long times, in the pattern '12:34:56 PM'")]
        public void Config(
            [Flag("UseTimestamps", 't', "Include to enable the addition of timestamps to debugging messages.", true)] bool timestamps,
            [Flag("NoTimestamps", 'n', "Include to disable the addition of timestamps to debugging messages. Overrides the 'UseTimestamps' flag.", true)] bool noStamps,
            [Flag("Use24HourTime", 'u', "Include to enable the printing of timestamps in 24 Hour Time.", true)] bool shortTime,
            [Flag("UseLongTime", 'l', "Include to enable the use of standard time stamps. Overrides the 'Use24HourTime' flag.")] bool longTime)
        {
            if (timestamps)
            {
                Options.Instance.UseTimestamps = true;
            }

            if (noStamps)
            {
                Options.Instance.UseTimestamps = false;
            }

            if (shortTime)
            {
                Options.Instance.Use24HourTime = true;
            }

            if (longTime)
            {
                Options.Instance.Use24HourTime = false;
            }

            CurrentStatus();
        }

        /// <summary>
        /// Toggles the included debugging levels.
        /// </summary>
        /// <param name="debug">Toggles debug level.</param>
        /// <param name="progress">Toggles progress level.</param>
        /// <param name="success">Toggles success level.</param>
        /// <param name="error">Toggles error level.</param>
        /// <param name="notice">Toggles notice level.</param>
        /// <param name="warning">Toggles warning level.</param>
        /// <param name="exception">Toggles exception level.</param>
        /// <param name="uncat">Toggles uncategorized level.</param>
        /// <param name="all">Toggles all levels.</param>
        [Verb("Toggle", "Configures debugging, toggling individual level settings.")]
        [Sample("debug toggle --error -nx", "Toggles error, notice, and exception messages.")]
        public void ToggleLevels(
            [Flag("debug", 'd', "Include to toggle printing debug messages.", true)] bool debug,
            [Flag("progress", 'p', "Include to toggle printing progress messages.", true)] bool progress,
            [Flag("success", 's', "Include to toggle printing success messages.", true)] bool success,
            [Flag("error", 'e', "Include to toggle printing error messages.", true)] bool error,
            [Flag("notice", 'n', "Include to toggle printing notice messages.", true)] bool notice,
            [Flag("warning", 'w', "Include to toggle printing warning messages.", true)] bool warning,
            [Flag("exception", 'x', "Include to toggle printing exception messages.", true)] bool exception,
            [Flag("uncat", 'u', "Include to toggle printing uncategorized debugging messages.", true)] bool uncat,
            [Flag("all", 'a', "Include to toggle printing all messages.", true)] bool all)
        {
            if (debug || all)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Debug;
            }

            if (progress || all)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Progress;
            }

            if (success || all)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Success;
            }

            if (error || all)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Error;
            }

            if (notice || all)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Notice;
            }

            if (warning || all)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Warning;
            }

            if (exception || all)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Exception;
            }

            if (uncat)
            {
                Options.Instance.DebugLevels = Options.Instance.DebugLevels ^ DebugLevels.Uncategorized;
            }

            CurrentStatus();
        }

        /// <summary>
        /// Configures the included debugging levels, using the included levels for enabled and excluded for disabled.
        /// </summary>
        /// <param name="debug">Enables debug level.</param>
        /// <param name="progress">Enables progress level.</param>
        /// <param name="success">Enables success level.</param>
        /// <param name="error">Enables error level.</param>
        /// <param name="notice">Enables subtle level.</param>
        /// <param name="warning">Enables warning level.</param>
        /// <param name="exception">Enables exception level.</param>
        /// <param name="uncat">Enables uncategorized level.</param>
        /// <param name="all">Enables all levels.</param>
        /// <param name="none">Disables all levels.</param>
        [Verb("Levels", "Configures logging, enabling or disabling individual level settings.")]
        [Sample("debug levels --error -nx", "Enables error, notice, and exception messages.")]
        public void ToggleLevels(
            [Flag("debug", 'd', "Include to enable printing debug messages. Exclude to disable.", true)] bool debug,
            [Flag("progress", 'p', "Include to enable printing progress messages. Exclude to disable.", true)] bool progress,
            [Flag("success", 's', "Include to enable printing success messages. Exclude to disable.", true)] bool success,
            [Flag("error", 'e', "Include to enable printing error messages. Exclude to disable.", true)] bool error,
            [Flag("notice", 'n', "Include to enable printing notice messages. Exclude to disable.", true)] bool notice,
            [Flag("warning", 'w', "Include to enable printing warning messages. Exclude to disable.", true)] bool warning,
            [Flag("exception", 'x', "Include to enable printing exception messages. Exclude to disable.", true)] bool exception,
            [Flag("uncat", 'u', "Include to enable printing uncategorized debugging messages. Exclude to disable.", true)] bool uncat,
            [Flag("all", 'a', "Include to enable printing all messages. Exclude to disable.", true)] bool all,
            [Flag("none", 'n', "Include to disable printing all messages. Exclude to disable.", true)] bool none)
        {
            if (all && none)
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightRed, "The '--all' and '--none' flags are mutually exclusive. You cannot include both of these.\r\n");
                return;
            }
            else if (all)
            {
                Options.Instance.DebugLevels = DebugLevels.All;
            }
            else if (none)
            {
                Options.Instance.DebugLevels = DebugLevels.None;
            }
            else if (!debug && !progress && !success && !error && !notice && !exception && !uncat)
            {
                CurrentStatus();
                return;
            }
            else
            {
                if (debug)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Debug;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Debug;
                }

                if (progress)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Progress;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Progress;
                }

                if (success)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Success;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Success;
                }

                if (error)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Error;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Error;
                }

                if (notice)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Notice;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Notice;
                }

                if (warning)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Warning;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Warning;
                }

                if (exception)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Exception;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Exception;
                }

                if (uncat)
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels | DebugLevels.Uncategorized;
                }
                else
                {
                    Options.Instance.DebugLevels = Options.Instance.DebugLevels & ~DebugLevels.Uncategorized;
                }
            }

            CurrentStatus();
        }

        /// <summary>
        /// Gets the current debugging state.
        /// </summary>
        [Verb("Config", "Configures debugging.")]
        [Sample("debug config", "Lists the current debug messaging status.")]
        public void CurrentStatus()
        {
            ConsoleBase.WriteCommandResponse(
                ConsoleBase.Colors.Progress,
                "Timestamps are {0}.\r\n24 Hour Timestamps are {0}.\r\n",
                Options.Instance.UseTimestamps ? "enabled" : "disabled",
                Options.Instance.Use24HourTime ? "enabled" : "disabled");

            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "Debug Levels = '{0}'\r\n", Options.Instance.DebugLevels);
       }

        /// <summary>
        /// Adds a suppression.
        /// </summary>
        /// <param name="valueToAdd">The suppression to add.</param>
        [Verb("Suppress", "Adds or removes objects from the suppression list. Item in this list aren't allowed to print debug messages to the console.")]
        [Sample("debug suppress --add \"App\"", "Adds the name \"App\" to the suppression list, which will prevent any class registered with that name from printing debugging messages.")]
        public void AddSuppress(
            [Operand("Add", "Adds the name that follows to the suppression list.")] string valueToAdd)
        {
            if (Debug.AddSuppression(valueToAdd))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Added the value '{0}' to the suppression list.", valueToAdd);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value '{0}' was already in the suppression list.", valueToAdd);
            }
        }

        /// <summary>
        /// Removes a suppression.
        /// </summary>
        /// <param name="valueToRemove">The suppression to remove.</param>
        [Verb("Suppress", "Adds or removes objects from the suppression list. Item in this list aren't allowed to print debug messages to the console.")]
        [Sample("debug suppress --remove \"App\"", "Removes the name \"App\" from the suppression list.")]
        public void RemoveSuppress(
            [Operand("Remove", "Removes the name that follows from the suppression list.")] string valueToRemove)
        {
            if (Debug.RemoveSuppression(valueToRemove))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Removed the value '{0}' from the suppression list.\r\n", valueToRemove);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value '{0}' was not in the suppression list.\r\n", valueToRemove);
            }
        }

        /// <summary>
        /// Lists all suppressions.
        /// </summary>
        /// <param name="list">Indicates whether to list the suppressions.</param>
        [Verb("Suppress", "")]
        [Sample("debug suppress --list", "Lists the items currently in the supression list.")]
        public void ListSuppress(
            [Flag("List", 'l', "Lists the names that are in the suppression list.")] bool list)
        {
            if (Options.Instance.Suppressed != null && Options.Instance.Suppressed.Count > 0)
            {
                foreach (var s in Options.Instance.Suppressed)
                {
                    if (Debug.RegisteredClasses.ContainsKey(s))
                    {
                        ConsoleBase.WriteCommandResponse(Debug.RegisteredClasses[s], "[{0}]\r\n", s);
                    }
                    else
                    {
                        ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "[{0}]\r\n", s);
                    }
                }
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "There are no names in the suppression list.\r\n");
            }
        }

        /// <summary>
        /// Adds an allowance.
        /// </summary>
        /// <param name="valueToAdd">The allowance to add.</param>
        [Verb("Allow", "Adds or removes objects from the allowed list. If any items are in this list, only these items are allowed to print debug messages.")]
        [Sample("debug allow --add \"App\"", "Adds the name \"App\" to the allowed list, which only allows items in the list to print debug messages.")]
        public void AddAllowed(
            [Operand("Add", "Adds the name that follows to the allowed list.")] string valueToAdd)
        {
            if (Debug.AddAllowed(valueToAdd))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Added the value '{0}' to the allowed list.\r\n", valueToAdd);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value '{0}' was already in the allowed list.\r\n", valueToAdd);
            }
        }

        /// <summary>
        /// Removes an allowance.
        /// </summary>
        /// <param name="valueToRemove">The allowance to remove.</param>
        [Verb("Allow", "")]
        [Sample("debug allow --remove \"App\"", "Removes the name \"App\" from the allowed list.")]
        public void RemoveAllowed(
            [Operand("Remove", "Removes the name that follows from the suppression list.")] string valueToRemove)
        {
            if (Debug.RemoveAllowed(valueToRemove))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Removed the value '{0}' from the allowed list.\r\n", valueToRemove);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value '{0}' was not in the allowed list.\r\n", valueToRemove);
            }
        }

        /// <summary>
        /// Lists all of the allowances.
        /// </summary>
        /// <param name="list">Whether to list the allowances.</param>
        [Verb("Allow", "")]
        [Sample("debug allow --list", "Lists the items currently in the allowed list.")]
        public void ListAllowed(
            [Flag("List", 'l', "Lists the names that are in the allowed list.")] bool list)
        {
            if (Options.Instance.Allowed != null && Options.Instance.Allowed.Count > 0)
            {
                foreach (var s in Options.Instance.Allowed)
                {
                    if (Debug.RegisteredClasses.ContainsKey(s))
                    {
                        ConsoleBase.WriteCommandResponse(Debug.RegisteredClasses[s], "[{0}]\r\n", s);
                    }
                    else
                    {
                        ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "[{0}]\r\n", s);
                    }
                }
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "There are no names in the allowed list.\r\n");
            }
        }
    }
}