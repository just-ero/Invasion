using Invasion.Core.Windows.Internals.Structs;

namespace Invasion.Core.Windows.Internals;

[SupportedOSPlatform(OS.Windows)]
internal static unsafe partial class Native
{
    /// <summary>
    ///     List all modules.
    /// </summary>
    public const int LIST_MODULES_ALL = 3;

    /// <summary>
    ///     Retrieves a handle for each module in the specified process that meets the specified filter criteria.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-enumprocessmodulesex">EnumProcessModulesEx function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process.</param>
    /// <param name="lphModule">An array that receives the list of module handles.</param>
    /// <param name="cb">The size of the lphModule array, in bytes.</param>
    /// <param name="lpcbNeeded">The number of bytes required to store all module handles in the lphModule array.</param>
    /// <param name="dwFilterFlag">The filter criteria.</param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    [LibraryImport(Lib.PSApi, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool K32EnumProcessModulesEx(
        nint hProcess,
        nint* lphModule,
        uint cb,
        out uint lpcbNeeded,
        uint dwFilterFlag);

    /// <summary>
    ///     Retrieves the base name of the specified module.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmodulebasenamew">GetModuleBaseNameW function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that contains the module.</param>
    /// <param name="hModule">A handle to the module.</param>
    /// <param name="lpBaseName">A pointer to the buffer that receives the base name of the module.</param>
    /// <param name="nSize">The size of the <paramref name="lpBaseName"/> buffer, in characters.</param>
    /// <returns>
    ///     The length of the string copied to the buffer, in characters, if the function succeeds;
    ///     otherwise, zero.
    /// </returns>
    [LibraryImport(Lib.PSApi, SetLastError = true)]
    public static partial uint K32GetModuleBaseNameW(
        nint hProcess,
        nint hModule,
        ushort* lpBaseName,
        uint nSize);

    /// <summary>
    ///     Retrieves the fully qualified path for the file containing the specified module.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmodulefilenameexw">GetModuleFileNameExW function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that contains the module.</param>
    /// <param name="hModule">A handle to the module.</param>
    /// <param name="lpFilename">A pointer to a buffer that receives the fully qualified path to the module.</param>
    /// <param name="nSize">The size of the <paramref name="lpFilename"/> buffer, in characters.</param>
    /// <returns>
    ///     The length of the string copied to the buffer, in characters, if the function succeeds;
    ///     otherwise, zero.
    /// </returns>
    [LibraryImport(Lib.PSApi, SetLastError = true)]
    public static partial uint K32GetModuleFileNameExW(
        nint hProcess,
        nint hModule,
        ushort* lpFilename,
        uint nSize);

    /// <summary>
    ///     Retrieves information about the specified module in the <see cref="MODULEINFO"/> structure.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmoduleinformation">GetModuleInformation function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that contains the module.</param>
    /// <param name="hModule">A handle to the module.</param>
    /// <param name="lpmodinfo">A pointer to the <see cref="MODULEINFO"/> structure that receives information about the module.</param>
    /// <param name="cb">The size of the <see cref="MODULEINFO"/> structure, in bytes.</param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    [LibraryImport(Lib.PSApi, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool K32GetModuleInformation(
        nint hProcess,
        nint hModule,
        out MODULEINFO lpmodinfo,
        uint cb);
}
