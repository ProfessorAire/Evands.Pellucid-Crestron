#region copyright
// <copyright file="StandardColor.cs" company="Christopher McNeely">
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

namespace EVS.Pellucid.Terminal.Formatting
{
    /// <summary>
    /// Defines a standard ANSI console color.
    /// </summary>
    public struct StandardColor : IConsoleColor, IEquatable<ColorCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardColor"/> struct.
        /// </summary>
        /// <param name="code">The <see cref="ColorCode"/> to associate with the class.</param>        
        public StandardColor(ColorCode code)
            : this()
        {
            Code = code;
        }

        /// <summary>
        /// Gets the <see cref="ColorCode"/> associated with the color.
        /// </summary>        
        public ColorCode Code { get; private set; }

        /// <summary>
        /// Implicitly converts the specified <see cref="ColorCode"/> into a <see cref="StandardColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="ColorCode"/> to convert.</param>
        /// <returns>A <see cref="StandardColor"/>.</returns>
        public static implicit operator StandardColor(ColorCode color)
        {
            return new StandardColor(color);
        }

        /// <inheritdoc />
        public string AsForeground()
        {
            if (Code != ColorCode.None)
            {
                return string.Format("{0}", (int)Code + 30);
            }

            return string.Empty;
        }

        /// <inheritdoc />
        public string AsBackground()
        {
            if (Code != ColorCode.None)
            {
                return string.Format("{0}", (int)Code + 40);
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks for equality between this and a <see cref="ColorCode"/>.
        /// </summary>
        /// <param name="other">The <see cref="ColorCode"/> to compare equality with.</param>
        /// <returns><see langword="true"/> if the objects are equal, otherwise <see langword="false"/>.</returns>
        public bool Equals(ColorCode other)
        {
            return this.Code == other;
        }
    }
}