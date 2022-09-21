#region copyright
// <copyright file="DumpObjectFailure.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    /// <summary>
    /// The DumpObjectFailure Class.
    /// </summary>
    internal class DumpObjectFailure
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DumpObjectFailure"/> class.
        /// </summary>
        public DumpObjectFailure()
        {
            PropertyName = string.Empty;
            ErrorMessage = string.Empty;
            ExceptionMessage = string.Empty;
        }

        /// <summary>
        /// Gets or sets the name of the property that a failure occurred while getting the value of.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the type of exception that was caught.
        /// </summary>
        public string ExceptionType { get; set; }
    }
}
