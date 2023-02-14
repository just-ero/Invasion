namespace Invasion.Core.Models;

/// <summary>
///     The <see cref="MemoryRegion"/> struct
///     represents a region of consecutive memory in a <see cref="Process"/>.
/// </summary>
public readonly struct MemoryRegion
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryRegion"/> struct.
    /// </summary>
    /// <param name="base">The base address of the <see cref="MemoryRegion"/>.</param>
    /// <param name="size">The size of the <see cref="MemoryRegion"/>, in bytes.</param>
    public MemoryRegion(nint @base, int size)
    {
        Base = @base;
        Size = size;
    }

    /// <summary>
    ///     Gets the base address of the <see cref="MemoryRegion"/>.
    /// </summary>
    public nint Base { get; }

    /// <summary>
    ///     Gets the size of the <see cref="MemoryRegion"/>, in bytes.
    /// </summary>
    public int Size { get; }
}
