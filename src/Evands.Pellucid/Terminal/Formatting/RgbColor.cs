#region copyright
// <copyright file="RgbColor.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Formatting
{
    /// <summary>
    /// Defines an RGB console color.
    /// </summary>
    public struct RgbColor : IConsoleColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RgbColor"/> struct.
        /// </summary>
        /// <param name="red">The value of the red <see langword="byte"/>.</param>
        /// <param name="green">The value of the green <see langword="byte"/>.</param>
        /// <param name="blue">The value of the blue <see langword="byte"/>.</param>        
        public RgbColor(byte red, byte green, byte blue)
            : this()
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        /// Gets or sets the value of the Red <see langword="byte"/>.
        /// </summary>        
        public byte Red { get; set; }

        /// <summary>
        /// Gets or sets the value of the Green <see langword="byte"/>.
        /// </summary>        
        public byte Green { get; set; }

        /// <summary>
        /// Gets or sets the value of the Blue <see langword="byte"/>.
        /// </summary>        
        public byte Blue { get; set; }

        /// <inheritdoc />
        public string AsForeground()
        {
            return string.Format("38;2;{0};{1};{2}", Red, Green, Blue);
        }

        /// <inheritdoc />
        public string AsBackground()
        {
            return string.Format("48;2;{0};{1};{2}", Red, Green, Blue);
        }
    }
}