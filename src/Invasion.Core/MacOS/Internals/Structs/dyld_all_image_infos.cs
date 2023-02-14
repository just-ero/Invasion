namespace Invasion.Core.MacOS.Internals.Structs;

/// <summary>
///     Specifies which mach-o images are loaded in a process.
/// </summary>
#pragma warning disable CS0649, IDE1006
[SupportedOSPlatform(OS.MacOS)]
internal unsafe struct dyld_all_image_infos
{
    public uint version;
    public uint infoArrayCount;
    public dyld_image_info* infoArray;
    public void* notification;
    public byte processDetachedFromSharedRegion;
    public byte libSystemInitialized;
    public void* dyldImageLoadAddress;
    public void* jitInfo;
    public sbyte* dyldVersion;
    public sbyte* errorMessage;
    public nuint terminationFlags;
    public void* coreSymbolicationShmPage;
    public nuint systemOrderFlag;
    public nuint uuidArrayCount;
    public dyld_uuid_info* uuidArray;
    public dyld_all_image_infos* dyldAllImageInfosAddress;
    public nuint initialImageCount;
    public nuint errorKind;
    public sbyte* errorClientOfDylibPath;
    public sbyte* errorTargetDylibPath;
    public sbyte* errorSymbol;
    public nuint sharedCacheSlide;
}
#pragma warning restore IDE1006
