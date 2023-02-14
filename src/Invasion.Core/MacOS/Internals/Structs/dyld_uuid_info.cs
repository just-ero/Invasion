namespace Invasion.Core.MacOS.Internals.Structs;

#pragma warning disable CS0649, IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal unsafe struct dyld_uuid_info
{
    public void* imageLoadAddress;
    public fixed byte imageUUID[16];
}
#pragma warning restore CS0649, IDE1006
