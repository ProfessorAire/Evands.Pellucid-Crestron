﻿#region copyright
// <copyright file="Formatters.cs" company="Christopher McNeely">
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
using Crestron.SimplSharp.Reflection;

namespace Evands.Pellucid.Terminal.Formatting
{
    /// <summary>
    /// A variety of common formatting methods.
    /// </summary>
    public static class Formatters
    {
        /// <summary>
        /// Gets or sets a value containing the method to use to get the message to print to the console when using the
        /// Dump extension method. When not <see langword="null"/> this overrides the default implementation and
        /// can be used to customize the way in which objects are dumped to console.
        /// </summary>
        public static Func<object, StringBuilder> PrepareObjectToDumpToConsoleMethod { get; set; }

        /// <summary>
        /// Takes an object and creates a string with various information about the object.
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <returns>A string.</returns>
        public static string FormatObjectForConsole(object obj)
        {
            if (PrepareObjectToDumpToConsoleMethod != null)
            {
                return PrepareObjectToDumpToConsoleMethod(obj).ToString();
            }
            else
            {
                return FormatObjectForConsole(obj, 0, 0);
            }
        }

        /// <summary>
        /// Gets a string formatted with terminal color escape codes for foreground and background colors.
        /// </summary>
        /// <param name="colors">The colors to use for the text.</param>
        /// <param name="message">The message to format.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        /// <returns>A string with the fully formatted message.</returns>
        public static string GetColorFormattedString(ColorFormat colors, string message, params object[] args)
        {
            if (Options.Instance.ColorizeConsoleOutput)
            {
                return colors.FormatText(message, args);
            }
            else
            {
                return message.OptionalFormat(args);
            }
        }

        /// <summary>
        /// Gets a string formatted with terminal color escape codes for foreground and background colors.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="background">The background color of the text.</param>
        /// <param name="message">The message to format.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        /// <returns>A string with the fully formatted message.</returns>
        public static string GetColorFormattedString(ColorCode foreground, ColorCode background, string message, params object[] args)
        {
            return GetColorFormattedString(new ColorFormat(foreground, background), message, args);
        }

        /// <summary>
        /// Gets a string formatted with terminal color escape codes for foreground colors only.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="message">The message to format.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        /// <returns>A string with the color formatting applied.</returns>
        public static string GetColorFormattedString(ColorCode foreground, string message, params object[] args)
        {
            return GetColorFormattedString(new ColorFormat(foreground), message, args);
        }

        /// <summary>
        /// Takes an object and creates a string with various information about the object.
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="subLevel">The number of levels into an object the printing has gone.</param>
        /// <param name="minListPadding">The minimum number of characters to pad a list entry by.</param>
        /// <returns>A string.</returns>
        private static string FormatObjectForConsole(object obj, int subLevel, int minListPadding)
        {
            if (obj == null)
            {
                return "<null>";
            }

            var sb = new StringBuilder();

            var objIsValue = obj.GetType().GetCType().IsValueType;
            if (objIsValue || obj is string)
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    sb.Append(ConsoleBase.Colors.DumpPropertyValue.FormatText("<empty string>"));
                    sb.Append(ConsoleBase.NewLine);
                }
                else
                {
                    sb.Append(ConsoleBase.Colors.DumpPropertyValue.FormatText(obj.ToString()));
                    sb.Append(ConsoleBase.NewLine);
                }

                return sb.ToString();
            }

            var idict = obj as System.Collections.IDictionary;
            if (idict != null)
            {
                var index = 0;
                foreach (var key in idict.Keys)
                {
                    try
                    {
                        if (index == 0)
                        {
                            sb.Append(ConsoleBase.NewLine);
                        }

                        sb.Append(' ', minListPadding);
                        sb.AppendFormat("Key{0}".PadRight(12, ' '), index);
                        sb.Append("= ");
                        sb.Append(FormatObjectForConsole(key, 0, (subLevel * 2) + minListPadding + 12));

                        sb.Append(' ', minListPadding);
                        sb.AppendFormat("Value{0}".PadRight(12, ' '), index);
                        sb.Append("= ");
                        sb.Append(FormatObjectForConsole(idict[key], 0, (subLevel * 2) + minListPadding + 12));
                        index++;
                    }
                    catch
                    {
                    }
                }

                return sb.ToString();
            }

            var icoll = obj as System.Collections.ICollection;
            if (icoll != null && !(obj.GetType() == typeof(string)))
            {
                var index = 0;
                foreach (var o in icoll)
                {
                    if (index > 0)
                    {
                        sb.Append(' ', minListPadding);
                    }

                    sb.Append(FormatObjectForConsole(o, 0, minListPadding));
                    index++;
                }

                if (index == 0)
                {
                    sb.Append(ConsoleBase.Colors.DumpPropertyValue.FormatText("<empty>"));
                    sb.Append(ConsoleBase.NewLine);
                }

                return sb.ToString();
            }

