#region copyright
// <copyright file="SimpleFileLogger.cs" company="Christopher McNeely">
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
using System.Text;
using Crestron.SimplSharp.CrestronIO;
using EVS.Pellucid.Diagnostics;

namespace EVS.Pellucid.Diagnostics
{
    /// <summary>
    /// Provides simple methods for logging to a file.
    /// </summary>
    public class SimpleFileLogger : ILogWriter
    {
        /// <summary>
        /// Tracks the number of messages logged.
        /// </summary>
        private long numberOfMessages = 0;

        /// <summary>
        /// The file name the logger writes to.
        /// </summary>
        private string fileName;

        /// <summary>
        /// The base name of the file.
        /// </summary>
        private string baseFileName;

        /// <summary>
        /// The extension of the file.
        /// </summary>
        private string fileExtension;

        /// <summary>
        /// The base directory of the file.
        /// </summary>
        private string baseDirectory;

        /// <summary>
        /// The date of the current file stamp.
        /// </summary>
        private DateTime fileStamp;

        /// <summary>
        /// The string builder used for only flushing the log when needed.
        /// </summary>
        private StringBuilder logBuilder;

        /// <summary>
        /// The time the log was last flushed.
        /// </summary>
        private DateTime lastFlushStamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleFileLogger"/> class. The 
        /// </summary>
        /// <param name="fileName">The file to write to, as a complete path.</param>
        public SimpleFileLogger(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            var fi = new FileInfo(fileName);
            baseFileName = Path.GetFileNameWithoutExtension(fileName);
            baseDirectory = fi.DirectoryName;
            fileExtension = fi.Extension;
            FormatFileName();
            logBuilder = new StringBuilder(5000);
            LoggingFrequency = TimeSpan.FromMinutes(10);

            Crestron.SimplSharp.CrestronEnvironment.ProgramStatusEventHandler += (args) =>
                {
                    if (args == Crestron.SimplSharp.eProgramStatusEventType.Stopping || args == Crestron.SimplSharp.eProgramStatusEventType.Paused)
                    {
                        Flush(true);
                    }
                };
        }

        /// <summary>
        /// Gets or sets the frequency with which to write the log to disk.
        /// </summary>
        public TimeSpan LoggingFrequency { get; set; }

        /// <inheritdoc/>
        public void WriteDebug(string message)
        {
            LogMessage(string.Format("Debug: {0}", message));
        }

        /// <inheritdoc/>
        public void WriteNotice(string message)
        {
            LogMessage(string.Format("Notice: {0}", message));
        }

        /// <inheritdoc/>
        public void WriteWarning(string message)
        {
            LogMessage(string.Format("Warning: {0}", message));
        }

        /// <inheritdoc/>
        public void WriteError(string message)
        {
            LogMessage(string.Format("Error: {0}", message));
        }

        /// <summary>
        /// Formats the file name if needed.
        /// </summary>
        private void FormatFileName()
        {
            var now = Crestron.SimplSharp.CrestronEnvironment.GetLocalTime();

            if (fileStamp == null || fileStamp.ToShortDateString() != now.ToShortDateString())
            {
                fileStamp = now;
                fileName = Path.Combine(baseDirectory, string.Format("{0}_{1}{2}{3}{4}", baseFileName, fileStamp.Year, fileStamp.Month, fileStamp.Day, fileExtension));
            }
        }

        /// <summary>
        /// Appends a message to the specified file.
        /// </summary>
        /// <param name="data">The <see langword="string"/> data to append.</param>
        private void LogMessage(string data)
        {
            logBuilder.AppendLine(data);
            Flush(false);
        }

        /// <summary>
        /// Flushes the log if ready, writing all pending values to disk.
        /// </summary>
        /// <param name="force">When true will flush to disk regardless of whether the log requires it.</param>
        private void Flush(bool force)
        {
            if (logBuilder.Length > 0 && (force || Crestron.SimplSharp.CrestronEnvironment.GetLocalTime() - lastFlushStamp >= LoggingFrequency))
            {
                try
                {
                    FormatFileName();

                    if (!File.Exists(fileName))
                    {
                        if (!Directory.Exists(baseDirectory))
                        {
                            Directory.CreateDirectory(baseDirectory);
                        }

                        using (var fs = File.CreateText(fileName))
                        {
                            fs.Write(string.Format("1: {0}\r\n", logBuilder.ToString()));
                        }

                        numberOfMessages = 1;
                    }
                    else
                    {
                        if (numberOfMessages == 0)
                        {
                            var msg = File.ReadToEnd(fileName, Encoding.UTF8);
                            var m = System.Text.RegularExpressions.Regex.Matches(msg, @"\d+: ", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Multiline);
                            numberOfMessages = m.Count;
                        }

                        using (var fs = File.AppendText(fileName))
                        {
                            fs.Write(string.Format("{0}: {1}\r\n", numberOfMessages++, logBuilder.ToString()));
                        }
                    }

                    logBuilder.Length = 0;
                    lastFlushStamp = Crestron.SimplSharp.CrestronEnvironment.GetLocalTime();
                }
                catch (InvalidDirectoryLocationException ex)
                {
                    Logger.UnregisterLogWriter(this);
                    Logger.LogException(this, ex, "The specifed directory appears to be invalid. Unable to flush the log to the file '{1}'. The SimpleFileLogger has been unregistered from the Logger.", fileName);
                }
                catch (FileNotFoundException ex)
                {
                    Logger.UnregisterLogWriter(this);
                    Logger.LogException(this, ex, "The specifed file could not be created. Unable to flush the log to the file '{1}'. The SimpleFileLogger has been unregistered from the Logger.", fileName);
                }
                catch (Exception ex)
                {
                    Logger.UnregisterLogWriter(this);
                    Logger.LogException(this, ex, "Unable to log messages. The SimplFileLogger has been unregistered from the Logger.");
                }
            }
        }
    }
}