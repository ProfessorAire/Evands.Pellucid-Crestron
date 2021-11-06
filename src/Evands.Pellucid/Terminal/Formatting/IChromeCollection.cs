#region copyright
// <copyright file="IChromeCollection.cs" company="Christopher McNeely">
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
    /// Defines the requirements for a class that provides IChromeCollection functionality.
    /// </summary>
    public interface IChromeCollection
    {
        /// <summary>
        /// Gets the top left corner glyph for headers.
        /// </summary>
        string HeaderTopLeft { get; }

        /// <summary>
        /// Gets the top glyph used for headers.
        /// </summary>
        string HeaderTop { get; }

        /// <summary>
        /// Gets the glyph used where a header's top border intersects with a vertical segment.
        /// </summary>
        string HeaderJoinTop { get; }

        /// <summary>
        /// Gets the top right corner glyph for body headers.
        /// </summary>
        string HeaderTopRight { get; }

        /// <summary>
        /// Gets the glyph used for the left border of a header.
        /// </summary>
        string HeaderLeft { get; }

        /// <summary>
        /// Gets the glyph used for the vertical bar that divides cells in a header.
        /// </summary>
        string HeaderInteriorVertical { get; }
        
        /// <summary>
        /// Gets the glyph used for the right border of a header.
        /// </summary>
        string HeaderRight { get; }

        /// <summary>
        /// Gets the glyph used where the bottom left corner of a header joins the top left corner of a body.
        /// </summary>
        string HeaderBodyJoinLeft { get; }

        /// <summary>
        /// Gets the glyph used for the bottom of a header when it is connected to a body.
        /// </summary>
        string HeaderBodyHorizontal { get; }

        /// <summary>
        /// Gets the glyph used where a header's bottom border intersects with a vertical segment for both the header and body.
        /// </summary>
        string HeaderBodyJoinMiddle { get; }

        /// <summary>
        /// Gets the glyph used where the bottom right corner of a header joins the top right corner of a body.
        /// </summary>
        string HeaderBodyJoinRight { get; }

        /// <summary>
        /// Gets the glyph used for the top left corner of a body that has no header.
        /// </summary>
        string BodyTopLeft { get; }

        /// <summary>
        /// Gets the glyph used for the top of a body with no header.
        /// </summary>
        string BodyTop { get; }

        /// <summary>
        /// Gets the glyph used where the top of a body with no header meets a vertical segment for the body.
        /// </summary>
        string BodyJoinTop { get; }

        /// <summary>
        /// Gets the glyph used for the top right corner of a body that has no header.
        /// </summary>
        string BodyTopRight { get; }

        /// <summary>
        /// Gets the glyph used where the left border of a body meets an interior horizontal segment.
        /// </summary>
        string BodyJoinLeft { get; }

        /// <summary>
        /// Gets the glyph used for the left side of a body.
        /// </summary>
        string BodyLeft { get; }

        /// <summary>
        /// Gets the glyph used for horizontal segments in the interior of a body.
        /// </summary>
        string BodyInteriorHorizontal { get; }

        /// <summary>
        /// Gets the glyph used for vertical segments in the interior of a body.
        /// </summary>
        string BodyInteriorVertical { get; }

        /// <summary>
        /// Gets the glyph used where a vertical and horizontal segment join in the interior of a body.
        /// </summary>
        string BodyInteriorJoin { get; }

        /// <summary>
        /// Gets the glyph used where the right border of a body meets an interior horizontal segment.
        /// </summary>
        string BodyJoinRight { get; }

        /// <summary>
        /// Gets the glyph used for the right border of a body.
        /// </summary>
        string BodyRight { get; }

        /// <summary>
        /// Gets the glyph used for the bottom left corner of a body.
        /// </summary>
        string BodyBottomLeft { get; }

        /// <summary>
        /// Gets the glyph used for the bottom border of a body.
        /// </summary>
        string BodyBottom { get; }

        /// <summary>
        /// Gets the glyph used where the bottom border of a body intersects with an interior vertical segment.
        /// </summary>
        string BodyJoinBottom { get; }

        /// <summary>
        /// Gets the glyph used for the bottom right corner of a body.
        /// </summary>
        string BodyBottomRight { get; }
    }
}