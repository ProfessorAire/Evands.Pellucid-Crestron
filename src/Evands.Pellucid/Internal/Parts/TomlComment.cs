#region copyright
// <copyright file="TomlComment.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Internal.Parts
{
    /// <summary>
    /// Represents a TOML comment.
    /// </summary>
    internal class TomlComment : IPrintToml
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TomlComment"/> class.
        /// </summary>
        /// <param name="value">The contents of the comment.</param>
        public TomlComment(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of the comment.
        /// </summary>        
        public string Value { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="Value"/> property is an empty string.
        /// </summary>        
        public bool HasValue
        {
            get
            {
                return !string.IsNullOrEmpty(Value);
            }
        }

        /// <summary>
        /// Formats the object as a string for writing to a file.
        /// </summary>
        /// <param name="forArray">If true will format the value for writing inside an array.</param>
        /// <returns>A string formatted for writing to a file.</returns>        
        public string PrintLines(bool forArray)
        {
            if (string.IsNullOrEmpty(Value))
            {
                return string.Empty;
            }

            return string.Format("#{0}{1}", Value.StartsWith(" ") ? string.Empty : " ", Value);
        }
    }
}