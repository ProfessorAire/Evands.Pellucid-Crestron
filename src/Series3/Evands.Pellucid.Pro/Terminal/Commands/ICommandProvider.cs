#region copyright
// <copyright file="ICommandProvider.cs" company="Christopher McNeely">
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Defines the requirements for a class that provides a terminal command.
    /// <para>This is an optional helper that can standardize the discovery of terminal commands when creating classes.</para>
    /// </summary>
    /// <remarks>
    /// Typically this is useful when needing to iterate through a list of classes, such as a list of devices. Each can be
    /// checked to see if they can provide terminal commands and those commands created and registered.
    /// </remarks>
    public interface ICommandProvider
    {
        /// <summary>
        /// Gets a list of commands associated with this object.
        /// </summary>
        ReadOnlyCollection<TerminalCommandBase> Commands { get; }
    }
}