#region copyright
// <copyright file="Extensions.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Extension methods used by classes related to <see cref="Table"/> objects.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Pads a string on both the left and right, with additional uneven rootPadding being added on the right of the provided text.
        /// </summary>
        /// <param name="value">The value to pad.</param>
        /// <param name="totalWidth">The total width to pad to.</param>
        /// <returns>A <see langword="string"/> padded to the specified total width.</returns>
        public static string Pad(this string value, int totalWidth)
        {
            if (value.Length < totalWidth)
            {
                double remaining = totalWidth - value.Length;
                var left = (int)Math.Floor(remaining / 2);
                return value.PadLeft(value.Length + left).PadRight(totalWidth);
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Pads a string as needed, based on the specified <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="value">The value to pad.</param>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to pad using.</param>
        /// <param name="totalWidth">The total width to pad to.</param>
        /// <returns>A string padded with spaces to provide the specified <see cref="HorizontalAlignment"/>.</returns>
        public static string Align(this string value, HorizontalAlignment alignment, int totalWidth)
        {
            if (value.Length < totalWidth)
            {
                switch (alignment)
                {
                    case HorizontalAlignment.Left:
                        return value.PadRight(totalWidth);
                    case HorizontalAlignment.Center:
                        return value.Pad(totalWidth);
                    case HorizontalAlignment.Right:
                        return value.PadLeft(totalWidth);
                }
            }

            return value;
        }
    }
}