#region copyright
// <copyright file="SampleAttribute.cs" company="Christopher McNeely">
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

namespace EVS.Pellucid.Terminal.Commands.Attributes
{
    /// <summary>
    /// Defines a sample that will be printed with a Verb's help.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class SampleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleAttribute"/> class.
        /// </summary>
        /// <param name="sample">The text of the sample that should be printed to the console help.</param>
        /// <param name="description">A description to accompany the sample text.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sample"/> equals <see cref="String.Empty"/> or <see langword="null"/>.</exception>
        public SampleAttribute(string sample, string description)
        {
            if (string.IsNullOrEmpty(sample))
            {
                throw new ArgumentNullException("sample", "Samples must have sample text.");
            }

            this.Sample = sample;
            this.Description = string.IsNullOrEmpty(description) ? string.Empty : description;
        }

        /// <summary>
        /// Gets the text representing the sample that should be printed to the console with the verb's help.
        /// </summary>
        public string Sample { get; private set; }

        /// <summary>
        /// Gets the text describing the sample that should be printed to the console with the sample.
        /// </summary>
        public string Description { get; private set; }
    }
}