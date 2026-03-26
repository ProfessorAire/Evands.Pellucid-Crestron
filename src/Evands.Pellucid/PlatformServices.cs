// <copyright file="PlatformServices.cs">
// The MIT License
// Copyright © Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Evands.Pellucid
{
    /// <summary>
    /// Provides access to the current platform services implementation.
    /// When running on Crestron hardware, the Evands.Pellucid.Crestron library
    /// registers a Crestron-specific implementation. Otherwise, a default
    /// implementation is used that writes to <see cref="System.Console"/>.
    /// </summary>
    public static class PlatformServices
    {
        /// <summary>
        /// The current platform services instance.
        /// </summary>
        private static IPlatformServices current;

        /// <summary>
        /// Gets or sets the current <see cref="IPlatformServices"/> implementation.
        /// When not explicitly set, returns <see cref="DefaultPlatformServices.Instance"/>.
        /// </summary>
        public static IPlatformServices Current
        {
            get { return current ?? DefaultPlatformServices.Instance; }
            set { current = value; }
        }

        /// <summary>
        /// Resets the platform services to the default implementation.
        /// This is primarily useful for testing.
        /// </summary>
        internal static void Reset()
        {
            current = null;
        }
    }
}
