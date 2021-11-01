#region copyright
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
                return DumpHelpers.DumpFactory.GetNode(obj).ToString();
            }
        }

        /// <summary>
        /// Takes an object and creates a string with various information about the object.
        /// <para>Only works with the built-in method for dumping to console. Will not use
        /// the <see cref="PrepareObjectToDumpToConsoleMethod"/> override.</para>
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="maxDepth">The maximum number of times to recurse on complex objects.
        /// <para>A value of 1 would indicate dumping the top-level properties on the object provided,
        /// while ignoring properties that are complex objects with additional properties of their own.</para>
        /// <para>A value of 0 indicates no max depth.</para>
        /// </param>
        /// <returns>A string.</returns>
        public static string FormatObjectForConsole(object obj, int maxDepth)
        {
            return DumpHelpers.DumpFactory.GetNode(obj).ToString(maxDepth);
        }

        /// <summary>
        /// Takes an object and creates a string with various information about the object.
        /// <para>Only works with the built-in method for dumping to console. Will not use
        /// the <see cref="PrepareObjectToDumpToConsoleMethod"/> override.</para>
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's fully qualified
        /// name, otherwise just the short name.</param>
        /// <returns>A string.</returns>
        public static string FormatObjectForConsole(object obj, bool useFullTypeNames)
        {
            return DumpHelpers.DumpFactory.GetNode(obj).ToString(useFullTypeNames);
        }

        /// <summary>
        /// Takes an object and creates a string with various information about the object.
        /// <para>Only works with the built-in method for dumping to console. Will not use
        /// the <see cref="PrepareObjectToDumpToConsoleMethod"/> override.</para>
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="maxDepth">The maximum number of times to recurse on complex objects.
        /// <para>A value of 1 would indicate dumping the top-level properties on the object provided,
        /// while ignoring properties that are complex objects with additional properties of their own.</para>
        /// <para>A value of 0 indicates no max depth.</para>
        /// </param>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's fully qualified
        /// name, otherwise just the short name.</param>
        /// <returns>A string.</returns>
        public static string FormatObjectForConsole(object obj, int maxDepth, bool useFullTypeNames)
        {
            return DumpHelpers.DumpFactory.GetNode(obj).ToString(maxDepth, useFullTypeNames);
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
    }
}