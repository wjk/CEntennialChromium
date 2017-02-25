using System;

namespace BuildChromium.Utilities
{
    /// <summary>
    /// The BuilderException is thrown when a fatal error occurs
    /// during the build process that should be reported using the
    /// <see cref="BuildChromium.Logging.Log"/> mechanism.
    /// </summary>
    public sealed class BuilderException : Exception
    {
        /// <summary>
        /// Creates a new instance of BuilderException.
        /// </summary>
        public BuilderException()
        {
        }

        /// <summary>
        /// Creates a new instance of BuilderException.
        /// </summary>
        /// <param name="message">
        /// A string to be logged that describes the error.
        /// </param>
        public BuilderException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of BuilerException.
        /// </summary>
        /// <param name="message">
        /// A string to be logged that describes the error.
        /// </param>
        /// <param name="innerException">
        /// Another <see cref="Exception"/> instance that further describes
        /// the cause of the error.
        /// </param>
        public BuilderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
