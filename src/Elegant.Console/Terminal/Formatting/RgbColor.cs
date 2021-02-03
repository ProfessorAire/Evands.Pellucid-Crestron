using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Elegant.Terminal.Formatting
{
    /// <summary>
    /// Defines an RGB console color.
    /// </summary>
    public struct RgbColor : IConsoleColor
    {
        /// <summary>
        /// Gets or sets the value of the Red <see langword="byte"/>.
        /// </summary>        
        public byte Red { get; set; }

        /// <summary>
        /// Gets or sets the value of the Green <see langword="byte"/>.
        /// </summary>        
        public byte Green { get; set; }

        /// <summary>
        /// Gets or sets the value of the Blue <see langword="byte"/>.
        /// </summary>        
        public byte Blue { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="RgbColor"/> class.
        /// </summary>
        /// <param name="red">The value of the red <see langword="byte"/>.</param>
        /// <param name="green">The value of the green <see langword="byte"/>.</param>
        /// <param name="blue">The value of the blue <see langword="byte"/>.</param>        
        public RgbColor(byte red, byte green, byte blue)
            : this()
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <inheritdoc />
        public string AsForeground()
        {
            return string.Format("38;2;{0};{1};{2}", Red, Green, Blue);
        }

        /// <inheritdoc />
        public string AsBackground()
        {
            return string.Format("48;2;{0};{1};{2}", Red, Green, Blue);
        }
    }
}