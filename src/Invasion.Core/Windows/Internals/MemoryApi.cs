using Invasion.Core.Windows.Internals.Structs;

namespace Invasion.Core.Windows.Internals;

[SupportedOSPlatform(OS.Windows)]
internal static unsafe partial class Native
{
    /// <summary>
    ///     Copies the data in the specified address range from the address space of
    ///     the specified process into the specified buffer of the current process.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-readprocessmemory">ReadProcessMemory function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A <see cref="Process.Handle"/> to the process with the memory that is being read.</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process from which to read.</param>
    /// <param name="lpBuffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
    /// <param name="nSize">The number of bytes to be read from the specified process.</param>
    /// <param name="lpNumberOfBytesRead">The number of bytes transferred into the specified buffer.</param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    [LibraryImport(Lib.Kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ReadProcessMemory(
        nint hProcess,
        nint lpBaseAddress,
        void* lpBuffer,
        nuint nSize,
        out nuint lpNumberOfBytesRead);

    /// <summary>
    ///     Writes data to an area of memory in a specified process.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-writeprocessmemory">WriteProcessMemory function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process memory to be modified.</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process to which data is written.</param>
    /// <param name="lpBuffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
    /// <param name="nSize">The number of bytes to be written to the specified process.</param>
    /// <param name="lpNumberOfBytesWritten">The number of bytes transferred into the specified process.</param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    [LibraryImport(Lib.Kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WriteProcessMemory(
        nint hProcess,
        nint lpBaseAddress,
        void* lpBuffer,
        nuint nSize,
        out nuint lpNumberOfBytesWritten);

    /// <summary>
    ///     Retrieves information about a range of pages within the virtual address space of a specified process.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualqueryex">VirtualQueryEx function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process whose memory information is queried.</param>
    /// <param name="lpAddress">A pointer to the base address of the region of pages to be queried.</param>
    /// <param name="lpBuffer">The <see cref="MEMORY_BASIC_INFORMATION"/> structure in which information about the specified page range is returned.</param>
    /// <param name="dwLength">The size of the buffer pointed to by <paramref name="lpBuffer"/>, in bytes.</param>
    /// <returns>
    ///     The actual number of bytes returned in the information buffer if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [LibraryImport(Lib.Kernel32, SetLastError = true)]
    public static partial nuint VirtualQueryEx(
        nint hProcess,
        nint lpAddress,
        out MEMORY_BASIC_INFORMATION lpBuffer,
        nuint dwLength);
}
