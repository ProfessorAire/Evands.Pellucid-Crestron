using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using System.Text.RegularExpressions;

namespace Evands.Pellucid.Terminal.Formatting.Markup
{
    public static class ConsoleMarkup
    {
        private const string formatPattern = @"\[.*\]";

        private const string colorsPattern = @"(?>(?>(?>bright)|(?>light)|b|l)?(?>(?>k|black)|(?>r(?>ed)?)|(?>g(?>reen)?)|(?>y(?>ellow)?)|(?>b(?>lue)?)|(?>m(?>agenta)?)|(?>c(?>yan)?)|(?>w(?>hite)?)))|(?>\d{1,3},\d{1,3},\d{1,3})";

        private const string corePattern = @"(?>\[:(?>(?>(?<bold>\/?b ?)|(?<italic>\/?i ?)|(?<under>\/?u ?)|(?<strike>\/?st ?)|(?<blink>\/?sb ?)|(?<reverse>\/?rv ?)|(?<frame>\/?fr ?)|(?<encircle>\/?en ?)|(?<overline>\/?ov ?)|(?<hideCursor>\/?hc ?)|(?>(?>cf:(?<fg>colors ?))|(?<closefg>\/cf ?)))|(?>(?>cb:(?<bg>colors ?))|(?<closebg>\/cb ?)))+\])|(?>\[:(?<all>\/all)\])";

        private const string getColorsPattern = @"(?>(?<bright>(?>(?>bright)|(?>light)|b|l))?(?<named>(?>k|black)|(?>r(?>ed)?)|(?>g(?>reen)?)|(?>y(?>ellow)?)|(?>b(?>lue)?)|(?>m(?>agenta)?)|(?>c(?>yan)?)|(?>w(?>hite)?)))|(?<rgb>\d{1,3},\d{1,3},\d{1,3})";

        private static Regex formatRegex;

        private static Regex coreRegex;

        private static Regex colorRegex;

        public static string ToAnsiFromConsoleMarkup(this string value)
        {
            if (!Options.Instance.EnableMarkup)
            {
                return value;
            }

            if (formatRegex == null)
            {
                formatRegex = new Regex(formatPattern, RegexOptions.Compiled);
            }

            if (coreRegex == null)
            {
                coreRegex = new Regex(corePattern.Replace("colors", colorsPattern), RegexOptions.Compiled);
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
                        sb.Append(match.Groups["hideCursor"].Success ? match.Groups["hideCursor"].Value[0] == '/' ? "?25h" : "?25l" : string.Empty);
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

        internal static string GetColor(string value, bool isBackground)
        {
            if (colorRegex == null)
            {
                colorRegex = new Regex(getColorsPattern, RegexOptions.Compiled);
            }

            var match = colorRegex.Match(value);
            if (match.Success)
            {
                var bright = match.Groups["bright"].Success;
                if (match.Groups["named"].Success)
                {
                    return string.Format("{0};", (GetColorValue(match.Groups["named"].Value, bright) + (isBackground ? 40 : 30)));
                }
                else if (match.Groups["rgb"].Success)
                {
                    var values = match.Groups["rgb"].Value.Split(',');
                    return string.Format("{0};2;{1};{2};{3};", isBackground ? 48 : 38, values[0], values[1], values[2]);
                }
            }

            return string.Empty;
        }
    }
}