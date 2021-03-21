#region copyright
// <copyright file="LoggerExtensions.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Provides extension methods to simplify logging on objects.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class LoggerExtensions
    {
        /// <summary>
        /// Writes a debug notice to the error log.
        /// </summary>
        /// <param name="obj">The object sending the message.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogNotice(this object obj, string message, params object[] args)
        {
            Logger.LogNotice(obj, message, args);
        }

        /// <summary>
        /// Writes a notice to the error log.
        /// </summary>
        /// <param name="obj">The object sending the message.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogDebug(this object obj, string message, params object[] args)
        {
            Logger.LogDebug(obj, message, args);
        }

        /// <summary>
        /// Writes a warning to the error log.
        /// </summary>
        /// <param name="obj">The object sending the message.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogWarning(this object obj, string message, params object[] args)
        {
            Logger.LogWarning(obj, message, args);
        }

        /// <summary>
        /// Writes an error to the error log.
        /// </summary>
        /// <param name="obj">The object sending the message.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogError(this object obj, string message, params object[] args)
        {
            Logger.LogError(obj, message, args);
        }

        /// <summary>
        /// Writes an exception to the error log (as a formatted error, not an exception).
        /// </summary>
        /// <param name="obj">The object sending the message.</param>
        /// <param name="ex">The exception to write.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogException(this object obj, Exception ex, string message, params object[] args)
        {
            Logger.LogException(obj, ex, message, args);
        }

        /// <summary>
        /// Determines if the specified flag is present in the <see cref="LogLevels"/> enumeration.
        /// </summary>
        /// <param name="levels">The object to check for flags.</param>
        /// <param name="flagToFind">The flag to look for.</param>
        /// <returns>A true or false value indicating the presence of the flag.</returns>
        public static bool Contains(this LogLevels levels, LogLevels flagToFind)
        {
            return (levels | flagToFind) == levels;
        }
    }
}