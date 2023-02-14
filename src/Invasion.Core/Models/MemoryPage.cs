using Invasion.Core.MacOS.Internals.Structs;
using Invasion.Core.Windows.Internals.Enums;
using Invasion.Core.Windows.Internals.Structs;

namespace Invasion.Core.Models;

/// <summary>
///     The <see cref="MemoryPage"/> class
///     represents an entry in the virtual memory space of a process.
/// </summary>
public sealed class MemoryPage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryPage"/> class on Windows.
    /// </summary>
    [SupportedOSPlatform(OS.Windows)]
    internal MemoryPage(MEMORY_BASIC_INFORMATION mbi)
    {
        unsafe
        {
            Base = (nint)mbi.BaseAddress;
        }

        MemorySize = (int)mbi.RegionSize;

        // TODO: Verify these!

        Protection = mbi.Protect switch
        {
            MemProtect.PAGE_READONLY => PageProtect.Read,
            MemProtect.PAGE_READWRITE => PageProtect.Read | PageProtect.Write,
            MemProtect.PAGE_WRITECOPY => PageProtect.Write,
            MemProtect.PAGE_EXECUTE => PageProtect.Execute,
            MemProtect.PAGE_EXECUTE_READ => PageProtect.Read | PageProtect.Execute,
            MemProtect.PAGE_EXECUTE_READWRITE => PageProtect.Read | PageProtect.Write | PageProtect.Execute,
            MemProtect.PAGE_EXECUTE_WRITECOPY => PageProtect.Write | PageProtect.Execute,
            _ => PageProtect.NoAccess
        };

        Access = mbi.Type switch
        {
            MemType.MEM_PRIVATE => PageAccess.Private,
            _ => PageAccess.Shared
        };
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryPage"/> class on Linux.
    /// </summary>
    [SupportedOSPlatform(OS.Linux)]
    internal MemoryPage(nint baseAddress, int size, PageProtect protect, PageAccess access)
    {
        Base = baseAddress;
        MemorySize = size;
        Protection = protect;
        Access = access;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryPage"/> class on MacOS.
    /// </summary>
    [SupportedOSPlatform(OS.MacOS)]
    internal MemoryPage(nint address, ulong size, vm_region_basic_info info)
    {
        Base = address;
        MemorySize = (int)size;

        // TODO: Need to figure out how protection works!
    }

    /// <summary>
    ///     Gets the base address of the <see cref="MemoryPage"/>.
    /// </summary>
    public nint Base { get; }

    /// <summary>
    ///     Gets the total size the <see cref="MemoryPage"/> occupies in the process, in bytes.
    /// </summary>
    public int MemorySize { get; }

    /// <summary>
    ///     Gets the <see cref="PageProtect"/> flags of the <see cref="MemoryPage"/>.
    /// </summary>
    public PageProtect Protection { get; }

    /// <summary>
    ///     Gets the <see cref="PageAccess"/> modifier of the <see cref="MemoryPage"/>.
    /// </summary>
    public PageAccess Access { get; }

    /// <summary>
    ///     Formats the values of the <see cref="MemoryPage"/> into a coherent <see cref="string"/>.
    /// </summary>
    public override string ToString()
    {
        return $"{Base:X16}-{Base + MemorySize:X16} ({Access} {Protection})";
    }
}
