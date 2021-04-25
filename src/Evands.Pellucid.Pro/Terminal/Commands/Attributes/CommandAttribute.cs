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

namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    /// <summary>
    /// Declares that a class should be treated as a console command.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        /// <summary>
        /// Backing field for the <see cref="Alias"/> property.
        /// </summary>
        private string alias = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the command, as it should be seen on the console.</param>
        /// <param name="help">Help to print in the console regarding the command.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public CommandAttribute(string name, string help)
            : this(name, 0, help)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the command, as it should be seen on the console.</param>
        /// <param name="aliasLength">The number of characters from the name that should be used for the command's alias.</param>
        /// <param name="help">Help to print in the console regarding the command.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public CommandAttribute(string name, int aliasLength, string help)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Commands must have names.");
            }

            Name = name;
            Help = string.IsNullOrEmpty(help) ? string.Empty : help;
            if (aliasLength > 0)
            {
                Alias = Name.Substring(0, Math.Min(aliasLength, Name.Length));
            }
            else
            {
                Alias = string.Empty;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the command, as it should be seen on the console.</param>
        /// <param name="alias">The alias to use for the command.</param>
        /// <param name="help">Help to print in the console regarding the command.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        public CommandAttribute(string name, string alias, string help)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Commands must have names.");
            }

            Name = name;
            Help = string.IsNullOrEmpty(help) ? string.Empty : help;
            Alias = string.IsNullOrEmpty(alias) ? string.Empty : alias;
        }

        /// <summary>
        /// Gets the name of the command as it should be seen in the console.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the help text for the command.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Gets the name/alias of the command formatted for presentation in help documentation.
        /// </summary>
        public string HelpFormattedName { get; private set; }

        /// <summary>
        /// Gets the alias of the command as it can be seen in the console.
        /// </summary>
        public string Alias
        {
            get { return this.alias; }

            private set
            {
                if (alias != value)
                {
                    alias = value;
                    if (!string.IsNullOrEmpty(alias))
                    {
                        HelpFormattedName = string.Format("{0} ({1})", Name, Alias);
                    }
                    else
                    {
                        HelpFormattedName = Name;
                    }
                }
            }
        }
    }
}