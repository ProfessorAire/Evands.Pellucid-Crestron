using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Elegant.Terminal.Formatting
{
    /// <summary>
    /// Defines a standard ANSI console color.
    /// </summary>
    public struct StandardColor : IConsoleColor, IEquatable<ColorCode>
    {
        /// <summary>
        /// Gets or sets the <see cref="ColorCode"/> associated with the color.
        /// </summary>        
        public ColorCode Code { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardColor"/> class.
        /// </summary>
        /// <param name="code">The <see cref="ColorCode"/> to associate with the class.</param>        
        public StandardColor(ColorCode code)
            : this()
        {
            Code = code;
        }

        /// <inheritdoc />
        public string AsForeground()
        {
            if (Code != ColorCode.None)
            {
                return string.Format("{0}", (int)Code + 30);
            }

            return string.Empty;
        }

        /// <inheritdoc />
        public string AsBackground()
        {
            if (Code != ColorCode.None)
            {
                return string.Format("{0}", (int)Code + 40);
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks for equality between this and a <see cref="ColorCode"/>.
        /// </summary>
        /// <param name="other">The <see cref="ColorCode"/> to compare equality with.</param>
        /// <returns><see langword="true"/> if the objects are equal, otherwise <see langword="false"/>.</returns>
        public bool Equals(ColorCode other)
        {
            return this.Code == other;
        }

        /// <summary>
        /// Implicitly converts the specified <see cref="ColorCode"/> into a <see cref="StandardColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="ColorCode"/> to convert.</param>
        /// <returns>A <see cref="StandardColor"/>.</returns>
        public static implicit operator StandardColor(ColorCode color)
        {
            return new StandardColor(color);
        }
    }
}