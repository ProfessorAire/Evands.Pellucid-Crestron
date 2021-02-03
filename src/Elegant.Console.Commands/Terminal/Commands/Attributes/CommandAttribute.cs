#region copyright
// <copyright file="CommandAttribute.cs" company="Christopher McNeely">
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

namespace Elegant.Terminal.Commands.Attributes
{
    /// <summary>
    /// Declares that a class should be treated as a console command.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the command, as it should be seen on the console.</param>
        /// <param name="help">Help to print in the console regarding the command.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public CommandAttribute(string name, string help)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Commands must have names.");
            }

            this.Name = name;
            this.Help = string.IsNullOrEmpty(help) ? string.Empty : help;
        }

        /// <summary>
        /// Gets the name of the command as it should be seen in the console.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the help text for the command.
        /// </summary>
        public string Help { get; private set; }
    }
}