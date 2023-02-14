namespace Invasion.Core.MacOS.Internals;

#pragma warning disable IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal static unsafe partial class Native
{
    public const uint MACH_PORT_NULL = 0;

    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial int task_for_pid(
        uint target_tport,
        int pid,
        out uint t);

    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial int mach_task_self();
}
#pragma warning restore IDE1006
