#region copyright
// <copyright file="ILogWriter.cs" company="Christopher McNeely">
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

namespace EVS.Pellucid.Diagnostics
{
    /// <summary>
    /// Defines the requirements for an object that allows writing directly to a log.
    /// <para>Typically this will be the default <see cref="CrestronLogWriter"/>, but additional loggers can be created for
    /// logging to a remote computer, or for VC-4 logging.</para>
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Writes a debug message to the corresponding log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void WriteDebug(string message);

        /// <summary>
        /// Writes a notice to the corresponding log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void WriteNotice(string message);

        /// <summary>
        /// Writes a warning to the corresponding log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void WriteWarning(string message);

        /// <summary>
        /// Writes an error to the corresponding log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void WriteError(string message);
    }
}