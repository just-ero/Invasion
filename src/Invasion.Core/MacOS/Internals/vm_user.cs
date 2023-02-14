using Invasion.Core.MacOS.Internals.Structs;

namespace Invasion.Core.MacOS.Internals;

#pragma warning disable IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal static unsafe partial class Native
{
    public const int KERN_SUCCESS = 0;
    public const int VM_REGION_BASIC_INFO_64 = 9;

    /// <summary>
    ///     Overwrite a range of the current map with data from the specified map/address range.
    /// </summary>
    /// <param name="map">The port for the task whose memory is to be read.</param>
    /// <param name="address">The address at which to start the read.</param>
    /// <param name="size">The number of bytes to read.</param>
    /// <param name="data">Out-pointer to dynamic array of bytes returned by the read.</param>
    /// <param name="data_size">The size returned in natural-sized units.</param>
    /// <returns>
    ///     <see cref="KERN_SUCCESS"/> if the function succeeds;
    ///     otherwise, the KERN error value.
    /// </returns>
    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial int mach_vm_read_overwrite(
        uint map,
        nint address,
        uint size,
        void* data,
        out uint data_size);

    /// <summary>
    ///     Overwrite the specified address range with the data provided.
    /// </summary>
    /// <param name="map">The port for the task whose memory is to be overwritten.</param>
    /// <param name="address">The address at which to start the write.</param>
    /// <param name="data">An array of data to be written.</param>
    /// <param name="size">The maximum size of the data.</param>
    /// <returns>
    ///     <see cref="KERN_SUCCESS"/> if the function succeeds;
    ///     otherwise, the KERN error value.
    /// </returns>
    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial int mach_vm_write(
        uint map,
        nint address,
        void* data,
        uint size);

    /// <summary>
    ///     User call to obtain information about a region in a task's address map.
    /// </summary>
    /// <param name="map">The port for the task whose address space contains the region.</param>
    /// <param name="address">The address at which to start looking for a region.</param>
    /// <param name="size">The number of bytes in the located region.</param>
    /// <param name="flavor">The type of information to be returned.</param>
    /// <param name="info">Returned region information.</param>
    /// <param name="count">The size returned in natural-sized units.</param>
    /// <param name="object_name">This parameter is no longer used.</param>
    /// <returns>
    ///     <see cref="KERN_SUCCESS"/> if the function succeeds;
    ///     otherwise, the KERN error value.
    /// </returns>
    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial int mach_vm_region(
        uint map,
        ref ulong address,
        out ulong size,
        int flavor,
        out vm_region_basic_info info,
        ref uint count,
        out uint object_name);
}
#pragma warning restore IDE1006
