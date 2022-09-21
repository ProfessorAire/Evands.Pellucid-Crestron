#region copyright
// <copyright file="ColumnCollection.cs" company="Christopher McNeely">
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
using System.Linq;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// A collection of <see cref="Column"/> objects.
    /// </summary>
    public class ColumnCollection : IEnumerable<Column>
    {
        /// <summary>
        /// List of <see cref="Column"/> items contained in this collection.
        /// </summary>
        private List<Column> columns = new List<Column>();

        /// <summary>
        /// The <see cref="RowCollection"/> this item wraps.
        /// </summary>
        private RowCollection rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnCollection"/> class.
        /// </summary>
        /// <param name="rows">The <see cref="RowCollection"/> to interact with.</param>
        public ColumnCollection(RowCollection rows)
        {
            this.rows = rows;
        }

        /// <summary>
        /// Gets the number of columns this collection contains.
        /// </summary>
        public int Count
        {
            get
            {
                ExpandIfNeeded(rows.Count > 0 ? rows.Max(r => r.Count) - 1 : -1);
                return columns.Count;
            }
        }

        /// <summary>
        /// Gets the <see cref="Column"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="Column"/> to return.</param>
        /// <returns>The <see cref="Column"/> from the specified index.</returns>
        public Column this[int index]
        {
            get
            {
                ExpandIfNeeded(index);
                return columns[index];
            }
        }

        /// <summary>
        /// Gets an enumerator of <see cref="Column"/>s contained in this collection.
        /// </summary>
        /// <returns>An enumeration of <see cref="Column"/> objects.</returns>
        public IEnumerator<Column> GetEnumerator()
        {
            return columns.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator of <see cref="Column"/>s contained in this collection.
        /// </summary>
        /// <returns>An enumeration of <see cref="Column"/> objects as objects.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return columns.GetEnumerator();
        }

        /// <summary>
        /// Checks the underlying <see cref="RowCollection"/> and list of <see cref="Column"/>s to determine if any expansion is needed.
        /// </summary>
        /// <param name="neededIndex">The index required.</param>
        private void ExpandIfNeeded(int neededIndex)
        {
            if (neededIndex >= columns.Count)
            {
                if (rows.Count == 0)
                {
                    rows.Add(new Row());
                }

                var max = rows.Count > 0 ? rows.Max(r => r.Count) : 0;

                while (neededIndex >= columns.Count)
                {
                    columns.Add(new Column(rows, columns.Count));
                }

                foreach (var r in rows)
                {
                    while (r.Count <= neededIndex)
                    {
                        r.AddCell(new Cell());
                    }
                }
            }
        }
    }
}