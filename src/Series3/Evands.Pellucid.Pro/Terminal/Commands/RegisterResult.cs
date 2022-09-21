#region copyright
// <copyright file="RegisterResult.cs" company="Christopher McNeely">
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

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Provides details about whether or not a command successfully registered and, if not, why.
    /// </summary>
    public enum RegisterResult
    {
        /// <summary>
        /// Indicates no registration operation has been performed.
        /// </summary>
        NoOperationPerformed,

        /// <summary>
        /// Indicates the command registered successfully.
        /// </summary>
        Success,

        /// <summary>
        /// Indicates a <see cref="Attributes.CommandAttribute"/> wasn't found on the class specified.
        /// </summary>
        NoCommandAttributeFound,

        /// <summary>
        /// Indicates a command with the specified name already exists. Use a name override if necessary.
        /// </summary>
        CommandNameAlreadyExists,

        /// <summary>
        /// The specified Global Command was not found.
        /// </summary>
        GlobalCommandNotFound
    }
}