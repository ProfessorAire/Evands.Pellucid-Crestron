﻿#region copyright
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
        /// <param name="obj">The object to dump to the console.</param>
        public static void Dump(this object obj)
        {
            ConsoleBase.WriteNoHeader(Terminal.Formatting.Formatters.FormatObjectForConsole(obj));
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
                return string.Format(message, args);
            }

            return message;
        }
    }
}