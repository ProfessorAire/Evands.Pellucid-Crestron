#region copyright
// <copyright file="ConsoleMarkup.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Formatting.Markup
{
    /// <summary>
    /// Provides extension methods for formatting text using Pellucid Console Markup.
    /// </summary>
    public static class ConsoleMarkup
    {
        /// <summary>
        /// The pattern for basic format objects.
        /// </summary>
        private const string FormatPattern = @"\[.*\]";

        /// <summary>
        /// The pattern for color objects.
        /// </summary>
        private const string ColorsPattern = @"(?>(?>(?>bright)|(?>light)|b|l)?(?>(?>k|black)|(?>r(?>ed)?)|(?>g(?>reen)?)|(?>y(?>ellow)?)|(?>b(?>lue)?)|(?>m(?>agenta)?)|(?>c(?>yan)?)|(?>w(?>hite)?)))|(?>\d{1,3},\d{1,3},\d{1,3})";

        /// <summary>
        /// The pattern for overall markup.
        /// </summary>
        private const string CorePattern = @"(?>\[:(?>(?>(?<bold>\/?b ?)|(?<italic>\/?i ?)|(?<under>\/?u ?)|(?<strike>\/?st ?)|(?<blink>\/?sb ?)|(?<reverse>\/?rv ?)|(?<frame>\/?fr ?)|(?<encircle>\/?en ?)|(?<overline>\/?ov ?)|(?<hideCursor>\/?hc ?)|(?>(?>cf:(?<fg>colors ?))|(?<closefg>\/cf ?)))|(?>(?>cb:(?<bg>colors ?))|(?<closebg>\/cb ?)))+\])|(?>\[:(?<all>\/all)\])";

        /// <summary>
        /// The pattern for parsing specific colors.
        /// </summary>
        private const string GetColorsPattern = @"(?>(?<bright>(?>(?>bright)|(?>light)|b|l))?(?<named>(?>k|black)|(?>r(?>ed)?)|(?>g(?>reen)?)|(?>y(?>ellow)?)|(?>b(?>lue)?)|(?>m(?>agenta)?)|(?>c(?>yan)?)|(?>w(?>hite)?)))|(?<rgb>\d{1,3},\d{1,3},\d{1,3})";

        /// <summary>
        /// The regex for parsing specific formats.
        /// </summary>
        private static Regex formatRegex;

        /// <summary>
        /// The regex for parsing the overall format.
        /// </summary>
        private static Regex coreRegex;

        /// <summary>
        /// The regex for parsing colors.
        /// </summary>
        private static Regex colorRegex;

        /// <summary>
        /// Takes a string and replaces markup declarations with ANSI format specifiers.
        /// <para>For example, the string "[:ib]This is bold and italic.[:/all]" will be
        /// replaced with "\x1b[1;3mThis is bold and italic.\x1b[0m".</para>
        /// <para>See the project Github Wiki for more information about Pellucid Console
        /// Markup.</para>
        /// </summary>
        /// <param name="value">The <see langword="string"/> value to format.</param>
        /// <returns>A <see langword="string"/> with the markup replace by ANSI format specifiers.</returns>
        public static string ToAnsiFromConsoleMarkup(this string value)
        {
            if (!Options.Instance.EnableMarkup)
            {
                return value;
            }

            if (formatRegex == null)
            {
                formatRegex = new Regex(FormatPattern, RegexOptions.Compiled);
            }

            if (coreRegex == null)
            {
                coreRegex = new Regex(CorePattern.Replace("colors", ColorsPattern), RegexOptions.Compiled);
            }

            var formatMatches = formatRegex.Matches(value);
            var resultBuilder = new StringBuilder(value);

            for (var fi = 0; fi < formatMatches.Count; fi++)
            {
                var coreMatches = coreRegex.Matches(formatMatches[fi].Value);
                var sb = new StringBuilder(32);

                for (var ci = 0; ci < coreMatches.Count; ci++)
                {
                    sb.Append("\x1b[");
                    var match = coreMatches[ci];
                    if (match.Groups["all"].Success)
                    {
                        sb.Append('0').Append('m');
                        resultBuilder.Replace(match.Value, sb.ToString());
                    }
                    else if (match.Groups["hideCursor"].Success)
                    {
                        sb.Append(match.Groups["hideCursor"].Success ? match.Groups["hideCursor"].Value[0] == '/' ? "?25h" : "?25l" : string.Empty);
                        resultBuilder.Replace(match.Value, sb.ToString());
                    }
                    else
                    {
                        sb.Append(match.Groups["bold"].Success ? match.Groups["bold"].Value[0] == '/' ? "22;" : "1;" : string.Empty);
                        sb.Append(match.Groups["italic"].Success ? match.Groups["italic"].Value[0] == '/' ? "23;" : "3;" : string.Empty);
                        sb.Append(match.Groups["under"].Success ? match.Groups["under"].Value[0] == '/' ? "24;" : "4;" : string.Empty);
                        sb.Append(match.Groups["blink"].Success ? match.Groups["blink"].Value[0] == '/' ? "25;" : "5;" : string.Empty);
                        sb.Append(match.Groups["reverse"].Success ? match.Groups["reverse"].Value[0] == '/' ? "27;" : "7;" : string.Empty);
                        sb.Append(match.Groups["strike"].Success ? match.Groups["strike"].Value[0] == '/' ? "29;" : "9;" : string.Empty);
                        sb.Append(match.Groups["frame"].Success ? match.Groups["frame"].Value[0] == '/' ? "54;" : "51;" : string.Empty);
                        sb.Append(match.Groups["encircle"].Success ? match.Groups["encircle"].Value[0] == '/' ? "54;" : "52;" : string.Empty);
                        sb.Append(match.Groups["overline"].Success ? match.Groups["overline"].Value[0] == '/' ? "55;" : "53;" : string.Empty);
                        sb.Append(match.Groups["fg"].Success ? GetColor(match.Groups["fg"].Value, false) : string.Empty);
                        sb.Append(match.Groups["bg"].Success ? GetColor(match.Groups["bg"].Value, true) : string.Empty);
                        sb.Append(match.Groups["closefg"].Success ? "39;" : string.Empty);
                        sb.Append(match.Groups["closebg"].Success ? "49;" : string.Empty);

                        if (sb.Length > 2)
                        {
                            sb.Replace(';', 'm', sb.Length - 1, 1);
                            resultBuilder.Replace(match.Value, sb.ToString());
                        }
                    }

                    sb.Length = 0;
                }
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Gets the ANSI color format from a markup color tag.
        /// </summary>
        /// <param name="value">The markup color tag to parse.</param>
        /// <param name="isBackground">A value indicating whether the markup color tag is for the background color.</param>
        /// <returns>A <see langword="string"/> with the ANSI color specifier for the specified markup color.</returns>
        internal static string GetColor(string value, bool isBackground)
        {
            if (colorRegex == null)
            {
                colorRegex = new Regex(GetColorsPattern, RegexOptions.Compiled);
            }

            var match = colorRegex.Match(value);
            if (match.Success)
            {
                var bright = match.Groups["bright"].Success;
                if (match.Groups["named"].Success)
                {
                    return string.Format("{0};", GetColorValue(match.Groups["named"].Value, bright) + (isBackground ? 40 : 30));
                }
                else if (match.Groups["rgb"].Success)
                {
                    var values = match.Groups["rgb"].Value.Split(',');
                    return string.Format("{0};2;{1};{2};{3};", isBackground ? 48 : 38, values[0], values[1], values[2]);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the integer color values from strings.
        /// </summary>
        /// <param name="value">The string containing the color descriptor.</param>
        /// <param name="bright">A value indicating whether the colors are bright/light.</param>
        /// <returns>An integer representing the ANSI color format.</returns>
        private static int GetColorValue(string value, bool bright)
        {
            var result = 0;
            switch (value)
            {
                case "k":
                case "black":
                    result = 0;
                    break;
                case "r":
                case "red":
                    result = 1;
                    break;
                case "g":
                case "green":
                    result = 2;
                    break;
                case "y":
                case "yellow":
                    result = 3;
                    break;
                case "b":
                case "blue":
                    result = 4;
                    break;
                case "m":
                case "magenta":
                    result = 5;
                    break;
                case "c":
                case "cyan":
                    result = 6;
                    break;
                case "w":
                case "white":
                    result = 7;
                    break;
            }

            return result + (bright ? 60 : 0);
        }
    }
}