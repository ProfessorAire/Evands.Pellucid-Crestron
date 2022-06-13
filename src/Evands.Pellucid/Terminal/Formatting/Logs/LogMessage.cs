#region copyright
// <copyright file="ErrorMessage.cs" company="Christopher McNeely">
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
using System.Text;
using System.Text.RegularExpressions;

namespace Evands.Pellucid.Terminal.Formatting.Logs
{
    /// <summary>
    /// Contains information regarding error messages.
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Parses out the message contents from an error string.
        /// </summary>
        private static readonly Regex MsgRegex = new Regex(@"^(?> *(?<num>\d+)\. )?(?<type>[^:]+): (?<orig>.*) # (?<stamp>\d[^#]+) +# (?<msg>.*)$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);

        /// <summary>
        /// Parses out the number of messages logged from a full error log.
        /// Used to remove the number of messages when needed.
        /// </summary>
        private static readonly Regex CountRegex = new Regex(@"Total [^ ]+ Logged += +\d+");

        /// <summary>
        /// Backing field for the <see cref="TimestampFormat"/> property.
        /// </summary>
        private string timestampFormat;

        /// <summary>
        /// Prevents a default instance of the <see cref="LogMessage"/> class from being created.
        /// </summary>
        private LogMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="number">The message number.</param>
        /// <param name="messageType">The message type as a <see langword="string"/>.</param>
        /// <param name="origination">The origination point of the message.</param>
        /// <param name="timestamp">The date/time stamp of the message.</param>
        /// <param name="message">The message body.</param>
        internal LogMessage(
                    int number,
                    string messageType,
                    string origination,
                    DateTime timestamp,
                    string message)
        {
            this.Number = number;
            this.MessageType = messageType;
            this.Origination = origination;
            this.TimeStamp = timestamp;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the number of the error.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the type of error the message represents.
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the system-level origination point for the message.
        /// </summary>
        public string Origination { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the message.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the message contents.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value representing the default Date/Time format string to use when writing log
        /// messages to strings, using the <see cref="Terminal.Formatting.Logs.LogMessage"/> class.
        /// <para>
        /// If not specified will return the value specified by the <see cref="Options.DefaultLogTimestampFormat"/> property.
        /// </para>
        /// </summary>
        public string TimestampFormat
        {
            get
            {
                return !string.IsNullOrEmpty(this.timestampFormat) ? this.timestampFormat : Options.Instance.DefaultLogTimestampFormat;
            }

            set
            {
                this.timestampFormat = value;
            }
        }

        /// <summary>
        /// Tries to parse a complete <see cref="LogMessage"/> from an unformatted error message.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="index">The index of the message.</param>
        /// <param name="result">A <see cref="LogMessage"/> instance.</param>
        /// <returns><see langword="true"/> if the message was parsed, <see langword="false"/> otherwise.</returns>
        public static bool TryParse(string message, int index, out LogMessage result)
        {
            result = new LogMessage();

            try
            {
                var match = MsgRegex.Match(message);
                result.Number = match.Groups["num"].Success ? int.Parse(match.Groups["num"].Value) : index;

                result.TimeStamp = DateTime.Parse(match.Groups["stamp"].Value, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal);
                result.MessageType = match.Groups["type"].Value;
                result.Origination = match.Groups["orig"].Value;
                var countMatch = CountRegex.Match(match.Groups["msg"].Value);
                result.Message = countMatch.Success ? match.Groups["msg"].Value.Substring(0, countMatch.Index).TrimEnd('\r', '\n') : match.Groups["msg"].Value.TrimEnd('\r', '\n');

                if (Environment.NewLine != "\r\n")
                {
                    result.Message = result.Message.Replace(Environment.NewLine, "\r\n");
                }

                if (result.Message.Contains('\r') || result.Message.Contains('\n'))
                {
                    result.Message += "\r\n";
                }

                result.Message = result.Message.Trim();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Prints the message as a string, without colorization or additional padding.
        /// </summary>
        /// <returns>A <see langword="string"/> with the formatted message.</returns>
        public override string ToString()
        {
            return this.ToString(false, 0, 0, 0);
        }

        /// <summary>
        /// Prints the message as a string, optionally colorizing it, with the specified padding.
        /// </summary>
        /// <param name="colorize">A value indicating whether to print the message with color.</param>
        /// <param name="numberPad">The number of characters to pad the number component of the message. Used to align messages.</param>
        /// <param name="typePad">The number of characters to pad the type component of the message. Used to align messages.</param>
        /// <param name="originationPad">The number of characters to pad the origination component of the message. Used to align messages.</param>
        /// <param name="totalWidth">The total width of the line the log message should be printed on.</param>
        /// <param name="wrapHeaders"><see langword="true"/> to wrap the headers, leaving more room for the message content.</param>
        /// <returns>A <see langword="string"/> with the formatted message.</returns>
        public string ToString(bool colorize, int numberPad, int typePad, int originationPad, int totalWidth, bool wrapHeaders)
        {
            var sb = new StringBuilder();
            numberPad = Math.Max(numberPad, this.Number.ToString().Length);
            typePad = Math.Max(typePad, this.MessageType.Length);
            originationPad = Math.Max(originationPad, this.Origination.Length);

            var isColored = Options.Instance.ColorizeConsoleOutput;
            if (!colorize)
            {
                Options.Instance.ColorizeConsoleOutput = false;
            }

            try
            {
                var typeColor = GetTypeColorFormat();
                if (!wrapHeaders)
                {
                    sb.Append(
                        ConsoleBase.Colors.LogHeaders.FormatText(false, "{0}. {1} | {2} | {3}: ",
                            this.Number.ToString().PadLeft(numberPad),
                            this.TimeStamp.ToString(this.TimestampFormat),
                            this.Origination.PadRight(originationPad),
                            typeColor.FormatText(false, this.MessageType.PadLeft(typePad))));

                    var indentAmount = numberPad +
                        this.TimeStamp.ToString(this.TimestampFormat).Length +
                        originationPad +
                        typePad + 10;

                    var msgWidth = totalWidth - indentAmount;
                    if (msgWidth < 20)
                    {
                        sb.Append(this.Message);
                    }
                    else
                    {
                        var lines = this.Message.Replace(ConsoleBase.NewLine, "\n").Split('\n');
                        var first = true;
                        for (var i = 0; i < lines.Length; i++)
                        {
                            var line = lines[i];
                            var widthLeftToWrite = line.Length;

                            while (widthLeftToWrite > 0)
                            {
                                var start = line.Length - widthLeftToWrite;
                                var count = Math.Min(widthLeftToWrite, msgWidth);
                                sb.Append(start > 0 ? line.Substring(start, count).Trim() : line.Substring(start, count).TrimEnd());
                                widthLeftToWrite -= count;

                                if (first)
                                {
                                    first = false;
                                    msgWidth += typePad + 2;
                                    indentAmount -= typePad + 2;
                                }

                                if (widthLeftToWrite > 0)
                                {
                                    sb.Append(ConsoleBase.NewLine);
                                    sb.Append(' ', indentAmount);
                                }
                            }

                            if (i < lines.Length - 1)
                            {
                                sb.Append(ConsoleBase.NewLine);
                                if (!string.IsNullOrEmpty(lines[i + 1]))
                                {
                                    sb.Append(' ', indentAmount);
                                }
                            }
                        }
                    }

                    sb.Append(ColorFormat.CloseTextFormat(string.Empty));
                }
                else
                {
                    var msgWidth = totalWidth - typePad - 2 - numberPad - 2;
                    Console.WriteLine(msgWidth);
                    sb.Append(
                        ConsoleBase.Colors.LogHeaders.FormatText(false, "{0}. {1} | {2}",
                            this.Number.ToString().PadLeft(numberPad),
                            this.TimeStamp.ToString(this.TimestampFormat),
                            this.Origination));

                    sb.Append(ConsoleBase.NewLine);
                    sb.Append(' ', numberPad + 2);
                    sb.Append(typeColor.FormatText(false, "{0}: ", this.MessageType.PadLeft(typePad)));

                    if (msgWidth <= 20)
                    {
                        sb.Append(this.Message);
                    }
                    else
                    {
                        var lines = this.Message.Replace(ConsoleBase.NewLine, "\n").Split('\n');
                        var indentAmount = numberPad + 4 + typePad;

                        for (var i = 0; i < lines.Length; i++)
                        {
                            var line = lines[i];
                            var widthLeftToWrite = line.Length;

                            while (widthLeftToWrite > 0)
                            {
                                var start = line.Length - widthLeftToWrite;
                                var count = Math.Min(widthLeftToWrite, msgWidth);
                                sb.Append(start > 0 ? line.Substring(start, count).Trim() : line.Substring(start, count).TrimEnd());
                                widthLeftToWrite -= count;

                                if (widthLeftToWrite > 0)
                                {
                                    sb.Append(ConsoleBase.NewLine);
                                    sb.Append(' ', indentAmount);
                                }
                            }

                            if (i < lines.Length - 1)
                            {
                                sb.Append(ConsoleBase.NewLine);
                                if (!string.IsNullOrEmpty(lines[i + 1]))
                                {
                                    sb.Append(' ', indentAmount);
                                }
                            }
                        }
                    }

                    sb.Append(ColorFormat.CloseTextFormat(string.Empty));
                }

                return sb.ToString();
            }
            finally
            {
                Options.Instance.ColorizeConsoleOutput = isColored;
            }

        }

        /// <summary>
        /// Prints the message as a string, optionally colorizing it, with the specified padding.
        /// </summary>
        /// <param name="colorize">A value indicating whether to print the message with color.</param>
        /// <param name="numberPad">The number of characters to pad the number component of the message. Used to align messages.</param>
        /// <param name="typePad">The number of characters to pad the type component of the message. Used to align messages.</param>
        /// <param name="originationPad">The number of characters to pad the origination component of the message. Used to align messages.</param>
        /// <returns>A <see langword="string"/> with the formatted message.</returns>
        public string ToString(bool colorize, int numberPad, int typePad, int originationPad)
        {
            if (colorize)
            {
                var typeColor = GetTypeColorFormat();
                return string.Format(
                    ConsoleBase.Colors.LogHeaders.FormatText(false, "{0}. {1} | {2} | {3}: {4}"),
                    this.Number.ToString().PadLeft(numberPad),
                    this.TimeStamp.ToString(this.TimestampFormat),
                    this.Origination.PadRight(originationPad),
                    typeColor.FormatText(false, this.MessageType.PadLeft(typePad)),
                    ColorFormat.CloseTextFormat(this.Message));
            }

            return string.Format(
                    "{0}. {1} | {2} | {3}: {4}",
                    this.Number.ToString().PadLeft(numberPad),
                    this.TimeStamp.ToString(this.TimestampFormat),
                    this.Origination.PadRight(originationPad),
                    this.MessageType.PadLeft(typePad),
                    this.Message);
        }

        /// <summary>
        /// Compares this instance with another <see cref="LogMessage"/> instance.
        /// </summary>
        /// <param name="obj">An instance of <see cref="LogMessage"/> to compare this one to.</param>
        /// <returns><see langword="true"/> if the two instances are equal, <see langword="false"/> otherwise.</returns>
        public bool Equals(LogMessage obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.Number == obj.Number &&
                this.TimeStamp == obj.TimeStamp &&
                this.MessageType.Equals(obj.MessageType) &&
                this.Origination == obj.Origination &&
                this.Message.Equals(obj.Message);
        }

        /// <summary>
        /// Gets the color format for the specified types.
        /// </summary>
        /// <returns>An instance of <see cref="ColorFormat"/>.</returns>
        private ColorFormat GetTypeColorFormat()
        {
            if (this.MessageType.Equals("warning", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Warning;
            }

            if (this.MessageType.Equals("error", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Error;
            }

            if (this.MessageType.Equals("notice", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Notice;
            }

            if (this.MessageType.Equals("info", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Notice;
            }

            if (this.MessageType.Equals("ok", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Notice;
            }

            return ConsoleBase.Colors.Subtle;
        }
    }
}