﻿#region copyright
// <copyright file="RoundedChrome.cs" company="Christopher McNeely">
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

using System.Text;

namespace Evands.Pellucid.Terminal.Formatting
{
    /// <summary>
    /// Implementation of <see cref="IChromeCollection"/> that uses rounded glyphs.
    /// <para>When using the option for thick headers the top corners of headers will be square, as there are no rounded thick glyphs available.</para>
    /// </summary>
    public class RoundedChrome : IChromeCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedChrome"/> class using thin header glyphs.
        /// </summary>
        public RoundedChrome()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedChrome"/> class.
        /// </summary>
        /// <param name="thickHeaders">If <see langword="true"/> will use thick (square) header glyphs.</param>
        public RoundedChrome(bool thickHeaders)
        {
            var enc = Encoding.GetEncoding("ISO-8859-1");
            this.ThickHeaders = thickHeaders;
            
            if (thickHeaders)
            {
                this.HeaderTopLeft = this.GetValue('┏', enc);
                this.HeaderTop = this.GetValue('━', enc);
                this.HeaderTopJoin = this.GetValue('┯', enc);
                this.HeaderTopRight = this.GetValue('┓', enc);
                this.HeaderLeft = this.GetValue('┃', enc);
                this.HeaderRight = this.GetValue('┃', enc);
                this.HeaderBodyLeftJoin = this.GetValue('┡', enc);
                this.HeaderBodyHorizontal = this.GetValue('━', enc);
                this.HeaderBodyInteriorJoin = this.GetValue('┿', enc);
                this.HeaderBodyRightJoin = this.GetValue('┩', enc);
                this.HeaderBottomLeft = this.GetValue('┗', enc);
                this.HeaderBottom = this.GetValue('━', enc);
                this.HeaderBottomJoin = this.GetValue('┷', enc);
                this.HeaderBottomRight = this.GetValue('┛', enc);
            }
            else
            {
                this.HeaderTopLeft = this.GetValue('╭', enc);
                this.HeaderTop = this.GetValue('─', enc);
                this.HeaderTopJoin = this.GetValue('┬', enc);
                this.HeaderTopRight = this.GetValue('╮', enc);
                this.HeaderLeft = this.GetValue('│', enc);
                this.HeaderRight = this.GetValue('│', enc);
                this.HeaderBodyLeftJoin = this.GetValue('├', enc);
                this.HeaderBodyHorizontal = this.GetValue('─', enc);
                this.HeaderBodyInteriorJoin = this.GetValue('┼', enc);
                this.HeaderBodyRightJoin = this.GetValue('┤', enc);
                this.HeaderBottomLeft = this.GetValue('╰', enc);
                this.HeaderBottom = this.GetValue('─', enc);
                this.HeaderBottomJoin = this.GetValue('┴', enc);
                this.HeaderBottomRight = this.GetValue('╯', enc);
            }

            this.HeaderInteriorVertical = this.GetValue('│', enc);
            this.BodyTopLeft = this.GetValue('╭', enc);
            this.BodyTop = this.GetValue('─', enc);
            this.BodyTopJoin = this.GetValue('┬', enc);
            this.BodyTopRight = this.GetValue('╮', enc);
            this.BodyLeft = this.GetValue('│', enc);
            this.BodyLeftJoin = this.GetValue('├', enc);
            this.BodyInteriorHorizontal = this.GetValue('─', enc);
            this.BodyInteriorVertical = this.GetValue('│', enc);
            this.BodyInteriorJoin = this.GetValue('┼', enc);
            this.BodyRightJoin = this.GetValue('┤', enc);
            this.BodyBottomLeft = this.GetValue('╰', enc);
            this.BodyBottom = this.GetValue('─', enc);
            this.BodyRight = this.GetValue('│', enc);
            this.BodyBottomJoin = this.GetValue('┴', enc);
            this.BodyBottomRight = this.GetValue('╯', enc);
        }

        /// <summary>
        /// Gets a value indicating whether this will use thick headers.
        /// </summary>
        public bool ThickHeaders { get; private set; }

        /// <summary>
        /// Gets the top left corner glyph for headers.
        /// </summary>
        public string HeaderTopLeft { get; private set; }

        /// <summary>
        /// Gets the top glyph used for headers.
        /// </summary>
        public string HeaderTop { get; private set; }

