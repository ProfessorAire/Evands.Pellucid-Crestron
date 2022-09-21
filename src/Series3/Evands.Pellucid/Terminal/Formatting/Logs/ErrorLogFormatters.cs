#region copyright
// <copyright file="ErrorLogFormatters.cs" company="Christopher McNeely">
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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Crestron.SimplSharp;

namespace Evands.Pellucid.Terminal.Formatting.Logs
{
    /// <summary>
    /// Provides formatting methods for Crestron ErrorLogs.
    /// </summary>
    public static class ErrorLogFormatters
    {
        /// <summary>
        /// Takes the contents of the result from an ErrorLog command and parses it out to a
        /// list of <see cref="LogMessage"/> objects.
        /// </summary>
        /// <param name="logContents">The contents of the error log.</param>
        /// <returns>An enumerable of <see cref="LogMessage"/> instances.</returns>
        public static IEnumerable<LogMessage> ParseCrestronErrorLog(string logContents)
        {
            if (CrestronEnvironment.DevicePlatform == eDevicePlatform.Appliance)
            {
                if (CrestronEnvironment.ProgramCompatibility == eCrestronSeries.Series3)
                {
                    return ParseThreeSeriesErrorLog(logContents);
                }
                
                if ((CrestronEnvironment.ProgramCompatibility & eCrestronSeries.Series4) == eCrestronSeries.Series4)
                {
                    return ParseFourSeriesErrorLog(logContents);
                }
            }

            return new LogMessage[0];
        }

        /// <summary>
        /// Prints an enumeration of <see cref="LogMessage"/> instances to a string,
        /// formatting them in a way that is easier to review.
        /// </summary>
        /// <param name="messages">The <see cref="LogMessage"/>s to print.</param>
        /// <param name="colorize"><see langword="true"/> to colorize the messages, <see langword="false"/> otherwise.</param>
        /// <returns>A <see langword="string"/> with the error log contents prettily formatted.</returns>
        public static string PrintPrettyErrorLog(IEnumerable<LogMessage> messages, bool colorize)
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            if (messages.Any())
            {
                var sb = new StringBuilder();
                var numberMax = messages.Max(m => m.Number.ToString().Length);
                var typeMax = messages.Max(m => m.MessageType.Length);
                var origMax = messages.Max(m => m.Origination.Length);

                foreach (var msg in messages)
                {
                    sb.Append(msg.ToString(colorize, numberMax, typeMax, origMax));
                    sb.Append("\r\n");
                }

                return sb.ToString();
            }

            return colorize ? ConsoleBase.Colors.Warning.FormatText("No Messages to Display")
                : "No Messages to Display";
        }

        /// <summary>
        /// Prints an enumeration of <see cref="LogMessage"/> instances to a string,
        /// formatting them in a way that is easier to review.
        /// </summary>
        /// <param name="messages">The <see cref="LogMessage"/>s to print.</param>
        /// <param name="colorize"><see langword="true"/> to colorize the messages, <see langword="false"/> otherwise.</param>
        /// <param name="totalWidth">The width of the console, in characters.</param>
        /// <param name="wrapHeaders"><see langword="true"/> to break the message headers, with the message starting on the second line.</param>
        /// <returns>A <see langword="string"/> with the error log contents prettily formatted.</returns>
        public static string PrintPrettyErrorLog(IEnumerable<LogMessage> messages, bool colorize, int totalWidth, bool wrapHeaders)
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            if (messages.Any())
            {
                var sb = new StringBuilder();
                var numberMax = messages.Max(m => m.Number.ToString().Length);
                var typeMax = messages.Max(m => m.MessageType.Length);
                var origMax = messages.Max(m => m.Origination.Length);

                foreach (var msg in messages)
                {
                    sb.Append(msg.ToString(colorize, numberMax, typeMax, origMax, totalWidth, wrapHeaders));
                    sb.Append("\r\n");
                }

                return sb.ToString();
            }

