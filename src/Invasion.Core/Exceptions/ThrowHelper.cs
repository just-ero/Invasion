using System.Runtime.CompilerServices;

namespace Invasion.Core.Exceptions;

internal static partial class ThrowHelper
{
    /// <summary>
    ///     Checks whether the provided <paramref name="argument"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="argument">The variable to check.</param>
    /// <param name="message">A message that describes the exception</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="argument"/> was <see langword="null"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull(
        object argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))] string paramName = "")
    {
        if (argument is null)
        {
            if (message is not null)
            {
                Throw.ArgumentNull(paramName, message);
            }
            else
            {
                Throw.ArgumentNull(paramName);
            }
        }
    }

    /// <summary>
    ///     Calls <see cref="Process.Refresh"/> on <paramref name="process"/>
    ///     and checks whether its <see cref="Process.HasExited"/> property is <see langword="true"/>.
    /// </summary>
    /// <param name="process">The <see cref="Process"/> to check.</param>
    /// <param name="message">A message that describes the exception.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the <see cref="Process.HasExited"/> property of <paramref name="process"/>
    ///     returned <see langword="true"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfProcessExited(Process process, string message)
    {
        process.Refresh();

        if (process.HasExited)
        {
            Throw.InvalidOperation(message);
        }
    }
}
