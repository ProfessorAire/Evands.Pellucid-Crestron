#region copyright
// <copyright file="Column.cs" company="Christopher McNeely">
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
using System.Collections.ObjectModel;
using System.Linq;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Provides a collection of cells organized as a column.
    /// <para>Not intended for stand-alone use, requires a reference to a <see cref="RowCollection"/> object.</para>
    /// </summary>
    public class Column
    {
        /// <summary>
        /// The <see cref="RowCollection"/> the column is related to.
        /// </summary>
        private RowCollection rows;

        /// <summary>
        /// The index the column exists at in relation to the underlying <see cref="RowCollection"/>.
        /// </summary>
        private int columnIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        /// <param name="rows">The underlying <see cref="RowCollection"/> this column uses.</param>
        /// <param name="columnIndex">The index of the column in relation to the provided <see cref="RowCollection"/>.</param>
        public Column(RowCollection rows, int columnIndex)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }

            this.rows = rows;
            this.columnIndex = columnIndex;
        }

        /// <summary>
        /// Gets the number of cells the column contains.
        /// </summary>
        public int Count
        {
            get
            {
                return rows.Count;
            }
        }

        /// <summary>
        /// Gets or sets the cell at the specified index.
        /// </summary>
        /// <param name="index">The index of the cell to return.</param>
        /// <returns>The <see cref="Cell"/> at the specified index.</returns>
        public Cell this[int index]
        {
            get
            {
                ExpandIfNeeded(index);
                return rows[index][columnIndex];
            }

            set
            {
                ExpandIfNeeded(index);
                rows[index][columnIndex] = value;
            }
        }

        /// <summary>
        /// Checks the column to see if the underlying <see cref="RowCollection"/> needs to be expanded to accommodate the specified index.
        /// </summary>
        /// <param name="indexNeeded">The index that needs to be interacted with.</param>
        private void ExpandIfNeeded(int indexNeeded)
        {
            while (indexNeeded >= rows.Count)
            {
                rows.Add(new Row(rows.Max(r => r.Count)));
            }
        }
    }
}