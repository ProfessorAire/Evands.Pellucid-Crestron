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

using Elegant.Terminal.Commands;
using Elegant.Terminal.Commands.Attributes;

namespace Elegant.Diagnostics
{
    /// <summary>
    /// Console commands for configuring debugging.
    /// </summary>
    [Command("Debug", "Provides methods for configuring program debugging.")]
    public class DebuggingCommands : TerminalCommandBase
    {
        /// <summary>
        /// Enables debugging.
        /// </summary>
        /// <param name="enable">Flag indicating enable.</param>
        [Verb("Config", "Configures debugging.")]
        [Sample("config --enable", "Enables debug messaging.")]
        public void Enable(
            [Flag("Enable", "Enables debugging.")] bool enable)
        {
            Debug.IsEnabled = true;
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "Debug messaging is enabled.");
        }

        /// <summary>
        /// Disables debugging.
        /// </summary>
        /// <param name="disable">Flag indicating disable.</param>
        [Verb("Config", "Configures debugging.")]
        [Sample("config --disable", "Disables debug messaging.")]
        public void Disable(
            [Flag("Disable", "Disables debugging.")] bool disable)
        {
            Debug.IsEnabled = false;
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "Debug messaging is disabled.");
        }

        /// <summary>
        /// Configures debugging.
        /// </summary>
        /// <param name="enable">Whether to enable or disable debugging.</param>
        /// <param name="timestamps">Whether to enable timestamps.</param>
        /// <param name="shortTime">Whether to enable 24 hour timestamps.</param>
        [Verb("", "Configures debugging.")]
        [Sample("--enable true --useTimestamps --use24HourTime", "Enables debugging, timestamps, and the use of 24 hour timestamps.")]
        [Sample("--enable true -tu", "Enables debugging, timestamps, and the use of 24 hour timestamps.")]
        public void Config(
            [Operand("Enable", "Enables or disables debugging, depending on how it is set. Valid values 'true' and 'false'.")] bool enable,
            [Flag("UseTimestamps", 't', "Include to enable the addition of timestamps to debugging messages.", true)] bool timestamps,
            [Flag("Use24HourTime", 'u', "Include to enable the printing of timestamps in 24 Hour Time.", true)] bool shortTime)
        {
            Debug.IsEnabled = enable;
            Debug.UseTimestamps = timestamps;
            Debug.Use24HourTime = shortTime;
        }

        /// <summary>
        /// Configures debugging.
        /// </summary>
        /// <param name="timestamps">Whether to enable timestamps.</param>
        /// <param name="shortTime">Whether to enable 24 hour timestamps.</param>
        [Verb("", "Configures debugging.")]
        [Sample("--useTimestamps --use24HourTime", "Enables timestamps, and the use of 24 hour timestamps.")]
        [Sample("-tu", "Enables timestamps, and the use of 24 hour timestamps.")]
        [Sample("", "Disables timestamps, and the use of 24 hour timestamps.")]
        public void Config(
            [Flag("UseTimestamps", 't', "Include to enable the addition of timestamps to debugging messages.", true)] bool timestamps,
            [Flag("Use24HourTime", 'u', "Include to enable the printing of timestamps in 24 Hour Time.", true)] bool shortTime)
        {
            Debug.UseTimestamps = timestamps;
            Debug.Use24HourTime = shortTime;
        }        

        /// <summary>
        /// Gets the current debugging state.
        /// </summary>
        [Verb("", "Configures debugging.")]
        [Sample("", "Lists the current debug messaging status.")]
        public void CurrentStatus()
        {
            ConsoleBase.WriteCommandResponse(
                ConsoleBase.Colors.Progress,
                "Debugging is {0}.\nTimestamps are {1}.\n24 Hour Timestamps are {2}.",
                Debug.IsEnabled ? "enabled" : "disabled",
                Debug.UseTimestamps ? "enabled" : "disabled",
                Debug.Use24HourTime ? "enabled" : "disabled");
       }

        /// <summary>
        /// Adds a suppression.
        /// </summary>
        /// <param name="valueToAdd">The suppression to add.</param>
        [Verb("Suppress", "Adds or removes objects from the suppression list. Item in this list aren't allowed to print debug messages to the console.")]
        [Sample("suppress --add \"App\"", "Adds the name \"App\" to the suppression list, which will prevent any class registered with that name from printing debugging messages.")]
        public void AddSuppress(
            [Operand("Add", "Adds the name that follows to the suppression list.")] string valueToAdd)
        {
            if (Debug.AddSuppression(valueToAdd))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Added the value {0} to the suppression list.", valueToAdd);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value {0} was already in the suppression list.", valueToAdd);
            }
        }

        /// <summary>
        /// Removes a suppression.
        /// </summary>
        /// <param name="valueToRemove">The suppression to remove.</param>
        [Verb("Suppress", "Adds or removes objects from the suppression list. Item in this list aren't allowed to print debug messages to the console.")]
        [Sample("suppress --remove \"App\"", "Removes the name \"App\" from the suppression list.")]
        public void RemoveSuppress(
            [Operand("Remove", "Removes the name that follows from the suppression list.")] string valueToRemove)
        {
            if (Debug.RemoveSuppression(valueToRemove))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Removed the value {0} from the suppression list.", valueToRemove);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value {0} was not in the suppression list.", valueToRemove);
            }
        }

        /// <summary>
        /// Lists all suppressions.
        /// </summary>
        /// <param name="list">Indicates whether to list the suppressions.</param>
        [Verb("Suppress", "")]
        [Sample("suppress --list", "Lists the items currently in the supression list.")]
        public void ListSuppress(
            [Flag("List", "Lists the names that are in the suppression list.")] bool list)
        {
            if (Debug.Suppressed.Count > 0)
            {
                foreach (var s in Debug.Suppressed)
                {
                    if (Debug.RegisteredClasses.ContainsKey(s))
                    {
                        ConsoleBase.WriteCommandResponse(Debug.RegisteredClasses[s], "[{0}]", s);
                    }
                    else
                    {
                        ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "[{0}]", s);
                    }
                }
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "There are no names in the suppression list.");
            }
        }

        /// <summary>
        /// Adds an allowance.
        /// </summary>
        /// <param name="valueToAdd">The allowance to add.</param>
        [Verb("Allow", "Adds or removes objects from the allowed list. If any items are in this list, only these items are allowed to print debug messages.")]
        [Sample("allow --add \"App\"", "Adds the name \"App\" to the allowed list, which only allows items in the list to print debug messages.")]
        public void AddAllowed(
            [Operand("Add", "Adds the name that follows to the allowed list.")] string valueToAdd)
        {
            if (Debug.AddAllowed(valueToAdd))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Added the value {0} to the allowed list.", valueToAdd);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value {0} was already in the allowed list.", valueToAdd);
            }
        }

        /// <summary>
        /// Removes an allowance.
        /// </summary>
        /// <param name="valueToRemove">The allowance to remove.</param>
        [Verb("Allow", "")]
        [Sample("allow --remove \"App\"", "Removes the name \"App\" from the allowed list.")]
        public void RemoveAllowed(
            [Operand("Remove", "Removes the name that follows from the suppression list.")] string valueToRemove)
        {
            if (Debug.RemoveAllowed(valueToRemove))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Success, "Removed the value {0} from the allowed list.", valueToRemove);
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "The value {0} was not in the allowed list.", valueToRemove);
            }
        }

        /// <summary>
        /// Lists all of the allowances.
        /// </summary>
        /// <param name="list">Whether to list the allowances.</param>
        [Verb("Allow", "")]
        [Sample("allow --list", "Lists the items currently in the allowed list.")]
        public void ListAllowed(
            [Flag("List", "Lists the names that are in the allowed list.")] bool list)
        {
            if (Debug.Allowed.Count > 0)
            {
                foreach (var s in Debug.Allowed)
                {
                    if (Debug.RegisteredClasses.ContainsKey(s))
                    {
                        ConsoleBase.WriteCommandResponse(Debug.RegisteredClasses[s], "[{0}]", s);
                    }
                    else
                    {
                        ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "[{0}]", s);
                    }
                }
            }
            else
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "There are no names in the allowed list.");
            }
        }
    }
}