#region copyright
// <copyright file="CrestronLogWriter.cs" company="Christopher McNeely">
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

using Crestron.SimplSharp;

namespace EVS.Pellucid.Diagnostics
{
    /// <summary>
    /// A <see cref="ILogWriter"/> that writes to the Crestron <see cref="ErrorLog"/>.
    /// </summary>
    public class CrestronLogWriter : ILogWriter
    {
        /// <summary>
        /// Writes a notice to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteDebug(string message)
        {
            ErrorLog.Notice(message);
        }

        /// <summary>
        /// Writes a notice to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteNotice(string message)
        {
            ErrorLog.Notice(message);
        }

        /// <summary>
        /// Writes a warning to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteWarning(string message)
        {
            ErrorLog.Warn(message);
        }

        /// <summary>
        /// Writes an error to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteError(string message)
        {
            ErrorLog.Error(message);
        }
    }
}