using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Elegant.Terminal.Formatting
{
    /// <summary>
    /// Defines the requirements for console color objects.
    /// </summary>
    public interface IConsoleColor
    {
        /// <summary>
        /// Gets the color as a foreground ANSI SGR color format.
        /// </summary>
        /// <returns>An ANSI SGR formatted <see langword="string"/>.</returns>
        string AsForeground();

        /// <summary>
        /// Gets the color as a background ANSI SGR color format.
        /// </summary>
        /// <returns>An ANSI SGR formatted <see langword="string"/>.</returns>
        string AsBackground();
    }
}