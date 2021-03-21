#region copyright
// <copyright file="ColorCode.cs" company="Christopher McNeely">
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

namespace EVS.Pellucid.Terminal
{
    /// <summary>
    /// List of basic valid terminal colors.
    /// </summary>
    public enum ColorCode
    {
        /// <summary>
        /// Indicates the color should be either the default or previously set color.
        /// </summary>
        None = -1000,

        /// <summary>
        /// The terminal's black color.
        /// </summary>
        Black = 0,

        /// <summary>
        /// The terminal's red color.
        /// </summary>
        Red = 1,

        /// <summary>
        /// The terminal's green color.
        /// </summary>
        Green = 2,

        /// <summary>
        /// The terminal's yellow color.
        /// </summary>
        Yellow = 3,

        /// <summary>
        /// The terminal's blue color.
        /// </summary>
        Blue = 4,

        /// <summary>
        /// The terminal's magenta color.
        /// </summary>
        Magenta = 5,

        /// <summary>
        /// The terminal's cyan color.
        /// </summary>
        Cyan = 6,

        /// <summary>
        /// The terminal's light gray or white color.
        /// </summary>
        LightGray = 7,

        /// <summary>
        /// The terminal's dark gray color.
        /// </summary>
        DarkGray = 60,

        /// <summary>
        /// The terminal's bright red color.
        /// </summary>
        BrightRed = 61,

        /// <summary>
        /// The terminal's bright green color.
        /// </summary>
        BrightGreen = 62,

        /// <summary>
        /// The terminal's bright yellow color.
        /// </summary>
        BrightYellow = 63,
        
        /// <summary>
        /// The terminal's bright blue color.
        /// </summary>
        BrightBlue = 64,

        /// <summary>
        /// The terminal's bright magenta color.
        /// </summary>
        BrightMagenta = 65,

        /// <summary>
        /// The terminal's bright cyan color.
        /// </summary>
        BrightCyan = 66,

        /// <summary>
        /// The terminal's bright white color.
        /// </summary>
        White = 67
    }
}