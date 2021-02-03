#region copyright
// <copyright file="LogLevels.cs" company="Christopher McNeely">
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

namespace Silver.Diagnostics
{
    /// <summary>
    /// Logging level types.
    /// </summary>
    [Flags]
    public enum LogLevels
    {
        /// <summary>
        /// No logging.
        /// </summary>
        None = 1,

        /// <summary>
        /// Notice logging.
        /// </summary>
        Notices = 2,

        /// <summary>
        /// Warning logging.
        /// </summary>
        Warnings = 4,

        /// <summary>
        /// Error logging.
        /// </summary>
        Errors = 8,

        /// <summary>
        /// Exception logging.
        /// </summary>
        Exceptions = 16,

        /// <summary>
        /// Debug logging.
        /// </summary>
        Debug = 32,

        /// <summary>
        /// All logging.
        /// </summary>
        All = 63
    }
}