namespace Invasion.Core.MacOS.Internals.Structs;

#pragma warning disable IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal unsafe struct vm_region_basic_info
{
    public int protection;
    public int max_protection;
    public uint inheritance;
    public byte shared;
    public byte reserved;
    public uint offset;
    public int behavior;
    public ushort user_wired_count;

    public static uint SelfSize
    {
        get => (uint)sizeof(vm_region_basic_info);
    }
}
#pragma warning restore IDE1006
