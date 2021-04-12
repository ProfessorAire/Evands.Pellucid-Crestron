#region copyright
// <copyright file="Cell.cs" company="Christopher McNeely">
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

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Holds contents and formatting for table cells.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Regex for filtering out colors from contents, for calculating lengths.
        /// </summary>
        private static readonly Regex ColorFilter;

        /// <summary>
        /// Internal array of strings for each line of the cell when printed.
        /// </summary>
        private string[] lines;

        /// <summary>
        /// The text contents of the cell.
        /// </summary>
        private string contents = string.Empty;

        /// <summary>
        /// The max width the cell is allowed to consume.
        /// </summary>
        private int maxWidth = -1;

        /// <summary>
        /// Backing field for the <see cref="HorizontalAlignment"/> property.
        /// </summary>
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;

        /// <summary>
        /// Initializes static members of the <see cref="Cell"/> class.
        /// </summary>
        static Cell()
        {
            ColorFilter = new Regex("\x1b\\[[0-9;]+m", RegexOptions.Multiline);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        public Cell()
        {
            this.Contents = string.Empty;
            this.Color = ColorFormat.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="contents">The text contents of the cell.</param>
        public Cell(string contents)
            : this(contents, ColorFormat.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="contents">The contents of the cell.</param>
        /// <param name="color">The color of the contents of the cell.</param>
        public Cell(string contents, ColorFormat color)
            : this()
        {
            this.Contents = contents;
            this.Color = color;
        }

        /// <summary>
        /// Gets or sets the text contents of the cell.
        /// </summary>
        public string Contents
        {
            get
            {
                return contents;
            }

            set
            {
                if (contents != value)
                {
                    contents = value;
                    lines = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the text contents of the cell.
        /// </summary>
        public ColorFormat Color { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="HorizontalAlignment"/> of the cell's text contents.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set { horizontalAlignment = value; }
        }

        /// <summary>
        /// Gets the total width of the cell's contents.
        /// </summary>
        /// <returns>A value indicating the total width the cell's contents will consume.</returns>
        public int GetTotalWidth()
        {
            if (string.IsNullOrEmpty(Contents))
            {
                return 0;
            }

            return ColorFilter.Replace(Contents, string.Empty).Length;
        }

        /// <summary>
        /// Gets the number of lines the cell's contents will span.
        /// </summary>
        /// <param name="maxWidth">The maximum width the cell should consume.</param>
        /// <returns>A value indicating the number of lines the cell will span.</returns>
        public int GetNumberOfLines(int maxWidth)
        {
            CalculateLines(maxWidth);
            return lines.Length;
        }

        /// <summary>
        /// Gets the line specified. If there are less lines than the line requested an empty string padded with spaces will be returned.
        /// </summary>
        /// <param name="lineToGet">The zero-based line to retrieve.</param>
        /// <param name="maxWidth">The maximum width the cell should consume.</param>
        /// <param name="withColor">A value indicating whether or not the cell should be printed with color.</param>
        /// <returns>The cell's text contents at the line specified.</returns>
        public string GetLine(int lineToGet, int maxWidth, bool withColor)
        {
            var number = GetNumberOfLines(maxWidth);
            if (lineToGet >= number)
            {
                return string.Empty.PadRight(maxWidth);
            }

            return withColor && Color != null ? Color.FormatText(lines[lineToGet].PadRight(maxWidth)) : lines[lineToGet].PadRight(maxWidth);
        }

        /// <summary>
        /// Calculate the total number of lines.
        /// </summary>
        /// <param name="maxWidth">The max width the cell should consume.</param>
        private void CalculateLines(int maxWidth)
        {
            if (this.maxWidth != maxWidth || lines == null)
            {
                if (maxWidth > 0)
                {
                    var lineBreaker = new Regex("(?:\\b|^|\\w??)(?:(?:.{1,{WIDTH}}(?:\\b|$)|(?:\\w{1,{WIDTH}}))?)(?:\r|\n|\r\n)?".Replace("{WIDTH}", maxWidth.ToString()));
                    var matches = lineBreaker.Matches(Contents);

                    var l = new List<string>();
                    for (var i = 0; i < matches.Count; i++)
                    {
                        if (matches[i].Value.Contains("\r") || matches[i].Value.Contains("\n"))
                        {
                            var val = matches[i].Value.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                            if (!string.IsNullOrEmpty(val))
                            {
                                l.Add(val.Align(HorizontalAlignment, maxWidth));
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(matches[i].Value))
                            {
                                l.Add(matches[i].Value.Trim().Align(HorizontalAlignment, maxWidth));
                            }
                        }
                    }

                    lines = l.ToArray();
                }
                else
                {
                    lines = new string[1] { this.Contents };
                }

                this.maxWidth = maxWidth;
            }
        }
    }
}