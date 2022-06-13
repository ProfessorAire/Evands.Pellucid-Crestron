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

using System;
using System.Linq;
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
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog", "Prints the log.")]
        [Sample("logging plog -o", "Prints the log with no color.")]
        public void PrettyPrintLog(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor)
        {
            this.PrettyPrintLog(noColor, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog", "Prints the log.")]
        [Sample("logging plog -o -w --width 80", "Prints the log with no color, breaking headers with a console width of 80.")]
        public void PrettyPrintLog(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width)
        {
            this.PrettyPrintLog(wrapHeaders, noColor, width, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o --origin App10", "Prints the log with no color filtering to print only messages that originate from App10.")]
        public void PrettyPrintLogOrigin(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin)
        {
            this.PrettyPrintLog(noColor, filterOrigin, string.Empty, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o -w --width 80 --origin App10", "Prints the log with no color filtering to print only messages that originate from App10, breaking headers with a console width of 80.")]
        public void PrettyPrintLogOrigin(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin)
        {
            this.PrettyPrintLog(wrapHeaders, noColor, width, filterOrigin, string.Empty, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o --origin App10 --message \"Test Issue\"", "Prints the log with no color filtering to print only messages that originate from App10 with a message that contains 'Test Issue'.")]
        public void PrettyPrintLogOriginMessage(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin,
            [Operand("Message", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterMessage)
        {
            this.PrettyPrintLog(noColor, filterOrigin, filterMessage, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o -w --width 80 --origin App10 --message \"Test Issue\"", "Prints the log with no color filtering to print only messages that originate from App10 with a message that contains 'Test Issue', breaking headers with a console width of 80.")]
        public void PrettyPrintLogOriginMessage(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin,
            [Operand("Message", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterMessage)
        {
            this.PrettyPrintLog(wrapHeaders, noColor, width, filterOrigin, filterMessage, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o --origin App10 --level Error", "Prints the log with no color filtering to print only errors that originate from App10.")]
        public void PrettyPrintLogOriginLevel(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            this.PrettyPrintLog(noColor, filterOrigin, string.Empty, filterLevel);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o -w --width 80 --origin App10 --level Error", "Prints the log with no color filtering to print only errors that originate from App10, breaking headers with a console width of 80.")]
        public void PrettyPrintLogOriginLevel(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            this.PrettyPrintLog(wrapHeaders, noColor, width, filterOrigin, string.Empty, filterLevel);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o --message \"Pseudo Random\"", "Prints the log with no color filtering to print only messages that contain the phrase 'Psuedo Random'.")]
        public void PrettyPrintLogMessage(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Message", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterMessage)
        {
            this.PrettyPrintLog(noColor, string.Empty, filterMessage, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o -w --width 80 --message \"Pseudo Random\"", "Prints the log with no color filtering to print only messages that contain the phrase 'Psuedo Random', breaking headers with a console width of 80.")]
        public void PrettyPrintLogMessage(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width,
            [Operand("Message", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterMessage)
        {
            this.PrettyPrintLog(wrapHeaders, noColor, width, string.Empty, filterMessage, string.Empty);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o --message \"Pseudo Random\" --level Error", "Prints the log with no color filtering to print only Errors that contain the phrase 'Psuedo Random'.")]
        public void PrettyPrintLogMessageLevel(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Message", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterMessage,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            this.PrettyPrintLog(noColor, string.Empty, filterMessage, filterLevel);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o -w --width 80 --message \"Pseudo Random\" --level Error", "Prints the log with no color filtering to print only Errors that contain the phrase 'Psuedo Random', breaking headers with a console width of 80.")]
        public void PrettyPrintLogMessageLevel(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width,
            [Operand("Message", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterMessage,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            this.PrettyPrintLog(wrapHeaders, noColor, width, string.Empty, filterMessage, filterLevel);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o --level Error", "Prints the log with no color only printing Error messages.")]
        [Sample("logging plog -o --level Error,Notice", "Prints the log with no color only printing Error and Notice messages.")]
        public void PrettyPrintLogLevel(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            this.PrettyPrintLog(noColor, string.Empty, string.Empty, filterLevel);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o -w --width 80 --level Error", "Prints the log with no color only printing Error messages, breaking headers with a console width of 80.")]
        [Sample("logging plog -o --width 80 --level Error,Notice", "Prints the log with no color only printing Error and Notice messages with a console width of 80.")]
        public void PrettyPrintLogLevel(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            this.PrettyPrintLog(wrapHeaders, noColor, width, string.Empty, string.Empty, filterLevel);
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the error log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o --level Error --origin App10 --message SomethingOrOther", "Prints the log with no color only printing errors from App10 with a message that contains 'SomethingOrOther'.")]
        public void PrettyPrintLog(
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin,
            [Operand("Message", "When specified will only print messages that contain the provided text.")] string filterMessage,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "Retrieving the error log...\r\n");

            var response = string.Empty;
            CrestronConsole.SendControlSystemCommand(string.Format("err"), ref response);
            if (string.IsNullOrEmpty(response))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "Unable to retrieve the error log. No content returned.\r\n");
            }
            else
            {
                var msgs = Terminal.Formatting.Logs.ErrorLogFormatters.ParseCrestronErrorLog(response);
                if (!string.IsNullOrEmpty(filterOrigin))
                {
                    var fo = filterOrigin.ToUpper();
                    msgs = msgs.Where(m => m.Origination.ToUpper().Contains(fo));
                }

                if (!string.IsNullOrEmpty(filterMessage))
                {
                    var fm = filterMessage.ToUpper();
                    msgs = msgs.Where(m => m.Message.ToUpper().Contains(fm));
                }

                if (!string.IsNullOrEmpty(filterLevel))
                {
                    var filters = filterLevel.Split(',');
                    msgs = msgs.Where(m => filters.Any(f => m.MessageType.Equals(f, StringComparison.OrdinalIgnoreCase)));
                }

                ConsoleBase.WriteCommandResponse(Terminal.Formatting.Logs.ErrorLogFormatters.PrintPrettyErrorLog(msgs, !noColor));
            }
        }

        /// <summary>
        /// Prints the error log in a reformatted and prettily printed manner.
        /// </summary>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap headers onto two lines, with messages starting on the second line.</param>
        /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        /// <param name="width">The total width of the console in characters, for determining line breaks and wrapping.</param>
        /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        /// <param name="filterLevel">Restricts the messages to those with levels that contain the specified value.</param>
        [Verb("PrettyLog", "plog", "Retrieves the error log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        [Sample("logging plog -o -w --width 85 --level Error --origin App10 --message SomethingOrOther", "Prints the log with no color only printing errors from App10 with a message that contains 'SomethingOrOther', breaking the headers onto two lines, with a max width of 85.")]
        public void PrettyPrintLog(
            [Flag("Wrap", 'w', "When present the headers will be broken onto two lines, with messages being printed starting on the second.", true)] bool wrapHeaders,
            [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
            [Operand("Width", "Defines the total width of the console, in characters. Used for determining line breaks and text wrapping.")] int width,
            [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin,
            [Operand("Message", "When specified will only print messages that contain the provided text.")] string filterMessage,
            [Operand("Level", "When specified will only print messages with a message level that contain the provided text. Can be used to filter messages by 'Error', 'Notice', etc.")] string filterLevel)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "Retrieving the error log...\r\n");

            var response = string.Empty;
            CrestronConsole.SendControlSystemCommand(string.Format("err"), ref response);
            if (string.IsNullOrEmpty(response))
            {
                ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "Unable to retrieve the error log. No content returned.\r\n");
            }
            else
            {
                var msgs = Terminal.Formatting.Logs.ErrorLogFormatters.ParseCrestronErrorLog(response);
                if (!string.IsNullOrEmpty(filterOrigin))
                {
                    var fo = filterOrigin.ToUpper();
                    msgs = msgs.Where(m => m.Origination.ToUpper().Contains(fo));
                }

                if (!string.IsNullOrEmpty(filterMessage))
                {
                    var fm = filterMessage.ToUpper();
                    msgs = msgs.Where(m => m.Message.ToUpper().Contains(fm));
                }

                if (!string.IsNullOrEmpty(filterLevel))
                {
                    var filters = filterLevel.Split(',');
                    msgs = msgs.Where(m => filters.Any(f => m.MessageType.Equals(f, StringComparison.OrdinalIgnoreCase)));
                }

                ConsoleBase.WriteCommandResponse(Terminal.Formatting.Logs.ErrorLogFormatters.PrintPrettyErrorLog(msgs, !noColor,  width, wrapHeaders));
            }
        }

        // /// <summary>
        // /// Prints the error log in a reformatted and prettily printed manner.
        // /// </summary>
        // /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        // /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        // [Verb("PrettyLog", "plog", "Retrieves the error log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        // [Sample("logging plog -o --origin App10", "Prints the error log with no color filtering to print only messages that originate from App10.")]
        // public void PrettyPrintLogOrigin(
        //     [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
        //     [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin)
        // {
        //     this.PrettyPrintLogOriginAll(noColor, false, false, false, false, false, false, false, false, filterOrigin, string.Empty);
        // }

        // /// <summary>
        // /// Prints the error log in a reformatted and prettily printed manner.
        // /// </summary>
        // /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        // /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        // [Verb("PrettyLog", "plog", "Retrieves the error log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        // [Sample("logging plog -o --message \"Pseudo Random\"", "Prints the error log with no color filtering to print only messages that contain the phrase 'Psuedo Random'.")]
        // public void PrettyPrintLogMessage(
        //     [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
        //     [Operand("Message", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterMessage)
        // {
        //     this.PrettyPrintLogOriginAll(noColor, false, false, false, false, false, false, false, false, string.Empty, filterMessage);
        // }

        // /// <summary>
        // /// Prints the error log in a reformatted and prettily printed manner.
        // /// </summary>
        // /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        // /// <param name="sys"><see langword="true"/> to retrieve the log using the 'SYS' option.</param>
        // /// <param name="current"><see langword="true"/> to retrieve the log using the 'PLOGCURRENT' option.</param>
        // /// <param name="previous"><see langword="true"/> to retrieve the log using the 'PLOGPREVIOUS' option.</param>
        // /// <param name="all"><see langword="true"/> to retrieve the log using the 'PLOGALL' option.</param>
        // /// <param name="fatal"><see langword="true"/> to retrieve the log with only fatal values.</param>
        // /// <param name="error"><see langword="true"/> to retrieve the log with only error or worse values.</param>
        // /// <param name="warn"><see langword="true"/> to retrieve the log with warning or worse values.</param>
        // /// <param name="notice"><see langword="true"/> to retrieve the log with notice or worse values.</param>
        // [Verb("PrettyLog", "plog", "Retrieves the error log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        // [Sample("logging plog -n --origin App10", "Prints the error log with no color filtering to print only messages that originate from App10.")]
        // public void PrettyPrintLogOriginFlags(
        //     [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
        //     [Flag("Sys", 's', "When present this indicates that the log will be retrieved with the 'SYS' option.", true)] bool sys,
        //     [Flag("Current", 'c', "When present this indicates that the current persistent log will be retrieved.", true)] bool current,
        //     [Flag("Previous", 'p', "When present this indicates that the previous persistent log will be retrieved.", true)] bool previous,
        //     [Flag("All", 'a', "When present this indicates that all persistent logs will be retrieved.", true)] bool all,
        //     [Flag("Fatal", 'f', "When present this indicates that only fatal errors will be retrieved.", true)] bool fatal,
        //     [Flag("Error", 'e', "When present this indicates that only error messages and above will be retrieved.", true)] bool error,
        //     [Flag("Warn", 'w', "When present this indicates that only warning messages and above will be retrieved.", true)] bool warn,
        //     [Flag("Notice", 'n', "When present this indicates that all messages will be retrieved.", true)] bool notice)
        // {
        //     this.PrettyPrintLogOriginAll(noColor, sys, current, previous, all, fatal, error, warn, notice, string.Empty, string.Empty);
        // }

        // /// <summary>
        // /// Prints the error log in a reformatted and prettily printed manner.
        // /// </summary>
        // /// <param name="noColor"><see langword="true"/> to not colorize the printed messages.</param>
        // /// <param name="sys"><see langword="true"/> to retrieve the log using the 'SYS' option.</param>
        // /// <param name="current"><see langword="true"/> to retrieve the log using the 'PLOGCURRENT' option.</param>
        // /// <param name="previous"><see langword="true"/> to retrieve the log using the 'PLOGPREVIOUS' option.</param>
        // /// <param name="all"><see langword="true"/> to retrieve the log using the 'PLOGALL' option.</param>
        // /// <param name="fatal"><see langword="true"/> to retrieve the log with only fatal values.</param>
        // /// <param name="error"><see langword="true"/> to retrieve the log with only error or worse values.</param>
        // /// <param name="warn"><see langword="true"/> to retrieve the log with warning or worse values.</param>
        // /// <param name="notice"><see langword="true"/> to retrieve the log with notice or worse values.</param>
        // /// <param name="filterOrigin">Restricts the messages to those with origination points that contain the specified value.</param>
        // /// <param name="filterMessage">Restricts the messages to those with message contents that contain the specified value.</param>
        // [Verb("PrettyLog", "plog", "Retrieves the error log and prints it to the console with pretty formatting. Only supported on physical control system hardware.")]
        // [Sample("logging plog -n --origin App10", "Prints the error log with no color filtering to print only messages that originate from App10.")]
        // public void PrettyPrintLogOriginAll(
        //     [Flag("NoColor", 'o', "When present this indicates that the log should be printed with no color.", true)] bool noColor,
        //     [Flag("Sys", 's', "When present this indicates that the log will be retrieved with the 'SYS' option.", true)] bool sys,
        //     [Flag("Current", 'c', "When present this indicates that the current persistent log will be retrieved.", true)] bool current,
        //     [Flag("Previous", 'p', "When present this indicates that the previous persistent log will be retrieved.", true)] bool previous,
        //     [Flag("All", 'a', "When present this indicates that all persistent logs will be retrieved.", true)] bool all,
        //     [Flag("Fatal", 'f', "When present this indicates that only fatal errors will be retrieved.", true)] bool fatal,
        //     [Flag("Error", 'e', "When present this indicates that only error messages and above will be retrieved.", true)] bool error,
        //     [Flag("Warn", 'w', "When present this indicates that only warning messages and above will be retrieved.", true)] bool warn,
        //     [Flag("Notice", 'n', "When present this indicates that all messages will be retrieved.", true)] bool notice,
        //     [Operand("Origin", "When specified will only print messages with origination points that contain the provided text. Can be used to filter messages by a specific application.")] string filterOrigin,
        //     [Operand("Message", "When specified will only print messages that contain the provided text.")] string filterMessage)
        // {
        //     ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Progress, "Retrieving the error log.\r\n");
        //     if (Options.Instance.EnableMarkup)
        //     {
        //         ConsoleBase.WriteCommandResponse("[:hc]");
        //     }

        //     try
        //     {
        //         var response = string.Empty;
        //         var options = sys ? " sys" : current ? " plogcurrent" : previous ? " plogprevious" : all ? " plogall" : fatal ? " fatal" : error ? " error" : warn ? " warning" : notice ? " notice" : string.Empty;
        //         CrestronConsole.SendControlSystemCommand(string.Format("err{0}", options), ref response);
        //         if (string.IsNullOrEmpty(response))
        //         {
        //             ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.Error, "Unable to retrieve the error log. No content returned.");
        //         }
        //         else
        //         {
        //             var msgs = Terminal.Formatting.Logs.ErrorLogFormatters.ParseCrestronErrorLog(response);
        //             if (!string.IsNullOrEmpty(filterOrigin))
        //             {
        //                 msgs = msgs.Where(m => m.Origination.Contains(filterOrigin));
        //             }

        //             if (!string.IsNullOrEmpty(filterMessage))
        //             {
        //                 msgs = msgs.Where(m => m.Message.Contains(filterMessage));
        //             }

        //             ConsoleBase.WriteCommandResponse(Terminal.Formatting.Logs.ErrorLogFormatters.PrintPrettyErrorLog(msgs, !noColor));
        //         }
        //     }
        //     finally
        //     {
        //         if (Options.Instance.EnableMarkup)
        //         {
        //             ConsoleBase.WriteCommandResponse("[:/hc]");
        //         }
        //     }
        // }

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