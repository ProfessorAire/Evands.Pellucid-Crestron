#region copyright
// <copyright file="Debug.cs" company="Christopher McNeely">
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
using System.Collections.ObjectModel;
using System.Text;
using Crestron.SimplSharp;
using Evands.Pellucid.Terminal.Formatting;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Commands and values related to debug messaging.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Tracks line writing states.
        /// </summary>
        private static bool isLastWriteALine = true;

        /// <summary>
        /// A Dictionary of classes registered with the Logger to use unique header colors.
        /// </summary>
        private static Dictionary<string, ColorFormat> registeredClasses = new Dictionary<string, ColorFormat>(0);

        /// <summary>
        /// Gets a list of the class names registered with unique <see cref="ColorFormat"/>s to use for header text.
        /// </summary>
        public static ReadOnlyDictionary<string, ColorFormat> RegisteredClasses
        {
            get { return new ReadOnlyDictionary<string, ColorFormat>(registeredClasses); }
        }

        /// <summary>
        /// Registers or reregisters an object type to use specific colors for debug headers.
        /// </summary>
        /// <param name="obj">The object to register.</param>
        /// <param name="colors">The ColorFormat to use for this object type's debug messages.</param>
        public static void RegisterHeaderObject(object obj, ColorFormat colors)
        {
            if (obj != null)
            {
                var key = string.Empty;
                var str = obj as string;

                if (!string.IsNullOrEmpty(str))
                {
                    key = str;
                }
                else
                {
                    key = obj.GetType().FullName;
                }

                if (registeredClasses.ContainsKey(key))
                {
                    registeredClasses[key] = colors;
                }
                else
                {
                    registeredClasses.Add(key, colors);
                }
            }
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with the specified color formatting.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="colors">The ColorFormat to use when writing the message.</param>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(object obj, Terminal.Formatting.ColorFormat colors, string message, params object[] args)
        {
            if (IsValid(obj))
            {
                var header = GetMessageHeader(obj, true, true);

                message = Formatters.GetColorFormattedString(colors, message, args);

                Write("{0}{1}", header, message);
            }
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with the specified color formatting.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="foreground">The message foreground color.</param>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(object obj, Terminal.ColorCode foreground, string message, params object[] args)
        {
            Write(obj, foreground, Terminal.ColorCode.None, message, args);
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with the specified color formatting.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="foreground">The message foreground color.</param>
        /// <param name="background">The message background color.</param>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(object obj, Terminal.ColorCode foreground, Terminal.ColorCode background, string message, params object[] args)
        {
            Write(obj, new ColorFormat(foreground, background), message, args);
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with no header and no additional color formatting applied.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Uncategorized))
            {
                ConsoleBase.Write(message, args);
                isLastWriteALine = false;
            }
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with the specified color formatting.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="colors">The ColorFormat to use when writing the message.</param>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(object obj, Terminal.Formatting.ColorFormat colors, string message, params object[] args)
        {
            if (IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(colors, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    WriteLine("{0}{1}", header, message);
                }
                else
                {
                    WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with the specified color formatting.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="foreground">The message foreground color.</param>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(object obj, Terminal.ColorCode foreground, string message, params object[] args)
        {
            WriteLine(obj, foreground, Terminal.ColorCode.None, message, args);
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with the specified color formatting.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="foreground">The message foreground color.</param>
        /// <param name="background">The message background color.</param>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(object obj, Terminal.ColorCode foreground, Terminal.ColorCode background, string message, params object[] args)
        {
            WriteLine(obj, new ColorFormat(foreground, background), message, args);
        }

        /// <summary>
        /// Writes an uncategorized message to the console, with no header and no additional color formatting applied.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Uncategorized))
            {
                ForceWriteLine(message, args);
            }
        }

        /// <summary>
        /// Writes a debug message to the console, using the console's DebugColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteDebug(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Debug) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Debug, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWrite("{0}{1}", header, message);
                }
                else
                {
                    ForceWrite(message);
                }
            }
        }

        /// <summary>
        /// Writes a debug message to the console, using the console's DebugColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteDebugLine(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Debug) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Debug, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWriteLine("{0}{1}", header, message);
                }
                else
                {
                    ForceWriteLine(Formatters.GetColorFormattedString(ConsoleBase.Colors.Debug, message, args));
                }
            }
        }

        /// <summary>
        /// Writes a success message to the console, using the console's SuccessColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteSuccess(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Success) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Success, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWrite("{0}{1}", header, message);
                }
                else
                {
                    ForceWrite(message);
                }
            }
        }

        /// <summary>
        /// Writes a success message to the console, using the console's SuccessColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteSuccessLine(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Success) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Success, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWriteLine("{0}{1}", header, message);
                }
                else
                {
                    ForceWriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes a progress message to the console, using the console's ProgressColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteProgress(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Progress) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Progress, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWrite("{0}{1}", header, message);
                }
                else
                {
                    ForceWrite(message);
                }
            }
        }

        /// <summary>
        /// Writes a progress message to the console, using the console's ProgressColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteProgressLine(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Progress) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Progress, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWriteLine("{0}{1}", header, message);
                }
                else
                {
                    ForceWriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes a notice message to the console, using the console's Notice ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteNotice(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Notice) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Notice, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWrite("{0}{1}", header, message);
                }
                else
                {
                    ForceWrite(message);
                }
            }
        }

        /// <summary>
        /// Writes a notice message to the console, using the console's Notice ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteNoticeLine(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Notice) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Notice, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWriteLine("{0}{1}", header, message);
                }
                else
                {
                    ForceWriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes a warning message to the console, using the console's Warning ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteWarning(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Warning) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Warning, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWrite("{0}{1}", header, message);
                }
                else
                {
                    ForceWrite(message);
                }
            }
        }

        /// <summary>
        /// Writes a warning message to the console, using the console's Warning ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteWarningLine(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Warning) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Warning, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWriteLine("{0}{1}", header, message);
                }
                else
                {
                    ForceWriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes an error message to the console, using the console's ErrorColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteError(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Error) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Error, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWrite("{0}{1}", header, message);
                }
                else
                {
                    ForceWrite(message);
                }
            }
        }

        /// <summary>
        /// Writes an error message to the console, using the console's ErrorColors ColorFormat.
        /// </summary>
        /// <param name="obj">The object the message is originating from. Can be null.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteErrorLine(object obj, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Error) && IsValid(obj))
            {
                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Error, message, args);

                if (isLastWriteALine)
                {
                    var header = GetMessageHeader(obj, true, true);
                    ForceWriteLine("{0}{1}", header, message);
                }
                else
                {
                    ForceWriteLine(message);
                }
            }
        }

        /// <summary>
        /// Writes an exception to the console.
        /// </summary>
        /// <param name="obj">The object the exception originated from. Can be null.</param>
        /// <param name="ex">The exception being written.</param>
        /// <param name="message">The message to write before the exception.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteException(object obj, Exception ex, string message, params object[] args)
        {
            if (Options.Instance.DebugLevels.Contains(DebugLevels.Exception) && IsValid(obj))
            {
                var sb = new StringBuilder();
                if (!isLastWriteALine)
                {
                    isLastWriteALine = true;
                    ConsoleBase.WriteLine();
                }

                var header = GetMessageHeader(obj, true, true);

                message = Formatters.GetColorFormattedString(ConsoleBase.Colors.Exception, message, args);

                sb.AppendFormat("{0}{1}", header, message);

                int exceptionIndex = 0;

                var exceptionFormat = ConsoleBase.Colors.Exception;

                while (ex != null)
                {
                    exceptionIndex++;
                    sb.Append(exceptionFormat.FormatText(false, "{1}--------Exception {0}--------{1}", exceptionIndex, ConsoleBase.NewLine));
                    sb.AppendFormat("{0}", ex.ToString().Replace(Environment.NewLine, ConsoleBase.NewLine));
                    sb.Append(exceptionFormat.FormatText(true, "{0}-----------------------------", ConsoleBase.NewLine));
                    ex = ex.InnerException;
                }

                ForceWriteLine(
                    Formatters.GetColorFormattedString(ConsoleBase.Colors.Exception, sb.ToString()));
            }
        }

        /// <summary>
        /// Adds a value to the suppression list.
        /// </summary>
        /// <param name="valueToAdd">The value to add to the list.</param>
        /// <returns><see langword="true"/> if the value was added, <see langword="false"/> if it wasn't for any reason.</returns>
        public static bool AddSuppression(string valueToAdd)
        {
            if (Options.Instance.Suppressed != null && !Options.Instance.Suppressed.Contains(valueToAdd))
            {
                Options.Instance.Suppressed.Add(valueToAdd);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a value from the suppression list.
        /// </summary>
        /// <param name="valueToRemove">The value to remove from the suppression list.</param>
        /// <returns><see langword="true"/> if the value was removed, <see langword="false"/> if it wasn't for any reason.</returns>
        public static bool RemoveSuppression(string valueToRemove)
        {
            if (Options.Instance.Suppressed != null)
            {
                return Options.Instance.Suppressed.Remove(valueToRemove);
            }

            return false;
        }

        /// <summary>
        /// Adds a value to the allowed list.
        /// </summary>
        /// <param name="valueToAdd">The value to add to the list.</param>
        /// <returns><see langword="true"/> if the value was added, <see langword="false"/> if it wasn't for any reason.</returns>
        public static bool AddAllowed(string valueToAdd)
        {
            if (Options.Instance.Allowed != null && !Options.Instance.Allowed.Contains(valueToAdd))
            {
                Options.Instance.Allowed.Add(valueToAdd);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a value from the allowed list.
        /// </summary>
        /// <param name="valueToRemove">The value to remove from the allowed list.</param>
        /// <returns><see langword="true"/> if the value was removed, <see langword="false"/> if it wasn't for any reason.</returns>
        public static bool RemoveAllowed(string valueToRemove)
        {
            if (Options.Instance.Allowed != null)
            {
                return Options.Instance.Allowed.Remove(valueToRemove);
            }

            return false;
        }

        /// <summary>
        /// Returns a header snippet based on the object provided.
        /// </summary>
        /// <param name="obj">The object to use when formatting the header.<para>Can be null.</para></param>
        /// <returns>A string with header information for a terminal message.</returns>
        [Obsolete("This method has been deprecated and will be removed in a future version.", false)]
        public static string GetMessageHeaderWithTimestamp(object obj)
        {
            return GetMessageHeader(obj, true, true);
        }

        /// <summary>
        /// Returns a header snippet based on the object provided. Has no timestamp and no color.
        /// </summary>
        /// <param name="obj">The object to use when formatting the header.<para>Can be null.</para></param>
        /// <returns>A string with header information for a terminal message.</returns>
        public static string GetMessageHeader(object obj)
        {
            return GetMessageHeader(obj, false, false);
        }

        /// <summary>
        /// Returns a header snippet based on the object provided.
        /// </summary>
        /// <param name="obj">The object to use when formatting the header.<para>Can be null.</para></param>
        /// <param name="withTimestamp">Indicates whether to get a timestamp, provided the <see cref="Options.UseTimestamps"/> property is true.</param>
        /// <param name="withColor">Indicates whether to get a colored header, provided the <see cref="Options.ColorizeConsoleOutput"/> property is true.</param>
        /// <returns>A string with header information for a terminal message.</returns>
        public static string GetMessageHeader(object obj, bool withTimestamp, bool withColor)
        {
            var header = string.Empty;

            if (isLastWriteALine)
            {
                var stamp = (withTimestamp && Options.Instance.UseTimestamps) ? GetTimestamp() : string.Empty;
                var optional = !string.IsNullOrEmpty(ConsoleBase.OptionalHeader) ? ConsoleBase.OptionalHeader : string.Empty;
                if (obj == null)
                {
                    header = string.Format("{0}{1}", optional, stamp);
                }
                else if (obj as string != null)
                {
                    header = string.Format("{0}{1}[{2}]", optional, stamp, obj);
                }
                else
                {
                    var debugObject = obj as IDebugData;
                    if (debugObject != null)
                    {
                        header = string.Format("{0}{1}[{2}]", optional, stamp, debugObject.Header);
                    }
                    else
                    {
                        header = string.Format("{0}{1}[{2}]", optional, stamp, obj.GetType().Name);
                    }
                }

                if (!string.IsNullOrEmpty(header))
                {
                    if (withColor && Options.Instance.ColorizeConsoleOutput)
                    {
                        var colors = GetHeaderColors(obj);
                        header = string.Format("{0} ", colors.FormatText(header));
                    }
                    else
                    {
                        header = string.Format("{0} ", header);
                    }
                }
            }

            return header;
        }

        /// <summary>
        /// Gets the colors to use for an object's header for debugging.
        /// </summary>
        /// <param name="obj">The object to get a color format for.</param>
        /// <returns>A <see cref="ColorFormat"/> object.</returns>
        private static ColorFormat GetHeaderColors(object obj)
        {
            if (obj != null)
            {
                var key = obj.GetType().FullName;
                if (registeredClasses.ContainsKey(key))
                {
                    return registeredClasses[key];
                }

                var debugObject = obj as IDebugData;
                if (debugObject != null)
                {
                    return debugObject.HeaderColor;
                }
            }

            return ConsoleBase.Colors.Subtle;
        }

        /// <summary>
        /// Always writes a message to the console, with no additional color formatting applied.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        private static void ForceWrite(string message, params object[] args)
        {
            ConsoleBase.WriteNoHeader(message, args);
            isLastWriteALine = false;
        }

        /// <summary>
        /// Always writes a message to the console, with no additional color formatting applied.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        private static void ForceWriteLine(string message, params object[] args)
        {
            ConsoleBase.WriteLineNoHeader(message, args);
            isLastWriteALine = true;
        }

        /// <summary>
        /// Gets a correctly formatted timestamp.
        /// </summary>
        /// <returns>A <see langword="string"/> with a properly formatted timestamp.</returns>
        private static string GetTimestamp()
        {
            if (!Options.Instance.UseTimestamps)
            {
                return string.Empty;
            }

            if (Options.Instance.Use24HourTime)
            {
                return string.Format("[{0}]", CrestronEnvironment.GetLocalTime().ToString("HH:mm:ss"));
            }

            return string.Format("[{0}]", CrestronEnvironment.GetLocalTime().ToLongTimeString());
        }

        /// <summary>
        /// Checks an object to see if it has been suppressed or allowed.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>True if it is allowed to print to the console.</returns>
        private static bool IsValid(object obj)
        {
            var allowed = Options.Instance.Allowed;
            var suppressed = Options.Instance.Suppressed;

            if ((allowed.Count == 0 && suppressed.Count == 0) || obj == null)
            {
                return true;
            }

            var name = string.Empty;

            var data = obj as IDebugData;
            if (data != null)
            {
                name = data.Header;
            }
            else
            {
                name = obj as string;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = obj.GetType().Name;
            }

            if (allowed.Count > 0)
            {
                return allowed.Contains(name);
            }
            else
            {
                return !suppressed.Contains(name);
            }
        }
    }
}