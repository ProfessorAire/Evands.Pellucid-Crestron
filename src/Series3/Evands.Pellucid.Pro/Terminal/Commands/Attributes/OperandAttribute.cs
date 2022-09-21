#region copyright
// <copyright file="OperandAttribute.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    /// <summary>
    /// Declares that a parameter should be treated as an operand.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class OperandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperandAttribute"/> class.
        /// </summary>
        /// <param name="name">The name the operand should have in the console.</param>
        /// <param name="help">Help text describing the use of the operand.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public OperandAttribute(string name, string help)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Operands cannot have empty names.");
            }

            this.Name = name;
            this.Help = string.IsNullOrEmpty(help) ? string.Empty : help;
        }

        /// <summary>
        /// Gets the help text for the operand.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Gets the name of the operand as it should be seen in the console.
        /// </summary>
        public string Name { get; private set; }
    }
}