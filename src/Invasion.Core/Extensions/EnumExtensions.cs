namespace Invasion.Core.Extensions;

/// <summary>
///     The <see cref="EnumExtensions"/> class
///     provides extension methods on <see cref="Enum"/> values.
/// </summary>
internal static class EnumExtensions
{
    /// <summary>
    ///     Checks whether a provided value contains any flag from the <typeparamref name="E"/> enum.
    /// </summary>
    /// <typeparam name="E">The enum containing the target flags.</typeparam>
    /// <param name="value">The value to check for validity.</param>
    public static bool HasAnyFlag<E>(this E value) where E : unmanaged, Enum
    {
        foreach (E flag in Enum.GetValues<E>())
        {
            if (value.HasFlag(flag))
            {
                return true;
            }
        }

        return false;
    }
}
