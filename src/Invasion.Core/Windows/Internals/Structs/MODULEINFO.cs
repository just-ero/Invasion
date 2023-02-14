﻿namespace Invasion.Core.Windows.Internals.Structs;

/// <summary>
///     Contains the module load address, size, and entry point.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/psapi/ns-psapi-moduleinfo">MODULEINFO structure on Microsoft docs</see></i>.
/// </remarks>
[SupportedOSPlatform(OS.Windows)]
internal unsafe struct MODULEINFO
{
    /// <summary>
    ///     The load address of the module.
    /// </summary>
#pragma warning disable IDE1006
    public nint lpBaseOfDll;
#pragma warning restore IDE1006

    /// <summary>
    ///     The size of the linear space that the module occupies, in bytes.
    /// </summary>
    public uint SizeOfImage;

    /// <summary>
    ///     The entry point of the module.
    /// </summary>
    public nint EntryPoint;

    public static uint SelfSize
    {
        get => (uint)sizeof(MODULEINFO);
    }
}
