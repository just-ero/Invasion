namespace Invasion.Core.MacOS.Internals.Structs;

#pragma warning disable CS0649, IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal struct mach_header
{
    public uint magic;
    public int cputype;
    public int cpusubtype;
    public uint filetype;
    public uint ncmds;
    public uint sizeofcmds;
    public uint flags;
}

[SupportedOSPlatform(OS.MacOS)]
internal struct mach_header_64
{
    public uint magic;
    public int cputype;
    public int cpusubtype;
    public uint filetype;
    public uint ncmds;
    public uint sizeofcmds;
    public uint flags;
    public uint reserved;
}
#pragma warning restore CS0649, IDE1006
