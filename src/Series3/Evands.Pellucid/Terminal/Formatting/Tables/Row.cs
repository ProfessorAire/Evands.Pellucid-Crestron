#region copyright
// <copyright file="Row.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Provides a collection of cells organized as a row.
    /// </summary>
    public class Row : IEnumerable<Cell>
    {
        /// <summary>
        /// A list of <see cref="Cell"/> objects associated with the row.
        /// </summary>
        private List<Cell> cells = new List<Cell>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        public Row()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        /// <param name="intialQuantity">The initial number of empty cells the row contains.</param>
        public Row(int intialQuantity)
        {
            for (var i = 0; i < intialQuantity; i++)
            {
                cells.Add(new Cell());
            }
        }

        /// <summary>
        /// Gets the number of cells the row contains.
        /// </summary>
        public int Count
        {
            get { return cells.Count; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Cell"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="Cell"/> to return.</param>
        /// <returns>The <see cref="Cell"/> at the specified index.</returns>
        public Cell this[int index]
        {
            get
            {
                ExpandIfNeeded(index);
                return cells[index];
            }

            set
            {
                ExpandIfNeeded(index);
                if (cells[index] != value)
                {
                    cells[index] = value;
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="Cell"/> to the end of the row.
        /// </summary>
        /// <param name="cell">The <see cref="Cell"/> to add.</param>
        /// <returns>This <see cref="Row"/>.</returns>
        public Row AddCell(Cell cell)
        {
            cells.Add(cell);
            return this;
        }

        /// <summary>
        /// Adds the specified <see langword="string"/> contents as a new cell at the end of the row.
        /// </summary>
        /// <param name="contents">The <see langword="string"/> to add to the new cell's contents.</param>
        /// <returns>This <see cref="Row"/>.</returns>
        public Row AddCell(string contents)
        {
            cells.Add(new Cell(contents));
            return this;
        }

        /// <summary>
        /// Adds new <see cref="Cell"/> objects equal to the number of <see langword="string"/> objects in the <paramref name="contents"/> array.
        /// </summary>
        /// <param name="contents">The <see langword="string"/> objects to add to the new <see cref="Cell"/> contents.</param>
        /// <returns>This <see cref="Row"/>.</returns>
        public Row AddCells(params string[] contents)
        {
            foreach (var c in contents)
            {
                cells.Add(new Cell(c));
            }

            return this;
        }

        /// <summary>
        /// Adds the provided <see cref="Cell"/> objects to the end of the row.
        /// </summary>
        /// <param name="cells">The <see cref="Cell"/> objects to add.</param>
        /// <returns>This <see cref="Row"/>.</returns>
        public Row AddCells(params Cell[] cells)
        {
            this.cells.AddRange(cells);
            return this;
        }

        /// <summary>
        /// Adds the provided enumeration of <see cref="Cell"/> objects to the row.
        /// </summary>
        /// <param name="cells">The <see cref="Cell"/> objects to add.</param>
        /// <returns>This <see cref="Row"/>.</returns>
        public Row AddCells(IEnumerable<Cell> cells)
        {
            this.cells.AddRange(cells);
            return this;
        }

        /// <summary>
        /// Clears this row's cells.
        /// </summary>
        /// <returns>This <see cref="Row"/>.</returns>
        public Row Clear()
        {
            this.cells.Clear();
            return this;
        }

        /// <summary>
        /// Gets an enumerator of <see cref="Cell"/> objects.
        /// </summary>
        /// <returns>An enumerator of <see cref="Cell"/> objects.</returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            return cells.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator of <see cref="Cell"/> objects as objects.
        /// </summary>
        /// <returns>An enumerator of <see cref="Cell"/> objects as objects.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return cells.GetEnumerator();
        }

        /// <summary>
        /// Checks the row to see if the underlying list of <see cref="Cell"/> objects needs to be expanded to provide an object at the index specified.
        /// </summary>
        /// <param name="indexNeeded">The index that needs to be interacted with.</param>
        private void ExpandIfNeeded(int indexNeeded)
        {
            while (indexNeeded >= cells.Count)
            {
                cells.Add(new Cell());
            }
        }
    }
}