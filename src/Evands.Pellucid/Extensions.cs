#region copyright
// <copyright file="Extensions.cs" company="Christopher McNeely">
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

using Evands.Pellucid.Terminal.Formatting.Markup;

namespace Evands.Pellucid
{
    /// <summary>
    /// Provides extension methods for some useful commands.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class Extensions
    {
        /// <summary>
        /// Dumps the specified object to the console.
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <typeparam name="T">The type of the object being dumped.</typeparam>
        /// <returns>The object being dumped.</returns>
        public static T Dump<T>(this T obj)
        {
            if (Crestron.SimplSharp.CrestronEnvironment.ProgramCompatibility == Crestron.SimplSharp.eCrestronSeries.Series3)
            {
                ConsoleBase.WriteLine();
            }

            ConsoleBase.WriteNoHeader(Terminal.Formatting.Formatters.FormatObjectForConsole(obj));
            return obj;
        }

        /// <summary>
        /// Dumps the specified object to the console.
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="maxDepth">The maximum number of times to recurse on complex objects.
        /// <para>A value of 1 would indicate dumping the top-level properties on the object provided,
        /// while ignoring properties that are complex objects with additional properties of their own.</para>
        /// <para>A value of 0 indicates no max depth.</para>
        /// </param>
        /// <typeparam name="T">The type of the object being dumped.</typeparam>
        /// <returns>The object being dumped.</returns>
        public static T Dump<T>(this T obj, int maxDepth)
        {
            if (Crestron.SimplSharp.CrestronEnvironment.ProgramCompatibility == Crestron.SimplSharp.eCrestronSeries.Series3)
            {
                ConsoleBase.WriteLine();
            }

            ConsoleBase.WriteNoHeader(Terminal.Formatting.Formatters.FormatObjectForConsole(obj, maxDepth));
            return obj;
        }

        /// <summary>
        /// Dumps the specified object to the console.
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's fully qualified
        /// name, otherwise just the short name.</param>
        /// <typeparam name="T">The type of the object being dumped.</typeparam>
        /// <returns>The object being dumped.</returns>
        public static T Dump<T>(this T obj, bool useFullTypeNames)
        {
            if (Crestron.SimplSharp.CrestronEnvironment.ProgramCompatibility == Crestron.SimplSharp.eCrestronSeries.Series3)
            {
                ConsoleBase.WriteLine();
            }

            ConsoleBase.WriteNoHeader(Terminal.Formatting.Formatters.FormatObjectForConsole(obj, useFullTypeNames));
            return obj;
        }

        /// <summary>
        /// Dumps the specified object to the console.
        /// </summary>
        /// <param name="obj">The object to format details about.</param>
        /// <param name="maxDepth">The maximum number of times to recurse on complex objects.
        /// <para>A value of 1 would indicate dumping the top-level properties on the object provided,
        /// while ignoring properties that are complex objects with additional properties of their own.</para>
        /// <para>A value of 0 indicates no max depth.</para>
        /// </param>
        /// <param name="useFullTypeNames"><see langword="true"/> to use an object's fully qualified
        /// name, otherwise just the short name.</param>
        /// <typeparam name="T">The type of the object being dumped.</typeparam>
        /// <returns>The object being dumped.</returns>
        public static T Dump<T>(this T obj, int maxDepth, bool useFullTypeNames)
        {
            if (Crestron.SimplSharp.CrestronEnvironment.ProgramCompatibility == Crestron.SimplSharp.eCrestronSeries.Series3)
            {
                ConsoleBase.WriteLine();
            }

            ConsoleBase.WriteNoHeader(Terminal.Formatting.Formatters.FormatObjectForConsole(obj, maxDepth, useFullTypeNames));
            return obj;
        }

        /// <summary>
        /// Optionally formats a message, depending on whether the args parameter is null or empty.
        /// </summary>
        /// <param name="message">The message to potentially format.</param>
        /// <param name="args">Optional array of args.</param>
        /// <returns>A string with the proper formatting applied.</returns>
        internal static string OptionalFormat(this string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                if (!Options.Instance.EnableMarkup)
                {
                    return string.Format(message, args);
                }

                return string.Format(message, args).ToAnsiFromConsoleMarkup();
            }

            return Options.Instance.EnableMarkup ? message.ToAnsiFromConsoleMarkup() : message;
        }
    }
}