            var ienum = obj as System.Collections.IEnumerable;
            if (ienum != null && !(obj.GetType() == typeof(string)))
            {
                var index = 0;
                foreach (var o in ienum)
                {
                    if (index > 0)
                    {
                        sb.Append(' ', minListPadding);
                    }

                    sb.Append(FormatObjectForConsole(o, 0, minListPadding));
                    index++;
                }

                if (index == 0)
                {
                    sb.Append(ConsoleBase.Colors.DumpPropertyValue.FormatText("<empty>"));
                    sb.Append(ConsoleBase.NewLine);
                }

                return sb.ToString();
            }

            var width = 0;
            var instanceProps = new Dictionary<string, object>();
            var staticProps = new Dictionary<string, object>();
            var ot = obj.GetType().GetCType();

            foreach (var p in ot.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                try
                {
                    instanceProps.Add(p.Name, p.GetValue(obj, null));
                }
                catch
                {
                }
            }

            foreach (var p in ot.GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                try
                {
                    var val = p.GetValue(null, null);
                    if (obj != val)
                    {
                        staticProps.Add(p.Name, val);
                    }
                }
                catch
                {
                }
            }

            if (instanceProps.Count > 0)
            {
                width = instanceProps.Keys.Max(k => k.Length) + 4;
            }

            if (staticProps.Count > 0)
            {
                var width2 = staticProps.Keys.Max(k => k.Length) + 4;

                width = Math.Max(width, width2);
                width2 = width;
            }

            sb.Append(' ', subLevel * 2);
            sb.Append(GetColorFormattedString(ConsoleBase.Colors.DumpObjectDetail, ot.FullName));
            sb.Append(ConsoleBase.NewLine);
            sb.Append(' ', (subLevel * 2) + minListPadding);
            sb.Append(GetColorFormattedString(ConsoleBase.Colors.DumpObjectChrome, string.Empty.PadLeft(ot.FullName.Length, '-')));
            sb.Append(ConsoleBase.NewLine);

            if (obj.ToString() != ot.FullName)
            {
                sb.Append(' ', (subLevel * 2) + minListPadding);
                sb.Append(GetColorFormattedString(ConsoleBase.Colors.DumpObjectDetail, obj.ToString()));
                sb.Append(ConsoleBase.NewLine);
                sb.Append(' ', (subLevel * 2) + minListPadding);
                sb.Append(GetColorFormattedString(ConsoleBase.Colors.DumpObjectChrome, string.Empty.PadLeft(obj.ToString().Length, '-')));
                sb.Append(ConsoleBase.NewLine);
            }

            if (instanceProps.Count > 0)
            {
                foreach (var entry in instanceProps)
                {
                    sb.Append(ProcessEntry(entry, width, subLevel, minListPadding));
                }
            }

            if (staticProps.Count > 0)
            {
                foreach (var entry in staticProps)
                {
                    sb.Append(ProcessEntry(entry, width, subLevel, minListPadding));
                }
            }

            sb.Append(' ', (subLevel * 2) + minListPadding);
            sb.Append(GetColorFormattedString(ConsoleBase.Colors.DumpObjectChrome, string.Empty.PadLeft(ot.FullName.Length, '-')));
            sb.Append(ConsoleBase.NewLine);

            return sb.ToString();
        }

        private static string ProcessEntry(KeyValuePair<string, object> entry, int width, int subLevel, int minListPadding)
        {
            var sb = new StringBuilder();
            var isValueType = entry.Value != null && entry.Value.GetType().GetCType().IsValueType;
            if (isValueType)
            {
                sb.Append(' ', (subLevel * 2) + minListPadding);
                sb.AppendFormat(
                    "{0}=    {1}",
                    GetColorFormattedString(ConsoleBase.Colors.DumpPropertyName, entry.Key.PadRight(width)),
                    GetColorFormattedString(ConsoleBase.Colors.DumpPropertyValue, entry.Value != null ? entry.Value.ToString() : "<null>"));
                if (entry.Value != null)
                {
                    sb.Append(ConsoleBase.NewLine);
                }
            }
            else if (entry.Value != null && (entry.Value.ToString() == entry.Value.GetType().GetCType().FullName || !entry.Value.GetType().GetCType().IsValueType))
            {
                sb.Append(' ', (subLevel * 2) + minListPadding);
                sb.AppendFormat("{0}=    ", GetColorFormattedString(ConsoleBase.Colors.DumpPropertyName, entry.Key.PadRight(width)));
                sb.Append(FormatObjectForConsole(entry.Value, subLevel, (subLevel * 2) + minListPadding + width + 5));
            }
            else
            {
                sb.Append(' ', (subLevel * 2) + minListPadding);
                sb.AppendFormat(
                    "{0}=    {1}",
                    GetColorFormattedString(ConsoleBase.Colors.DumpPropertyName, entry.Key.PadRight(width)),
                    GetColorFormattedString(ConsoleBase.Colors.DumpPropertyValue, FormatObjectForConsole(entry.Value != null ? entry.Value : "<null>", subLevel + 1, (subLevel * 2) + minListPadding + width + 5)));
                if (entry.Value != null)
                {
                    sb.Append(ConsoleBase.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}