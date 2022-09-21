#region copyright
// <copyright file="NestedSample.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.ProDemo.Sample
{
    /// <summary>
    /// Class used for dumping an object with a nested list of objects to the console.
    /// </summary>
    public class NestedSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedSample"/> class.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="items">The list of <see cref="SampleItem"/> objects to include.</param>        
        public NestedSample(string name, List<SampleItem> items)
        {
            Name = name;
            Samples = items;
        }

        /// <summary>
        /// Gets or sets the list of <see cref="SampleItem"/> objects.
        /// </summary>        
        public List<SampleItem> Samples { get; set; }

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>        
        public string Name { get; set; }
    }
}