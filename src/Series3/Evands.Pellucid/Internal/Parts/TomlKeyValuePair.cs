#region copyright
// <copyright file="TomlKeyValuePair.cs" company="Christopher McNeely">
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
    /// Provides basic key value pair functionality, specifically for TOML functions.
    /// </summary>
    /// <typeparam name="T">The type of object the value will be.</typeparam>
    internal class TomlKeyValuePair<T> : INamedToml
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TomlKeyValuePair{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>        
        public TomlKeyValuePair(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TomlKeyValuePair{T}"/> class.
        /// </summary>
        /// <param name="name">The key name.</param>
        /// <param name="value">The value.</param>        
        public TomlKeyValuePair(string name, T value)
            : this(value)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TomlKeyValuePair{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="comment">A comment associated with the key value pair.</param>        
        public TomlKeyValuePair(T value, string comment)
            : this(string.Empty, value)
        {
            this.Comment = new TomlComment(comment);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TomlKeyValuePair{T}"/> class.
        /// </summary>
        /// <param name="name">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="comment">A comment associated with the key value pair.</param>        
        public TomlKeyValuePair(string name, T value, string comment)
            : this(name, value)
        {
            this.Comment = new TomlComment(comment);
        }

        /// <summary>
        /// Gets or sets the Name of the object, which is the key.
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// Gets the value of the object.
        /// </summary>        
        public T Value { get; private set; }

        /// <summary>
        /// Gets a comment associated with the object.
        /// </summary>        
        public TomlComment Comment { get; private set; }

        /// <summary>
        /// Formats the object as a string for writing to a file.
        /// </summary>
        /// <param name="forArray">If true will format the value for writing inside an array.</param>
        /// <returns>A string formatted for writing to a file.</returns>        
        public string PrintLines(bool forArray)
        {
            var comment = Comment != null && Comment.HasValue ? Comment.PrintLines(forArray) : string.Empty;
            if (string.IsNullOrEmpty(Name))
            {
                return string.Format("\"{0}\"{1}", Value.ToString(), comment);
            }
            else
            {
                return string.Format("{0} = \"{1}\"{2}", Name, Value, comment);
            }
        }
    }
}