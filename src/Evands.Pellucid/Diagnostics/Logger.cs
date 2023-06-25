#region copyright
// <copyright file="Logger.cs" company="Christopher McNeely">
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
using Evands.Pellucid.Terminal;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Logger used to log messages to the error log and, when debugging is enabled, to the ConsoleBase.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Holds the loggers registered with the <see cref="Logger"/>.
        /// </summary>
        private static List<ILogWriter> loggers;

        /// <summary>
        /// Initializes static members of the <see cref="Logger"/> class.
        /// </summary>
        static Logger()
        {
            loggers = new List<ILogWriter>();
        }

        /// <summary>
        /// Logs a debug message, if debug logging is allowed.
        /// <para>These messages are logged as notices in any log.</para>
        /// <para>If using the <see cref="CrestronLogWriter"/> it  writes debug messages as notices to the <see cref="ErrorLog"/>.</para>
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The debug message.</param>
        /// <param name="args">Optional array of formatting arguments.</param>        
        public static void LogDebug(object obj, string message, params object[] args)
        {
            Debug.WriteDebugLine(obj, message);

            if (Options.Instance.LogLevels.Contains(LogLevels.Debug))
            {
                if (loggers.Count == 0)
                {
                    loggers.Add(new CrestronLogWriter());
                }

                var msg = string.Format("{0}{1}", Debug.GetMessageHeader(obj, false, false), message.OptionalFormat(args));
                loggers.ForEach(l => l.WriteNotice(msg));
            }           
        }

        /// <summary>
        /// Writes a notice to the error log.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogNotice(object obj, string message, params object[] args)
        {
            Debug.WriteNoticeLine(obj, message, args);

            if (Options.Instance.LogLevels.Contains(LogLevels.Notice))
            {
                if (loggers.Count == 0)
                {
                    loggers.Add(new CrestronLogWriter());
                }

                var msg = string.Format("{0}{1}", Debug.GetMessageHeader(obj), message.OptionalFormat(args));
                loggers.ForEach(l => l.WriteNotice(msg));
            }
        }

        /// <summary>
        /// Logs a message to the error log as a notice, also writing it to the console with the specified color.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="color">The color to write the message to the console using.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogMessage(object obj, Evands.Pellucid.Terminal.Formatting.ColorFormat color, string message, params object[] args)
        {
            LogMessage(obj, LogLevels.Notice, color, message, args);
        }

        /// <summary>
        /// Logs a message to the error log at the specified level, also writing it to the console with the specified color.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="logLevel">The <see cref="LogLevels"/> level to write to the log using.</param>
        /// <param name="color">The color to write the message to the console using.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogMessage(object obj, LogLevels logLevel, Evands.Pellucid.Terminal.Formatting.ColorFormat color, string message, params object[] args)
        {
            Debug.WriteLine(obj, color, message, args);

            if (Options.Instance.LogLevels.Contains(logLevel))
            {
                if (loggers.Count == 0)
                {
                    loggers.Add(new CrestronLogWriter());
                }

                var msg = string.Format("{0}{1}", Debug.GetMessageHeader(obj), message.OptionalFormat(args));
                switch (logLevel)
                {
                    case LogLevels.Notice:
                        loggers.ForEach(l => l.WriteNotice(msg));
                        break;
                    case LogLevels.Warning:
                        loggers.ForEach(l => l.WriteWarning(msg));
                        break;
                    case LogLevels.Error:
                        loggers.ForEach(l => l.WriteError(msg));
                        break;
                    case LogLevels.Debug:
                        loggers.ForEach(l => l.WriteDebug(msg));
                        break;
                    case LogLevels.Exception:
                        loggers.ForEach(l => l.WriteError(msg));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Writes a warning to the error log.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogWarning(object obj, string message, params object[] args)
        {
            Debug.WriteWarningLine(obj, message, args);

            if (Options.Instance.LogLevels.Contains(LogLevels.Warning))
            {
                if (loggers.Count == 0)
                {
                    loggers.Add(new CrestronLogWriter());
                }

                var msg = string.Format("{0}{1}", Debug.GetMessageHeader(obj), message.OptionalFormat(args));
                loggers.ForEach(l => l.WriteWarning(msg));
            }
        }

        /// <summary>
        /// Writes an error to the error log.
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogError(object obj, string message, params object[] args)
        {
            Debug.WriteErrorLine(obj, message, args);

            if (Options.Instance.LogLevels.Contains(LogLevels.Error))
            {
                if (loggers.Count == 0)
                {
                    loggers.Add(new CrestronLogWriter());
                }

                var msg = string.Format("{0}{1}", Debug.GetMessageHeader(obj), message.OptionalFormat(args));
                loggers.ForEach(l => l.WriteError(msg));
            }
        }

        /// <summary>
        /// Writes an exception to the error log (as a formatted error, not an exception).
        /// </summary>
        /// <param name="obj">The object the message originated from.</param>
        /// <param name="ex">The exception to write.</param>
        /// <param name="message">The message to prefix the log entry with.</param>
        /// <param name="args">Optional array of objects to format the message prefix with.</param>
        public static void LogException(object obj, Exception ex, string message, params object[] args)
        {
            Debug.WriteException(obj, ex, message, args);

            if (Options.Instance.LogLevels.Contains(LogLevels.Exception))
            {
                if (loggers.Count == 0)
                {
                    loggers.Add(new CrestronLogWriter());
                }

                var msg = string.Format("{0}{1}", Debug.GetMessageHeader(obj), message.OptionalFormat(args));
                var sb = new StringBuilder(message.Length + (ex.Message.Length * 2));

                sb.AppendLine(msg);

                int exceptionIndex = 0;

                while (ex != null)
                {
                    exceptionIndex++;
                    sb.AppendFormat("--------Exception {0}--------\x0d\x0a", exceptionIndex);
                    sb.AppendLine(ex.ToString());
                    sb.AppendFormat("-----------------------------\x0d\x0a");
                    sb.AppendLine();

                    ex = ex.InnerException;
                }

                loggers.ForEach(l => l.WriteError(sb.ToString()));
            }
        }

        /// <summary>
        /// Registers a <see cref="ILogWriter"/> to write messages to.
        /// </summary>
        /// <param name="writer">The <see cref="ILogWriter"/> to register.</param>
        public static void RegisterLogWriter(ILogWriter writer)
        {
            if (!loggers.Contains(writer))
            {
                loggers.Add(writer);
            }
        }

        /// <summary>
        /// Unregisters a <see cref="ILogWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="ILogWriter"/> to unregister.</param>
        public static void UnregisterLogWriter(ILogWriter writer)
        {
            loggers.Remove(writer);
        }

        /// <summary>
        /// Gets the <see cref="ILogWriter"/>s registered.
        /// </summary>
        /// <returns>A collection of <see cref="ILogWriter"/> objects registered.</returns>
        public static ReadOnlyCollection<ILogWriter> GetRegisteredLogWriters()
        {
            return loggers.AsReadOnly();
        }
    }
}