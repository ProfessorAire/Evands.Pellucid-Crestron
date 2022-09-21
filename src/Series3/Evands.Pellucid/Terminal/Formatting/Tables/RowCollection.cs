#region copyright
// <copyright file="RowCollection.cs" company="Christopher McNeely">
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
using System.Collections.Generic;
using System.Linq;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// A collection of <see cref="Row"/> objects.
    /// </summary>
    public class RowCollection : IEnumerable<Row>
    {
        /// <summary>
        /// List of <see cref="Row"/> objects.
        /// </summary>
        private List<Row> rows = new List<Row>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RowCollection"/> class.
        /// </summary>
        public RowCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowCollection"/> class.
        /// </summary>
        /// <param name="defaultValues">The <see cref="Row"/> items this collection contains to start.</param>
        public RowCollection(IEnumerable<Row> defaultValues)
        {
            rows.AddRange(defaultValues);
        }

        /// <summary>
        /// Gets the number of items in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return rows.Count;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Row"/> at the specified index.
        /// </summary>
        /// <param name="index">The index to retrieve the <see cref="Row"/> from.</param>
        /// <returns>A <see cref="Row"/> from the specified index.</returns>
        public Row this[int index]
        {
            get
            {
                ExpandIfNeeded(index);
                return rows[index];
            }

            set
            {
                ExpandIfNeeded(index);
                rows[index] = value;
            }
        }

        /// <summary>
        /// Adds the provided <see cref="Row"/> to the end of the collection.
        /// </summary>
        /// <param name="row">The <see cref="Row"/> to add.</param>
        public void Add(Row row)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            rows.Add(row);
        }

        /// <summary>
        /// Adds the provided range of <see cref="Row"/> objects to the end of the collection.
        /// </summary>
        /// <param name="rows">The <see cref="Row"/> objects to add.</param>
        public void AddRange(IEnumerable<Row> rows)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }

            this.rows.AddRange(rows);
        }

        /// <summary>
        /// Clears the collection of any rows.
        /// </summary>
        public void Clear()
        {
            rows.Clear();
        }

        /// <summary>
        /// Gets an enumerator of <see cref="Row"/>s contained in this collection.
        /// </summary>
        /// <returns>An enumeration of <see cref="Row"/> objects.</returns>
        public IEnumerator<Row> GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator of <see cref="Row"/>s contained in this collection.
        /// </summary>
        /// <returns>An enumeration of <see cref="Row"/> objects as objects.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        /// <summary>
        /// Checks the list of <see cref="Row"/> objects to determine if any expansion is needed to interact with the specified index.
        /// </summary>
        /// <param name="neededIndex">The index required.</param>
        private void ExpandIfNeeded(int neededIndex)
        {
            if (neededIndex >= rows.Count)
            {
                var max = rows.Count > 0 ? rows.Max(r => r.Count) : 0;

                while (neededIndex >= rows.Count)
                {
                    rows.Add(new Row(max));
                }
            }
        }
    }
}