        /// <summary>
        /// Gets the glyph used where a header's top border intersects with a vertical segment.
        /// </summary>
        public string HeaderTopJoin { get; private set; }

        /// <summary>
        /// Gets the top right corner glyph for body headers.
        /// </summary>
        public string HeaderTopRight { get; private set; }

        /// <summary>
        /// Gets the glyph used for the left border of a header.
        /// </summary>
        public string HeaderLeft { get; private set; }

        /// <summary>
        /// Gets the glyph used for the vertical bar that divides cells in a header.
        /// </summary>
        public string HeaderInteriorVertical { get; private set; }

        /// <summary>
        /// Gets the glyph used for the right border of a header.
        /// </summary>
        public string HeaderRight { get; private set; }

        /// <summary>
        /// Gets the glyph used where the bottom left corner of a header joins the top left corner of a body.
        /// </summary>
        public string HeaderBodyLeftJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used for the bottom of a header when it is connected to a body.
        /// </summary>
        public string HeaderBodyHorizontal { get; private set; }

        /// <summary>
        /// Gets the glyph used where a header's bottom border intersects with a vertical segment for both the header and body.
        /// </summary>
        public string HeaderBodyInteriorJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used where the bottom right corner of a header joins the top right corner of a body.
        /// </summary>
        public string HeaderBodyRightJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used for the bottom left corner of a header with no body.
        /// </summary>
        public string HeaderBottomLeft { get; private set; }

        /// <summary>
        /// Gets the glyph used for the bottom of a header with no body.
        /// </summary>
        public string HeaderBottom { get; private set; }

        /// <summary>
        /// Gets the glyph used where a header with no body intersects with a vertical segment for the header.
        /// </summary>
        public string HeaderBottomJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used for the bottom right corner of a header with no body.
        /// </summary>
        public string HeaderBottomRight { get; private set; }

        /// <summary>
        /// Gets the glyph used for the top left corner of a body that has no header.
        /// </summary>
        public string BodyTopLeft { get; private set; }

        /// <summary>
        /// Gets the glyph used for the top of a body with no header.
        /// </summary>
        public string BodyTop { get; private set; }

        /// <summary>
        /// Gets the glyph used where the top of a body with no header meets a vertical segment for the body.
        /// </summary>
        public string BodyTopJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used for the top right corner of a body that has no header.
        /// </summary>
        public string BodyTopRight { get; private set; }

        /// <summary>
        /// Gets the glyph used where the left border of a body meets an interior horizontal segment.
        /// </summary>
        public string BodyLeftJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used for the left side of a body.
        /// </summary>
        public string BodyLeft { get; private set; }

        /// <summary>
        /// Gets the glyph used for horizontal segments in the interior of a body.
        /// </summary>
        public string BodyInteriorHorizontal { get; private set; }

        /// <summary>
        /// Gets the glyph used for vertical segments in the interior of a body.
        /// </summary>
        public string BodyInteriorVertical { get; private set; }

        /// <summary>
        /// Gets the glyph used where a vertical and horizontal segment join in the interior of a body.
        /// </summary>
        public string BodyInteriorJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used where the left border of a body meets an interior horizontal segment.
        /// </summary>
        public string BodyRightJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used for the right border of a body.
        /// </summary>
        public string BodyRight { get; private set; }

        /// <summary>
        /// Gets the glyph used for the bottom left corner of a body.
        /// </summary>
        public string BodyBottomLeft { get; private set; }

        /// <summary>
        /// Gets the glyph used for the bottom border of a body.
        /// </summary>
        public string BodyBottom { get; private set; }

        /// <summary>
        /// Gets the glyph used where the bottom border of a body intersects with an interior vertical segment.
        /// </summary>
        public string BodyBottomJoin { get; private set; }

        /// <summary>
        /// Gets the glyph used for the bottom right corner of a body.
        /// </summary>
        public string BodyBottomRight { get; private set; }

        /// <summary>
        /// Used to return the properly encoded glyph values.
        /// </summary>
        /// <param name="character">The character to encode.</param>
        /// <param name="iso88591">The ISO-8859-1 encoding.</param>
        /// <returns>A character encoded as a UTF8 byte sequence.</returns>
        private string GetValue(char character, Encoding iso88591)
        {
            var bytes = Encoding.UTF8.GetBytes(new char[] { character });
            return iso88591.GetString(bytes, 0, bytes.Length);
        }
    }
}