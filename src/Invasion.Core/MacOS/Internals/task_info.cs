using Invasion.Core.MacOS.Internals.Structs;

namespace Invasion.Core.MacOS.Internals;

#pragma warning disable IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal static unsafe partial class Native
{
    public const int TASK_DYLD_INFO = 17;

    /// <summary>
    ///     The <see cref="mach_header"/> magic number.
    /// </summary>
    public const uint MH_MAGIC = 0xFEEDFACE;

    /// <summary>
    ///     The <see cref="mach_header_64"/> magic number.
    /// </summary>
    public const uint MH_MAGIC_64 = 0xFEEDFACF;

    /// <summary>
    ///     Gets the mach-o size of the <see cref="task_dyld_info"/> struct.
    /// </summary>
    public static readonly uint TASK_DYLD_INFO_COUNT = (uint)(sizeof(task_dyld_info) / sizeof(uint));

    /// <summary>
    ///     Return per-task information according to specified flavor.<br/>
    ///     For further information, see:
    ///     <i><see href="https://web.mit.edu/darwin/src/modules/xnu/osfmk/man/task_info.html">task_info on MIT.edu</see></i>.
    /// </summary>
    /// <param name="task">The port for the task for which the information is to be returned.</param>
    /// <param name="flavor">The type of information to be returned</param>
    /// <param name="task_info_out">Information about the specified task.</param>
    /// <param name="task_info_count">The size returned in natural-sized units.</param>
    /// <returns>
    ///     <see cref="KERN_SUCCESS"/> if the function succeeds;
    ///     otherwise, the KERN error value.
    /// </returns>
    [LibraryImport(Lib.LibC, SetLastError = true)]
    public static partial int task_info(
        uint task,
        uint flavor,
        out task_dyld_info task_info_out,
        in uint task_info_count);
}
#pragma warning restore IDE1006
