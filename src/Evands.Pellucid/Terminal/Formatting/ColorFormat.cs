#region copyright
// <copyright file="ColorFormat.cs" company="Christopher McNeely">
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
using Evands.Pellucid.Terminal;

namespace Evands.Pellucid.Terminal.Formatting
{
    /// <summary>
    /// An object used to store color information about both foreground and backgrounds.
    /// </summary>
    public class ColorFormat
    {
        /// <summary>
        /// A ColorFormat with no foreground or background color defined.
        /// </summary>
        public static readonly ColorFormat None = new ColorFormat(null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorFormat"/> class.
        /// </summary>
        /// <param name="foreground">The foreground color to assign the format.</param>
        /// <param name="background">The background color to assign the format.</param>
        public ColorFormat(IConsoleColor foreground, IConsoleColor background)
        {
            if (foreground == null)
            {
                Foreground = (StandardColor)ColorCode.None;
            }
            else
            {
                Foreground = foreground;
            }

            if (background == null)
            {
                Background = (StandardColor)ColorCode.None;
            }
            else
            {
                Background = background;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorFormat"/> class.
        /// </summary>
        /// <param name="foreground">The foreground color to assign the format.</param>
        public ColorFormat(IConsoleColor foreground)
            : this(foreground, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorFormat"/> class.
        /// </summary>
        /// <param name="foreground">The foreground color to assign the format.</param>
        /// <param name="background">The background color to assign the format.</param>
        public ColorFormat(ColorCode foreground, ColorCode background)
        {
            Foreground = (StandardColor)foreground;
            Background = (StandardColor)background;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorFormat"/> class.
        /// </summary>
        /// <param name="foreground">The foreground color to assign the format.</param>
        public ColorFormat(ColorCode foreground)
        {
            Foreground = (StandardColor)foreground;
            Background = new StandardColor(ColorCode.None);
        }

        /// <summary>
        /// Gets the <see cref="IConsoleColor"/> to use for the foreground.
        /// </summary>
        public IConsoleColor Foreground { get; private set; }

        /// <summary>
        /// Gets the <see cref="IConsoleColor"/> to use for the background.
        /// </summary>
        public IConsoleColor Background { get; private set; }

        /// <summary>
        /// Formats the provided text to include an ANSI SGR escape sequence.
        /// </summary>
        /// <param name="foreground">The <see cref="IConsoleColor"/> to use for the foreground.</param>
        /// <param name="background">The <see cref="IConsoleColor"/> to use for the background.</param>
        /// <param name="textToFormat">The text to format.</param>
        /// <param name="args">Optional array of arguments to use when formatting the text.</param>
        /// <returns>A <see langword="string"/> containing the provided text and ANSI SGR color formatting based on the colors provided.</returns>
        public static string FormatText(IConsoleColor foreground, IConsoleColor background, string textToFormat, params object[] args)
        {
            return FormatText(foreground, background, true, textToFormat, args);
        }

        /// <summary>
        /// Formats the provided text to include an ANSI SGR escape sequence.
        /// </summary>
        /// <param name="foreground">The <see cref="IConsoleColor"/> to use for the foreground.</param>
        /// <param name="background">The <see cref="IConsoleColor"/> to use for the background.</param>
        /// <param name="closeColorFormat">When true will end the text with a terminating ANSI SGR sequence.</param>
        /// <param name="textToFormat">The text to format.</param>
        /// <param name="args">Optional array of arguments to use when formatting the text.</param>
        /// <returns>A <see langword="string"/> containing the provided text and ANSI SGR color formatting based on the colors provided.</returns>
        public static string FormatText(IConsoleColor foreground, IConsoleColor background, bool closeColorFormat, string textToFormat, params object[] args)
        {
            if (Options.Instance.ColorizeConsoleOutput)
            {
                var fore = foreground.AsForeground();
                var back = background.AsBackground();

                if (string.IsNullOrEmpty(fore) && string.IsNullOrEmpty(back))
                {
                    return textToFormat.OptionalFormat(args);
                }

                return string.Format("\x1b[{0}{1}{2}m{3}{4}",
                    fore,
                    !string.IsNullOrEmpty(fore) && !string.IsNullOrEmpty(back) ? ";" : string.Empty,
                    back,
                    textToFormat.OptionalFormat(args),
                    closeColorFormat ? "\x1b[0m" : string.Empty);
            }
            else
            {
                return textToFormat.OptionalFormat(args);
            }
        }

        /// <summary>
        /// Formats the provided text to include an ANSI SGR escape sequence.
        /// </summary>
        /// <param name="textToFormat">The text to format.</param>
        /// <param name="args">Optional array of arguments to use when formatting the text.</param>
        /// <returns>A <see langword="string"/> containing this <see cref="ColorFormat"/> object's ANSI SGR color formatting.</returns>
        public string FormatText(string textToFormat, params object[] args)
        {
            return FormatText(true, textToFormat, args);
        }

        /// <summary>
        /// Formats the provided text to include an ANSI SGR escape sequence.
        /// </summary>
        /// <param name="closeColorFormat">When true will end the text with a terminating ANSI SGR sequence.</param>
        /// <param name="textToFormat">The text to format.</param>
        /// <param name="args">Optional array of arguments to use when formatting the text.</param>
        /// <returns>A <see langword="string"/> containing this <see cref="ColorFormat"/> object's ANSI SGR color formatting.</returns>
        public string FormatText(bool closeColorFormat, string textToFormat, params object[] args)
        {
            return FormatText(Foreground, Background, closeColorFormat, textToFormat, args);
        }
    }
}