            return colorize ? ConsoleBase.Colors.Warning.FormatText("No Messages to Display")
                : "No Messages to Display";
        }

        /// <summary>
        /// Parses an error log, treating the contents as a 3-Series error log.
        /// </summary>
        /// <param name="logContents">The log contents from a 3-Series processor.</param>
        /// <returns>An enumerable of <see cref="LogMessage"/> instances.</returns>
        private static IEnumerable<LogMessage> ParseThreeSeriesErrorLog(string logContents)
        {
            var spaceRegex = new Regex(@"(?>\r\n|\r|\n)(?>  )+", RegexOptions.Compiled);
            var startingAtRegex = new Regex(@"(?>\r\n|\r|\n) *at ", RegexOptions.Compiled);
            var brokenRegex = new Regex(@"(?>\d+\. )?\w*:? +.*? +# +\d{4}-\d\d-\d\d \d\d:\d\d:\d\d +# +(?<msg>(?>(?!(?>\r|\n|\r\n)? *\d+\. +)(?>\w|\W|))*(?>\r\n|\r|\n)?)", RegexOptions.Compiled | RegexOptions.Singleline);

            var assembly = spaceRegex.Replace(logContents, "\r\n");
            assembly = startingAtRegex.Replace(assembly, "\r\n  at ");

            var messages = new List<LogMessage>();

            var matches = brokenRegex.Matches(assembly);

            var sb = new StringBuilder();
            var removeNextHeader = false;
            for (var i = 0; i < matches.Count; i++)
            {
                var remove = removeNextHeader;
                removeNextHeader = !matches[i].Groups["msg"].Value.EndsWith("\r\n");

                if (remove)
                {
                    sb.Append(matches[i].Groups["msg"].Value.Replace(Environment.NewLine, "\r\n"));
                    if (!removeNextHeader)
                    {
                        sb.Append("\r\n");
                    }
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        LogMessage msg;
                        if (LogMessage.TryParse(sb.ToString(), messages.Count + 1, out msg))
                        {
                            messages.Add(msg);
                        }

                        sb.Length = 0;
                    }

                    sb.Append(matches[i].Value.Replace(Environment.NewLine, "\r\n"));
                    if (!removeNextHeader)
                    {
                        sb.Append("\r\n");
                    }
                }
            }

            if (sb.Length > 0)
            {
                LogMessage msg;
                if (LogMessage.TryParse(sb.ToString(), messages.Count + 1, out msg))
                {
                    messages.Add(msg);
                }
            }

            return messages.AsReadOnly();
        }

        /// <summary>
        /// Parses an error log, treating the contents as a 4-Series error log.
        /// </summary>
        /// <param name="logContents">The log contents from a 4-Series processor.</param>
        /// <returns>An enumerable of <see cref="LogMessage"/> instances.</returns>
        private static IEnumerable<LogMessage> ParseFourSeriesErrorLog(string logContents)
        {
            var spaceRegex = new Regex(@"(?>\r\n|\r|\n)(?> +)+", RegexOptions.Compiled);
            var startingAtRegex = new Regex(@"(?>\r\n|\r|\n) *at ", RegexOptions.Compiled);
            var brokenRegex = new Regex(@"(?>\d+\. )?\w*:? +.*? +# +\d{4}-\d\d-\d\d \d\d:\d\d:\d\d +# +(?<msg>(?>(?!(?>\r|\n|\r\n)? *\d+\. +)(?>\w|\W|))*(?>\r\n|\r|\n)?)", RegexOptions.Compiled | RegexOptions.Singleline);

            var assembly = spaceRegex.Replace(logContents, "\r\n");
            assembly = startingAtRegex.Replace(assembly, "\r\n  at ");

            var messages = new List<LogMessage>();

            var matches = brokenRegex.Matches(assembly);

            var sb = new StringBuilder();
            var removeNextHeader = false;
            for (var i = 0; i < matches.Count; i++)
            {
                var remove = removeNextHeader;
                removeNextHeader = !matches[i].Groups["msg"].Value.EndsWith("\r\n") || matches[i].Groups["msg"].Value.Length >= 256;

                if (remove)
                {
                    sb.Append(matches[i].Groups["msg"].Value.Replace(Environment.NewLine, "\r\n").TrimEnd('\n', '\r'));
                    if (!removeNextHeader)
                    {
                        sb.Append("\r\n");
                    }
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        LogMessage msg;
                        if (LogMessage.TryParse(sb.ToString(), messages.Count + 1, out msg))
                        {
                            messages.Add(msg);
                        }

                        sb.Length = 0;
                    }

                    sb.Append(removeNextHeader ? matches[i].Value.Replace(Environment.NewLine, "\r\n").TrimEnd('\n', '\r')
                        : matches[i].Value.Replace(Environment.NewLine, "\r\n"));

                    if (!removeNextHeader)
                    {
                        sb.Append("\r\n");
                    }
                }
            }

            if (sb.Length > 0)
            {
                LogMessage msg;
                if (LogMessage.TryParse(sb.ToString(), messages.Count + 1, out msg))
                {
                    messages.Add(msg);
                }
            }

            return messages.AsReadOnly();
        }
    }
}