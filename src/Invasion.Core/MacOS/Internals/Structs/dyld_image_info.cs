namespace Invasion.Core.MacOS.Internals.Structs;

#pragma warning disable CS0649, IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal unsafe struct dyld_image_info
{
    public void* imageLoadAddress;
    public sbyte* imageFilePath;
    public nuint imageFileModDate;
}
#pragma warning restore CS0649, IDE1006
