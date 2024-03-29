﻿#region copyright
// <copyright file="Table.cs" company="Christopher McNeely">
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
using System.Linq;
using System.Text;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Assists with printing neatly formatted tables.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// The row used for headers.
        /// </summary>
        private Row headers = new Row();

        /// <summary>
        /// The columns in the table.
        /// </summary>
        private ColumnCollection columns;

        /// <summary>
        /// The rows in the table.
        /// </summary>
        private RowCollection rows = new RowCollection();

        /// <summary>
        /// Backing field for the <see cref="HorizontalBorder"/> property.
        /// </summary>
        private char horizontalBorder = '-';

        /// <summary>
        /// Backing field for the <see cref="VerticalBorder"/> property.
        /// </summary>
        private char verticalBorder = '|';

        /// <summary>
        /// Backing field for the <see cref="HeaderBottomBorder"/> property.
        /// </summary>
        private char headerBottomBorder = '-';

        /// <summary>
        /// The line ending style to use when creating the table.
        /// </summary>
        private string lineEnding = "\r\n";

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        public Table()
        {
            MinimumColumnWidth = 0;
            columns = new ColumnCollection(rows);
        }

        /// <summary>
        /// Gets the number of rows in the table.
        /// </summary>
        public int NumberOfRows
        {
            get
            {
                return this.rows.Count;
            }
        }

        /// <summary>
        /// Gets the number of columns in the table.
        /// </summary>
        public int NumberOfColumns
        {
            get
            {
                return Math.Max(rows.Count > 0 ? rows.Max(r => r.Count) : 0, headers.Count);
            }
        }

        /// <summary>
        /// Gets the collection of rows in the table.
        /// </summary>
        public RowCollection Rows
        {
            get { return this.rows; }
        }

        /// <summary>
        /// Gets the collection of columns in the table.
        /// </summary>
        public ColumnCollection Columns
        {
            get { return this.columns; }
        }

        /// <summary>
        /// Gets the headers row.
        /// </summary>
        public Row Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Gets or sets the minimum width of the printed columns.
        /// </summary>
        public int MinimumColumnWidth { get; set; }

        /// <summary>
        /// Gets or sets the character used for the horizontal borders.
        /// </summary>
        public char HorizontalBorder
        {
            get { return horizontalBorder; }
            set { horizontalBorder = value; }
        }

        /// <summary>
        /// Gets or sets the character used for the vertical borders.
        /// </summary>
        public char VerticalBorder
        {
            get { return verticalBorder; }
            set { verticalBorder = value; }
        }

        /// <summary>
        /// Gets or sets the character used for the border at the bottom of the header cells.
        /// </summary>
        public char HeaderBottomBorder
        {
            get { return headerBottomBorder; }
            set { headerBottomBorder = value; }
        }

        /// <summary>
        /// Creates an empty table.
        /// </summary>
        /// <returns>A <see cref="Table"/>.</returns>
        public static Table Create()
        {
            return new Table();
        }

        /// <summary>
        /// Sets the minimum column width.
        /// </summary>
        /// <param name="minimumWidth">The minimum width.</param>
        /// <returns>This table.</returns>
        public Table SetMinimumColumnWidth(int minimumWidth)
        {
            MinimumColumnWidth = minimumWidth > 0 ? minimumWidth : 0;
            return this;
        }

        /// <summary>
        /// Clears the headers and replaces them with the provided values.
        /// </summary>
        /// <param name="headers">Array of strings to use as header values.</param>
        /// <returns>This table.</returns>
        public Table WithHeaders(params string[] headers)
        {
            var cells = new Cell[headers.Length];
            for (var i = 0; i < headers.Length; i++)
            {
                cells[i] = new Cell() { Contents = headers[i] };
            }

            return this.WithHeaders(cells);
        }

        /// <summary>
        /// Clears the headers and replaces them with the provided values.
        /// </summary>
        /// <param name="headers">Array of cells to use as headers.</param>
        /// <returns>This table.</returns>
        public Table WithHeaders(Cell[] headers)
        {
            this.headers.Clear();
            this.headers.AddCells(headers);

            for (var i = this.headers.Count; i < this.NumberOfColumns; i++)
            {
                this.Headers.AddCell(new Cell());
            }

            return this;
        }

        /// <summary>
        /// Adds a new header.
        /// </summary>
        /// <param name="header">The string value for the header.</param>
        /// <returns>This table.</returns>
        public Table AddHeader(string header)
        {
            return this.AddHeader(new Cell() { Contents = header });
        }

        /// <summary>
        /// Adds a new header.
        /// </summary>
        /// <param name="cell">The cell to use as the header.</param>
        /// <returns>This table.</returns>
        public Table AddHeader(Cell cell)
        {
            this.headers.AddCell(cell);
            return this;
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <param name="contents">Array of strings to use for the row values.</param>
        /// <returns>This table.</returns>
        public Table AddRow(params string[] contents)
        {
            var c = new Cell[contents.Length];
            for (var i = 0; i < contents.Length; i++)
            {
                c[i] = new Cell() { Contents = contents[i] };
            }

            return AddRow(c);
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <param name="cells">Array of cells to use for the row.</param>
        /// <returns>This table.</returns>
        public Table AddRow(params Cell[] cells)
        {
            this.rows.Add(new Row().AddCells(cells));
            while (this.headers.Count < cells.Length)
            {
                this.headers.AddCell(new Cell());
            }

            return this;
        }

        /// <summary>
        /// Adds a new column.
        /// </summary>
        /// <param name="contents">Array of strings to use for the column values.</param>
        /// <returns>The table.</returns>
        public Table AddColumn(params string[] contents)
        {
            return AddColumnWithHeader(string.Empty, contents);
        }

        /// <summary>
        /// Adds a new column.
        /// </summary>
        /// <param name="cells">Array of cells to use for the column</param>
        /// <returns>This table.</returns>
        public Table AddColumn(params Cell[] cells)
        {
            return AddColumnWithHeader(string.Empty, cells);
        }

        /// <summary>
        /// Adds a new column with the specified header.
        /// </summary>
        /// <param name="header">The contents of the header for the new column.</param>
        /// <param name="cells">Array of cells to use for the column.</param>
        /// <returns>This table.</returns>
        public Table AddColumnWithHeader(string header, params Cell[] cells)
        {
            return AddColumnWithHeader(new Cell() { Contents = header }, cells);
        }

        /// <summary>
        /// Adds a new column with the specified header.
        /// </summary>
        /// <param name="header">The cell to use for the header.</param>
        /// <param name="contents">Array of strings to use for the column contents.</param>
        /// <returns>This table.</returns>
        public Table AddColumnWithHeader(Cell header, params string[] contents)
        {
            var cells = new Cell[contents.Length];
            for (var i = 0; i < contents.Length; i++)
            {
                cells[i] = new Cell() { Contents = contents[i] };
            }

            return AddColumnWithHeader(header, cells);
        }

        /// <summary>
        /// Adds a new column with the specified header.
        /// </summary>
        /// <param name="header">The string value to use for the contents of the header.</param>
        /// <param name="contents">Array of strings to use for the column contents.</param>
        /// <returns>This table.</returns>
        public Table AddColumnWithHeader(string header, params string[] contents)
        {
            var cells = new Cell[contents.Length];
            for (var i = 0; i < contents.Length; i++)
            {
                cells[i] = new Cell() { Contents = contents[i] };
            }

            return AddColumnWithHeader(new Cell() { Contents = header }, cells);
        }

        /// <summary>
        /// Adds a new column with the specified header.
        /// </summary>
        /// <param name="header">The cell to use for the header.</param>
        /// <param name="cells">Array of cells to use for the column.</param>
        /// <returns>This table.</returns>
        public Table AddColumnWithHeader(Cell header, params Cell[] cells)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (cells == null)
            {
                throw new ArgumentNullException("cells");
            }

            var numCols = 0;

            if (this.headers.Count == this.NumberOfColumns)
            {
                this.AddHeader(header);
            }
            else
            {
                numCols = this.NumberOfColumns;
                var i = this.headers.Count;
                while (i < numCols)
                {
                    this.headers.AddCell(new Cell());
                    i++;
                }

                this.headers.AddCell(header);
            }

            for (var i = 0; i < cells.Length; i++)
            {
                if (rows.Count <= i)
                {
                    rows.Add(new Row(numCols));
                }

                rows[i].AddCell(cells[i]);
            }

            return this;
        }

        /// <summary>
        /// Formats all of the headers, assigning the specified color and horizontal alignment.
        /// </summary>
        /// <param name="color">The <see cref="ColorFormat"/> to assign to the headers.</param>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to assign to the headers.</param>
        /// <returns>This table.</returns>
        public Table FormatHeaders(ColorFormat color, HorizontalAlignment alignment)
        {
            foreach (var c in headers)
            {
                c.Color = color;
                c.HorizontalAlignment = alignment;
            }

            return this;
        }

        /// <summary>
        /// Formats all of the headers, assigning the horizontal alignment.
        /// </summary>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to assign to the headers.</param>
        /// <returns>This table.</returns>
        public Table FormatHeaders(HorizontalAlignment alignment)
        {
            foreach (var c in headers)
            {
                c.HorizontalAlignment = alignment;
            }

            return this;
        }

        /// <summary>
        /// Formats all of the headers, assigning the specified color.
        /// </summary>
        /// <param name="color">The <see cref="ColorFormat"/> to assign to the headers.</param>
        /// <returns>This table.</returns>
        public Table FormatHeaders(ColorFormat color)
        {
            foreach (var c in headers)
            {
                c.Color = color;
            }

            return this;
        }

        /// <summary>
        /// Formats a column, assigning all the cells in it the specified horizontal alignment.
        /// </summary>
        /// <param name="columnIndex">The index of the column to format.</param>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to assign to the column.</param>
        /// <returns>This table.</returns>
        public Table FormatColumn(int columnIndex, HorizontalAlignment alignment)
        {
            if (this.NumberOfColumns > columnIndex)
            {
                foreach (var r in rows)
                {
                    r[columnIndex].HorizontalAlignment = alignment;
                }
            }

            return this;
        }

        /// <summary>
        /// Formats a column, assigning all the cells in it the specified horizontal alignment.
        /// </summary>
        /// <param name="columnIndex">The index of the column to format.</param>
        /// <param name="color">The <see cref="ColorFormat"/> to assign to the column.</param>
        /// <returns>This table.</returns>
        public Table FormatColumn(int columnIndex, ColorFormat color)
        {
            if (this.NumberOfColumns > columnIndex)
            {
                foreach (var r in rows)
                {
                    r[columnIndex].Color = color;
                }
            }

            return this;
        }

        /// <summary>
        /// Formats a column, assigning all the cells in it the specified color and horizontal alignment.
        /// </summary>
        /// <param name="columnIndex">The index of the column to format.</param>
        /// <param name="color">The <see cref="ColorFormat"/> to assign to the column.</param>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to assign to the column.</param>
        /// <returns>This table.</returns>
        public Table FormatColumn(int columnIndex, ColorFormat color, HorizontalAlignment alignment)
        {
            if (this.NumberOfColumns > columnIndex)
            {
                foreach (var r in rows)
                {
                    r[columnIndex].Color = color;
                    r[columnIndex].HorizontalAlignment = alignment;
                }
            }

            return this;
        }

        /// <summary>
        /// Formats a row, assigning all the cells in it the specified horizontal alignment.
        /// </summary>
        /// <param name="rowIndex">The index of the row to format.</param>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to assign to the row.</param>
        /// <returns>This table.</returns>
        public Table FormatRow(int rowIndex, HorizontalAlignment alignment)
        {
            if (this.NumberOfRows > rowIndex)
            {
                foreach (var c in rows[rowIndex])
                {
                    c.HorizontalAlignment = alignment;
                }
            }

            return this;
        }

        /// <summary>
        /// Formats a row, assigning all the cells in it the specified color.
        /// </summary>
        /// <param name="rowIndex">The index of the row to format.</param>
        /// <param name="color">The <see cref="ColorFormat"/> to assign to the row.</param>
        /// <returns>This table.</returns>
        public Table FormatRow(int rowIndex, ColorFormat color)
        {
            if (this.NumberOfRows > rowIndex)
            {
                foreach (var c in rows[rowIndex])
                {
                    c.Color = color;
                }
            }

            return this;
        }

        /// <summary>
        /// Formats a row, assigning all the cells in it the specified color and horizontal alignment.
        /// </summary>
        /// <param name="rowIndex">The index of the row to format.</param>
        /// <param name="color">The <see cref="ColorFormat"/> to assign to the row.</param>
        /// <param name="alignment">The <see cref="HorizontalAlignment"/> to assign to the row.</param>
        /// <returns>This table.</returns>
        public Table FormatRow(int rowIndex, ColorFormat color, HorizontalAlignment alignment)
        {
            if (this.NumberOfRows > rowIndex)
            {
                foreach (var c in rows[rowIndex])
                {
                    c.Color = color;
                    c.HorizontalAlignment = alignment;
                }
            }

            return this;
        }

        /// <summary>
        /// Perform the specified action for each cell in the specified column.
        /// <para>This makes it easy to do something like assign colors to cells based on the value of the contents.</para>
        /// </summary>
        /// <param name="columnIndex">The index of the column.</param>
        /// <param name="operation">The Action to perform.</param>
        /// <returns>This table.</returns>
        public Table ForEachCellInColumn(int columnIndex, Action<Cell> operation)
        {
            if (NumberOfColumns > columnIndex && operation != null)
            {
                foreach (var r in rows)
                {
                    operation(r[columnIndex]);
                }
            }

            return this;
        }

        /// <summary>
        /// Perform the specified action for each cell in the specified row.
        /// <para>This makes it easy to do something like assign colors to cells based on the value of the contents.</para>
        /// </summary>
        /// <param name="rowIndex">The index of the column.</param>
        /// <param name="operation">The Action to perform.</param>
        /// <returns>This table.</returns>
        public Table ForEachCellInRow(int rowIndex, Action<Cell> operation)
        {
            if (NumberOfRows > rowIndex && operation != null)
            {
                foreach (var r in rows[rowIndex])
                {
                    operation(r);
                }
            }

            return this;
        }

        /// <summary>
        /// Returns a string containing this table formatted for displaying in a log or console.
        /// </summary>
        /// <returns>A string containing the formatted table.</returns>
        public override string ToString()
        {
            return ToString(0, Options.Instance.ColorizeConsoleOutput);
        }

        /// <summary>
        /// Returns a string containing this table formatted for displaying in a log or console.
        /// </summary>
        /// <param name="maxWidth">The maximum width to allow each cell to be.</param>
        /// <returns>A string containing the formatted table.</returns>
        public string ToString(int maxWidth)
        {
            return ToString(maxWidth, Options.Instance.ColorizeConsoleOutput);
        }

        /// <summary>
        /// Returns a string containing this table formatted for displaying in a log or console.
        /// </summary>
        /// <param name="withColor">A value indicating whether or not to print the cell colors with the table.</param>
        /// <returns>A string containing the formatted table.</returns>
        public string ToString(bool withColor)
        {
            return ToString(0, true);
        }

        /// <summary>
        /// Returns a string containing this table formatted for displaying in a log or console.
        /// </summary>
        /// <param name="maxWidth">The maximum width to allow each cell to be.</param>
        /// <param name="withColor">A value indicating whether or not to print the cell colors with the table.</param>
        /// <returns>A string containing the formatted table.</returns>
        public string ToString(int maxWidth, bool withColor)
        {
            var sb = new StringBuilder();

            var numRows = this.NumberOfRows;
            var numCols = this.NumberOfColumns;

            if (maxWidth == 0)
            {
                maxWidth = this.rows.Max(r => r.Max(c => c.GetTotalWidth())) * numCols;
            }

            var columnWidths = new int[numCols];

            for (var i = 0; i < numCols; i++)
            {
                columnWidths[i] = Math.Min(rows.Max(r => r[i].GetTotalWidth()), maxWidth);
                if (headers != null && headers.Count > i)
                {
                    columnWidths[i] = Math.Min(Math.Max(columnWidths[i], headers[i].GetTotalWidth()), maxWidth);
                }

                columnWidths[i] = Math.Max(columnWidths[i], MinimumColumnWidth);
            }

            var totalWidth = columnWidths.Sum();
            totalWidth += (numCols * 2) + numCols + 1;

            sb.Append(horizontalBorder, totalWidth);
            sb.Append(lineEnding);

            if (headers.Count > 0)
            {
                var numLines = headers.Max(c => c.GetNumberOfLines(maxWidth));

                for (var line = 0; line < numLines; line++)
                {
                    sb.Append(verticalBorder);
                    sb.Append(' ');

                    for (var head = 0; head < numCols; head++)
                    {
                        sb.Append(headers[head].GetLine(line, columnWidths[head], withColor));
                        sb.Append(' ');
                        sb.Append(verticalBorder);
                        if (head < numCols - 1)
                        {
                            sb.Append(' ');
                        }
                    }

                    sb.Append(lineEnding);
                }

                sb.Append(verticalBorder);
                sb.Append(headerBottomBorder, totalWidth - 2);
                sb.Append(verticalBorder);
                sb.Append(lineEnding);
            }

            for (var row = 0; row < numRows; row++)
            {
                if (row > 0)
                {
                    sb.Append(verticalBorder);
                    sb.Append(horizontalBorder, totalWidth - 2);
                    sb.Append(verticalBorder);
                    sb.Append(lineEnding);
                }

                var numLines = 0;
                for (var i = 0; i < numCols; i++)
                {
                    numLines = Math.Max(rows[row][i].GetNumberOfLines(columnWidths[i]), numLines);
                }

                for (var line = 0; line < numLines; line++)
                {
                    sb.Append(verticalBorder);
                    sb.Append(' ');

                    for (var col = 0; col < numCols; col++)
                    {
                        sb.Append(rows[row][col].GetLine(line, columnWidths[col], withColor));
                        sb.Append(' ');
                        sb.Append(verticalBorder);

                        if (col < numCols - 1)
                        {
                            sb.Append(' ');
                        }
                    }

                    sb.Append(lineEnding);
                }
            }

            sb.Append(horizontalBorder, totalWidth);
            sb.Append(lineEnding);

            return sb.ToString();
        }
    }
}