namespace Invasion.Core.MacOS.Internals.Structs;

/// <summary>
///     The <see cref="task_dyld_info"/> struct holds information about the dynamic loader.<br/>
///     For further information, see:
///     <i><see href="https://github.com/apple-oss-distributions/xnu/blob/main/osfmk/mach/task_info.h#L281-L285">task_info.h</see></i>
/// </summary>
#pragma warning disable IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal struct task_dyld_info
{
    public ulong all_image_info_addr;
    public ulong all_image_info_size;
    public int all_image_info_format;
}
#pragma warning restore IDE1006
