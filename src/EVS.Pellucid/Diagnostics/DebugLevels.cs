#region copyright
// <copyright file="DebugLevels.cs" company="Christopher McNeely">
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

namespace EVS.Pellucid.Diagnostics
{
    /// <summary>
    /// The various levels of debug messaging available.
    /// </summary>
    [Flags]
    public enum DebugLevels
    {
        /// <summary>
        /// If included indicates no debug levels are enabled.
        /// </summary>
        None = 1,

        /// <summary>
        /// If included indicates debug messages are enabled.
        /// </summary>
        Debug = 2,

        /// <summary>
        /// If included indicates progress messages are enabled.
        /// </summary>
        Progress = 4,

        /// <summary>
        /// If included indicates success messages are enabled.
        /// </summary>
        Success = 8,

        /// <summary>
        /// If included indicates error messages are enabled.
        /// </summary>
        Error = 16,

        /// <summary>
        /// If included indicates notice messages are enabled.
        /// </summary>
        Notice = 32,

        /// <summary>
        /// If included indicates exception messages are enabled.
        /// </summary>
        Exception = 64,

        /// <summary>
        /// If included indicates uncategorized messages are enabled.
        /// </summary>
        Uncategorized = 128,

        /// <summary>
        /// If included indicates warning messages are enabled.
        /// </summary>
        Warning = 256,

        /// <summary>
        /// If included indicates all messages are enabled.
        /// </summary>
        All = 510,

        /// <summary>
        /// If included indicates all messages are enabled, except debug messages.
        /// </summary>
        AllButDebug = 508
    }

    /// <summary>
    /// Provides extension methods for easily checking the <see cref="DebugLevels"/> enumeration for flags.
    /// </summary>
    public static class DebugLevelsExtensions
    {
        /// <summary>
        /// Determines if the specified flag is present in the <see cref="DebugLevels"/> enumeration.
        /// </summary>
        /// <param name="levels">The object to check for flags.</param>
        /// <param name="flagToFind">The flag to look for.</param>
        /// <returns>A true or false value indicating the presence of the flag.</returns>
        public static bool Contains(this DebugLevels levels, DebugLevels flagToFind)
        {
            return (levels | flagToFind) == levels;
        }
    }
}