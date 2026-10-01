// <copyright file="CrestronErrorLogCommands.cs">
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
using System.Linq;
using Crestron.SimplSharp;
using Evands.Pellucid.Terminal.Commands;
using Evands.Pellucid.Terminal.Commands.Attributes;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Terminal commands for Crestron error log display.
    /// </summary>
    [Command("Logging", 3, "Configures logging parameters.")]
    public class CrestronErrorLogCommands : TerminalCommandBase
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
            CrestronConsole.SendControlSystemCommand("err", ref response);
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
            CrestronConsole.SendControlSystemCommand("err", ref response);
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

                ConsoleBase.WriteCommandResponse(Terminal.Formatting.Logs.ErrorLogFormatters.PrintPrettyErrorLog(msgs, !noColor, width, wrapHeaders));
            }
        }
    }
}
