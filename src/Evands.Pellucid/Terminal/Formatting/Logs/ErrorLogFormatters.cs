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

namespace Evands.Pellucid.Terminal.Formatting.Logs
{
    /// <summary>
    /// Provides formatting methods for Crestron ErrorLogs.
    /// </summary>
    public static class ErrorLogFormatters
    {
        /// <summary>
        /// Takes the contents of the result from an ErrorLog command and parses it out to a
        /// list of <see cref="ErrorMessage"/> objects.
        /// </summary>
        /// <param name="logContents">The contents of the error log.</param>
        /// <returns>An enumerable of <see cref="ErrorMessage"/> instances.</returns>
        public static IEnumerable<ErrorMessage> PrettifyCrestronErrorLog(string logContents)
        {
            var spaceRegex = new Regex(@"(?>(?>\r|\n)|\r\n)(?>  )+", RegexOptions.Compiled);
            var startingAtRegex = new Regex(@"(?>\r|\n|\r\n) *at ", RegexOptions.Compiled);
            var brokenRegex = new Regex(@"(?>\d+\. )?\w*: +.*? +# +\d{4}-\d\d-\d\d \d\d:\d\d:\d\d +# +(?<msg>(?>(?!(?>\r|\n|\r\n) *\d+\. +)(?>.)){0,256})", RegexOptions.Compiled | RegexOptions.Singleline);

            var assembly = spaceRegex.Replace(logContents, "\r\n");
            assembly = startingAtRegex.Replace(assembly, "\r\n   at ");

            var messages = new List<ErrorMessage>();

            var matches = brokenRegex.Matches(assembly);

            var sb = new StringBuilder();
            var removeNextHeader = false;
            for (var i = 0; i < matches.Count; i++)
            {
                var remove = removeNextHeader;
                removeNextHeader = matches[i].Groups["msg"].Value.Length == 256;

                if (remove)
                {
                    sb.Append(matches[i].Groups["msg"].Value);
                    if (!removeNextHeader)
                    {
                        sb.Append("\r\n");
                    }
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        ErrorMessage msg;
                        if (ErrorMessage.TryParse(sb.ToString(), messages.Count + 1, out msg))
                        {
                            messages.Add(msg);
                        }

                        sb.Length = 0;
                    }

                    sb.Append(matches[i].Value);
                    if (!removeNextHeader)
                    {
                        sb.Append("\r\n");
                    }
                }
            }

            if (sb.Length > 0)
            {
                ErrorMessage msg;
                if (ErrorMessage.TryParse(sb.ToString(), messages.Count + 1, out msg))
                {
                    messages.Add(msg);
                }
            }

            return messages.AsReadOnly();
        }

        /// <summary>
        /// Prints an enumeration of <see cref="ErrorMessage"/> instances to a string,
        /// formatting them in a way that is easier to review.
        /// </summary>
        /// <param name="messages">The <see cref="ErrorMessage"/>s to print.</param>
        /// <param name="colorize"><see langword="true"/> to colorize the messages, <see langword="false"/> otherwise.</param>
        /// <returns>A <see langword="string"/> with the error log contents prettily formatted.</returns>
        public static string PrintPrettyErrorLog(IEnumerable<ErrorMessage> messages, bool colorize)
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

            return string.Empty;
        }
    }
}