using Invasion.Core.Linux.Internals.Structs;

namespace Invasion.Core.Linux.Internals;

#pragma warning disable IDE1006
[SupportedOSPlatform(OS.Linux)]
internal static unsafe partial class Native
{
    /// <summary>
    ///     Copies the data in the specified address range from the address space of
    ///     the specified process into the specified buffer of the current process.<br/>
    ///     For further information see:
    ///     <i><see href="https://man7.org/linux/man-pages/man2/process_vm_readv.2.html">process_vm_readv(2)</see></i>.
    /// </summary>
    /// <param name="pid">A <see cref="Process.Id"/> to the process with the memory that is being read.</param>
    /// <param name="local_iov">A pointer to a buffer describing address ranges in the calling process.</param>
    /// <param name="liovcnt">The number of elements in <paramref name="local_iov"/>.</param>
    /// <param name="remote_iov">A pointer to a buffer describing address ranges in the process identified by <paramref name="pid"/>.</param>
    /// <param name="riovcnt">The number of elements in <paramref name="remote_iov"/>.</param>
    /// <param name="flags">Unused, must be set to 0.</param>
    /// <returns>
    ///     The number of bytes read when the function succeeds;
    ///     otherwise, -1.
    /// </returns>
    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial nint process_vm_readv(
        int pid,
        in iovec local_iov,
        ulong liovcnt,
        in iovec remote_iov,
        ulong riovcnt,
        ulong flags);

    /// <summary>
    ///     Writes data to an area of memory in a specified process.<br/>
    ///     For further information see:
    ///     <i><see href="https://man7.org/linux/man-pages/man2/process_vm_writev.2.html">process_vm_writev(2)</see></i>.
    /// </summary>
    /// <param name="pid">A <see cref="Process.Id"/> to the process with the memory that is being modified.</param>
    /// <param name="local_iov">A pointer to a buffer describing address ranges in the calling process.</param>
    /// <param name="liovcnt">The number of elements in <paramref name="local_iov"/>.</param>
    /// <param name="remote_iov">A pointer to a buffer describing address ranges in the process identified by <paramref name="pid"/>.</param>
    /// <param name="riovcnt">The number of elements in <paramref name="remote_iov"/>.</param>
    /// <param name="flags">Unused, must be set to 0.</param>
    /// <returns>
    ///     The number of bytes written when the function succeeds;
    ///     otherwise, -1.
    /// </returns>
    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial nint process_vm_writev(
        int pid,
        in iovec local_iov,
        ulong liovcnt,
        in iovec remote_iov,
        ulong riovcnt,
        ulong flags);
}
#pragma warning restore IDE1006
