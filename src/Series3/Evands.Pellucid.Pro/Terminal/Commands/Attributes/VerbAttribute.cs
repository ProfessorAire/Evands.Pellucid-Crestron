#region copyright
// <copyright file="VerbAttribute.cs" company="Christopher McNeely">
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
    /// Declares that a method should be treated as a verb.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class VerbAttribute : Attribute
    {
        /// <summary>
        /// Backing field for the <see cref="Alias"/> property.
        /// </summary>
        private string alias;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerbAttribute"/> class.
        /// <para>Multiple methods may have a verb attribute with the same name.</para>
        /// </summary>
        /// <param name="name">The name the verb should have in the console. Leave empty to use as a command's default action.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        /// <remarks>
        /// Multiple methods can have the same verb attribute name.
        /// <para>Only the help text from the first found attribute will be printed.</para>
        /// </remarks>
        public VerbAttribute(string name, string help)
            : this(name, 0, help)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerbAttribute"/> class.
        /// <para>Multiple methods may have a verb attribute with the same name.</para>
        /// </summary>
        /// <param name="name">The name the verb should have in the console. Leave empty to use as a command's default action.</param>
        /// <param name="aliasLength">The number of characters from the name that should be used as the verb's alias.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        /// <remarks>
        /// Multiple methods can have the same verb attribute name.
        /// <para>Only the help text from the first matching attribute will be printed.</para>
        /// </remarks>
        public VerbAttribute(string name, int aliasLength, string help)
        {
            this.Name = string.IsNullOrEmpty(name) ? string.Empty : name;

            if (aliasLength > 0)
            {
                Alias = Name.Substring(0, Math.Min(aliasLength, Name.Length));
                HelpFormattedName = string.Format("{0} ({1})", Name, Alias);
            }
            else
            {
                Alias = string.Empty;
                HelpFormattedName = Name;
            }

            this.Help = string.IsNullOrEmpty(help) ? "No help is available for this command." : help;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VerbAttribute"/> class.
        /// <para>Multiple methods may have a verb attribute with the same name.</para>
        /// </summary>
        /// <param name="name">The name the verb should have in the console. Leave empty to use as a command's default action.</param>
        /// <param name="alias">The alias to use for the verb.</param>
        /// <param name="help">Help text describing the use of the flag.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name parameter is null or an empty string.</exception>
        /// <remarks>
        /// Multiple methods can have the same verb attribute name.
        /// <para>Only the help text from the first matching attribute will be printed.</para>
        /// </remarks>
        public VerbAttribute(string name, string alias, string help)
        {
            this.Name = string.IsNullOrEmpty(name) ? string.Empty : name;
            if (!string.IsNullOrEmpty(alias))
            {
                Alias = alias;
                HelpFormattedName = string.Format("{0} ({1})", Name, Alias);
            }
            else
            {
                Alias = string.Empty;
                HelpFormattedName = Name;
            }

            this.Help = string.IsNullOrEmpty(help) ? "No help is available for this command." : help;
        }

        /// <summary>
        /// Gets the name of the verb as it should be seen in the console.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the alias of the verb as it should be seen in the console.
        /// </summary>
        public string Alias
        {
            get { return this.alias; }

            private set
            {
                if (alias != value)
                {
                    alias = value;
                    if (!string.IsNullOrEmpty(alias) && !string.IsNullOrEmpty(Name))
                    {
                        HelpFormattedName = string.Format("{0} ({1})", Name, Alias);
                    }
                    else if (string.IsNullOrEmpty(alias) && !string.IsNullOrEmpty(Name))
                    {
                        HelpFormattedName = Name;
                    }
                    else
                    {
                        HelpFormattedName = "<none>";
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets the help text for the verb.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Gets the name/alias of the verb formatted for presentation in help documentation.
        /// </summary>
        public string HelpFormattedName { get; private set; }
    }
}