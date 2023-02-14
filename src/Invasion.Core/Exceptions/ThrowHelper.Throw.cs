using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Invasion.Core.Exceptions;

internal static partial class ThrowHelper
{
    public static class Throw
    {
        /// <summary>
        ///     Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the exception.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Argument(string paramName, string message)
        {
            throw new ArgumentException(message, paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was <see langword="null"/>.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentNull(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that was <see langword="null"/>.</param>
        /// <param name="message">A message that describes the exception.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentNull(string paramName, string message)
        {
            throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        ///     Throws a <see cref="BadImageFormatException"/>.
        /// </summary>
        /// <param name="message">A message that describes the exception.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void BadImageFormat(string message)
        {
            throw new BadImageFormatException(message);
        }

        /// <summary>
        ///     Throws an <see cref="EndOfStreamException"/>.
        /// </summary>
        /// <param name="message">A message that describes the exception.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void EndOfStream(string message)
        {
            throw new EndOfStreamException(message);
        }

        /// <summary>
        ///     Throws an <see cref="InvalidOperationException"/>
        ///     with a specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">A message that describes the exception.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void InvalidOperation(string message)
        {
            throw new InvalidOperationException(message);
        }

        /// <summary>
        ///     Throws a <see cref="PlatformNotSupportedException"/>
        ///     with a specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">A message that describes the exception.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void PlatformNotSupported(string message)
        {
            throw new PlatformNotSupportedException(message);
        }

        /// <summary>
        ///     Throws a <see cref="Win32Exception"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Win32()
        {
            throw new Win32Exception();
        }
    }
}
