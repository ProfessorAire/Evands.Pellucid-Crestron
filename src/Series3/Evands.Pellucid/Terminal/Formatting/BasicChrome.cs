#region copyright
// <copyright file="BasicChrome.cs" company="Christopher McNeely">
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
    /// Implementation of <see cref="IChromeCollection"/> that uses standard ANSII characters.
    /// </summary>
    public class BasicChrome : IChromeCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicChrome"/> class.
        /// </summary>
        public BasicChrome()
        {
            this.HeaderTopLeft = "-";
            this.HeaderTop = "-";
            this.HeaderTopJoin = "-";
            this.HeaderTopRight = "-";
            this.HeaderLeft = "|";
            this.HeaderInteriorVertical = "|";
            this.HeaderRight = "|";
            this.HeaderBodyLeftJoin = "-";
            this.HeaderBodyHorizontal = "-";
            this.HeaderBodyInteriorJoin = "-";
            this.HeaderBodyRightJoin = "-";
            this.HeaderBottomLeft = "-";
            this.HeaderBottom = "-";
            this.HeaderBottomJoin = "-";
            this.HeaderBottomRight = "-";
            this.BodyTopLeft = "-";
            this.BodyTop = "-";
            this.BodyTopJoin = "-";
            this.BodyTopRight = "-";
            this.BodyLeft = "|";
            this.BodyLeftJoin = "|";
            this.BodyInteriorHorizontal = "-";
            this.BodyInteriorVertical = "|";
            this.BodyInteriorJoin = "+";
            this.BodyRightJoin = "|";
            this.BodyBottomLeft = "-";
            this.BodyBottom = "-";
            this.BodyRight = "|";
            this.BodyBottomJoin = "-";
            this.BodyBottomRight = "-";
        }

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
    }
}