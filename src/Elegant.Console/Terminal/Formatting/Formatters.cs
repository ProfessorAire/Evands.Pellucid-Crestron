﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;

namespace Elegant.Terminal.Formatting
{
    public static class Formatters
    {
        /// <summary>
        /// Gets or sets a value containing the method to use to get the message to print to the console when using the
        /// <see cref="Dump"/> command. When not <see langword="null"/> this overrides the default implementation and
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
                return FormatObjectForConsole(obj, 0);
            }
        }

        /// <summary>
        /// Takes an object and creates a string with various information about the object.
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="subLevel">The number of levels into an object the printing has gone.</param>
        /// <returns>A string.</returns>
        private static string FormatObjectForConsole(object obj, int subLevel)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            var ienum = obj as System.Collections.IEnumerable;
            if (ienum != null && !(obj.GetType() == typeof(string)))
            {
                foreach (var o in ienum)
                {
                    sb.Append(' ', subLevel * 2);
                    sb.Append(FormatObjectForConsole(o, subLevel + 1));
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
                    instanceProps.Add(p.Name, p.GetValue(null, null));
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
            }

            sb.AppendLine();
            sb.Append(' ', subLevel * 2);
            sb.AppendLine(GetColorFormattedString(ConsoleBase.Colors.DumpObjectDetail, ot.FullName));
            sb.Append(' ', subLevel * 2);
            sb.AppendLine(GetColorFormattedString(ConsoleBase.Colors.DumpObjectChrome, string.Empty.PadLeft(ot.FullName.Length, '-')));

            if (obj.ToString() != ot.FullName)
            {
                sb.Append(' ', subLevel * 2);
                sb.AppendLine(GetColorFormattedString(ConsoleBase.Colors.DumpObjectDetail, obj.ToString()));
                sb.Append(' ', subLevel * 2);
                sb.AppendLine(GetColorFormattedString(ConsoleBase.Colors.DumpObjectChrome, string.Empty.PadLeft(obj.ToString().Length, '-')));
            }

            if (instanceProps.Count > 0)
            {
                foreach (var entry in instanceProps)
                {
                    if (entry.Value != null && entry.Value.ToString() == entry.Value.GetType().GetCType().FullName)
                    {
                        sb.Append(' ', subLevel * 2);
                        sb.AppendFormat("{0}=", GetColorFormattedString(ConsoleBase.Colors.DumpPropertyName, entry.Key.PadRight(width)));
                        sb.Append(FormatObjectForConsole(entry.Value, subLevel + 1));
                    }
                    else
                    {
                        sb.Append(' ', subLevel * 2);
                        sb.AppendFormat(
                            "{0}=    {1}",
                            GetColorFormattedString(ConsoleBase.Colors.DumpPropertyName, entry.Key.PadRight(width)),
                            GetColorFormattedString(ConsoleBase.Colors.DumpPropertyValue, entry.Value != null ? entry.Value.ToString() : "<null>"));
                        sb.AppendLine();
                    }
                }
            }

            if (staticProps.Count > 0)
            {
                foreach (var entry in staticProps)
                {
                    if (entry.Value != null && entry.Value.ToString() == entry.Value.GetType().GetCType().FullName)
                    {
                        sb.Append(' ', subLevel * 2);
                        sb.AppendFormat("{0}=", GetColorFormattedString(ConsoleBase.Colors.DumpPropertyName, entry.Key.PadRight(width)));
                        sb.Append(FormatObjectForConsole(entry.Value, subLevel + 1));
                    }
                    else
                    {
                        sb.Append(' ', subLevel * 2);
                        sb.AppendFormat(
                            "{0}=    {1}",
                            GetColorFormattedString(ConsoleBase.Colors.DumpPropertyName, entry.Key.PadRight(width)),
                            GetColorFormattedString(ConsoleBase.Colors.DumpPropertyValue, entry.Value != null ? entry.Value.ToString() : "<null>"));
                        sb.AppendLine();
                    }
                }
            }

            sb.Append(' ', subLevel * 2);
            sb.AppendLine(GetColorFormattedString(ConsoleBase.Colors.DumpObjectChrome, string.Empty.PadLeft(ot.FullName.Length, '-')));

            return sb.ToString();
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
            if (ConsoleBase.ColorizeOutput)
            {
                return colors.FormatText(message, args);
            }
            else
            {
                return string.Format(message, args);
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
            if (ConsoleBase.ColorizeOutput)
            {
                return GetColorFormattedString(new ColorFormat(foreground), message, args);
            }
            else
            {
                return string.Format(message, args);
            }
        }
    }
}