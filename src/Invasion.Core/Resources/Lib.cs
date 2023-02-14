namespace Invasion.Core.Resources;

/// <summary>
///     The <see cref="Lib"/> class
///     holds a number of different native library names.
/// </summary>
internal static class Lib
{
    // Windows
    public const string Kernel32 = "kernel32";
    public const string PSApi = "psapi";
    public const string DbgHelp = "dbghelp";

    // Unix
    public const string LibC = "libc";
}
