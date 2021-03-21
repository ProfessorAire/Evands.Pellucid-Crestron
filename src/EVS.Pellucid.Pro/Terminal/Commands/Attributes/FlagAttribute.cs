#region copyright
// <copyright file="FlagAttribute.cs" company="Christopher McNeely">
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
    /// Declares that a parameter should be treated as a flag.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class FlagAttribute : Attribute
    {
        /// <summary>
        /// Backing field for the <see cref="IsOptional"/> property.
        /// </summary>
        private readonly bool isOptional = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name the flag should have in the console.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public FlagAttribute(string name, string help)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Flags cannot have empty names.");
            }

            this.Name = name;
            this.Help = string.IsNullOrEmpty(help) ? string.Empty : help;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name the flag should have in the console.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <param name="isOptional">When true sets the flag as optional and will default to false when the method is called.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public FlagAttribute(string name, string help, bool isOptional)
            : this(name, help)
        {
            this.isOptional = isOptional;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name the flag should have in the console.</param>
        /// <param name="shortName">The short name of the flag, as a single <see langword="char"/>.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public FlagAttribute(string name, char shortName, string help)
            : this(name, help)
        {
            this.ShortName = shortName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name the flag should have in the console.</param>
        /// <param name="shortName">The short name of the flag, as a single <see langword="char"/>.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <param name="isOptional">When true sets the flag as optional and will default to false when the method is called.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public FlagAttribute(string name, char shortName, string help, bool isOptional)
            : this(name, help, isOptional)
        {
            this.ShortName = shortName;
        }

        /// <summary>
        /// Gets the help text for the flag.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Gets the name of the flag as it should be seen in the console.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the flag is required or optional.
        /// </summary>
        public bool IsOptional
        {
            get
            {
                return isOptional;
            }
        }

        /// <summary>
        /// Gets the optional short name of the flag, as a <see langword="char"/>.
        /// </summary>
        public char? ShortName { get; private set; }
    }
}