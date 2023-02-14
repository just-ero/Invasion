using System.Runtime.CompilerServices;

namespace Invasion.Core.Extensions;

/// <summary>
///     The see <see cref="PointerExtensions"/> class
///     provides extension methods assisting in pointer-related work.
/// </summary>
internal static unsafe class PointerExtensions
{
    /// <summary>
    ///     Converts a pointer to a null-terminated array of type <see cref="byte"/> into a string.
    /// </summary>
    /// <param name="chars">The pointer to the array of characters.</param>
    public static string StringFromBytePtr(byte* chars)
    {
        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(chars).ToString();
    }

    /// <summary>
    ///     Converts a pointer to a null-terminated array of type <see cref="ushort"/> into a string.
    /// </summary>
    /// <param name="chars">The pointer to the array of characters.</param>
    public static string StringFromCharPtr(ushort* chars)
    {
        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((char*)chars).ToString();
    }

    public static bool IsPointer<T>() where T : unmanaged
    {
        return typeof(T) == typeof(nint) || typeof(T) == typeof(IntPtr) || typeof(T) == typeof(nuint) || typeof(T) == typeof(UIntPtr);
    }

    public static int GetTypeSize<T>(bool is64Bit) where T : unmanaged
    {
        return IsPointer<T>() ? (is64Bit ? 0x8 : 0x4) : sizeof(T);
    }

    /// <summary>
    ///     Calculates the byte alignment required for the specified type.
    /// </summary>
    /// <typeparam name="T">The type whose byte alignment to calculate.</typeparam>
    public static nuint AlignOf<T>()
    {
        // Take the size of the type plus previous padding and subtract the type size so only the padding remains.
        return (uint)Unsafe.SizeOf<AlignOfHelper<T>>() - (uint)Unsafe.SizeOf<T>();
    }

    /// <summary>
    ///     The <see cref="AlignOfHelper{T}"/> struct
    ///     provides functionality for the <see cref="AlignOf{T}"/> method.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private struct AlignOfHelper<T>
    {
        /// <summary>
        ///     A minimum amount of padding for <see cref="Value"/> to be aligned to its next alignment byte.
        /// </summary>
        public byte Padding;

        public T Value;
    }
}
