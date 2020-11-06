#region copyright
// <copyright file="FlagAttribute.cs" company="Christopher McNeely">
// The MIT License (MIT)
// Copyright (c) Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
#endregion

using System;

namespace Elegant.Pellucid.Attributes
{
    /// <summary>
    /// Declares that a parameter should be treated as a flag.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class FlagAttribute : Attribute
    {
        readonly string help;

        readonly string name;

        readonly bool isOptional = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name the flag should have in the console.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        public FlagAttribute(string name, string help)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Flags cannot have empty names!");
            }

            this.help = help;
            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name the flag should have in the console.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <param name="isOptional">When true sets the flag as optional and will default to false when the method is called.</param>
        public FlagAttribute(string name, string help, bool isOptional)
            : this(name, help)
        {
            this.isOptional = isOptional;
        }

        /// <summary>
        /// Gets the help text for the flag.
        /// </summary>
        public string Help { get { return help; } }

        /// <summary>
        /// Gets the name of the flag as it should be seen in the console.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Gets a value indicating whether the flag is required or optional.
        /// </summary>
        public bool IsOptional { get { return isOptional; } }
    }
}