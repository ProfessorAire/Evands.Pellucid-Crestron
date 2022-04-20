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
using System.Text.RegularExpressions;

namespace Evands.Pellucid.Terminal.Formatting.Logs
{
    /// <summary>
    /// Contains information regarding error messages.
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// Parses out the message contents from an error string.
        /// </summary>
        private static readonly Regex MsgRegex = new Regex(@"^(?> *(?<num>\d+)\. )?(?<type>[^:]+): (?<orig>.*) # (?<stamp>\d[^#]+) # (?<msg>.*)$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);

        /// <summary>
        /// Parses out the number of messages logged from a full error log.
        /// Used to remove the number of messages when needed.
        /// </summary>
        private static readonly Regex CountRegex = new Regex(@"Total [^ ]+ Logged += +\d+");

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
        /// Tries to parse a complete <see cref="ErrorMessage"/> from an unformatted error message.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="index">The index of the message.</param>
        /// <param name="result">An <see cref="ErrorMessage"/> instance.</param>
        /// <returns><see langword="true"/> if the message was parsed, <see langword="false"/> otherwise.</returns>
        public static bool TryParse(string message, int index, out ErrorMessage result)
        {
            result = new ErrorMessage();

            try
            {
                var match = MsgRegex.Match(message);
                if (match.Groups["num"].Success)
                {
                    result.Number = int.Parse(match.Groups["num"].Value);
                }
                else
                {
                    result.Number = index;
                }

                result.TimeStamp = DateTime.Parse(match.Groups["stamp"].Value);
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
        /// <returns>A <see langword="string"/> with the formatted message.</returns>
        public string ToString(bool colorize, int numberPad, int typePad, int originationPad)
        {
            if (colorize)
            {
                var typeColor = GetTypeColorFormat();
                return string.Format(
                    ConsoleBase.Colors.LogHeaders.FormatText(false, "{0}. {1} | {2} | {3}: {4}"),
                    this.Number.ToString().PadLeft(numberPad),
                    this.TimeStamp.ToString("yy/mm/dd HH:mm:ss"),
                    this.Origination.PadRight(originationPad),
                    typeColor.FormatText(false, this.MessageType.PadLeft(typePad)),
                    ColorFormat.CloseTextFormat(this.Message));
            }

            return string.Format(
                    "{0}. {1} | {2} | {3}: {4}",
                    this.Number.ToString().PadLeft(numberPad),
                    this.TimeStamp.ToString("yy/mm/dd HH:mm:ss"),
                    this.Origination.PadLeft(originationPad),
                    this.MessageType.PadLeft(typePad),
                    this.Message);
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
            else if (this.MessageType.Equals("error", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Error;
            }
            else if (this.MessageType.Equals("notice", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Notice;
            }
            else if (this.MessageType.Equals("info", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Notice;
            }
            else if (this.MessageType.Equals("ok", StringComparison.OrdinalIgnoreCase))
            {
                return ConsoleBase.Colors.Notice;
            }

            return ConsoleBase.Colors.Subtle;
        }
    }
}