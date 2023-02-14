using System.Collections.ObjectModel;
using Invasion.Core.Windows.Internals.Structs;

namespace Invasion.Core.Models;

/// <summary>
///     The <see cref="Module"/> class
///     represents a module loaded by a process.
/// </summary>
public sealed class Module
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Module"/> class on Windows.
    /// </summary>
    [SupportedOSPlatform(OS.Windows)]
    internal Module(string baseName, string fileName, MODULEINFO moduleInfo)
    {
        Name = baseName;
        FilePath = fileName;
        Regions_Internal.Add(new(moduleInfo.lpBaseOfDll, (int)moduleInfo.SizeOfImage));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Module"/> class on Linux.
    /// </summary>
    [SupportedOSPlatform(OS.Linux)]
    internal Module(string filePath, string name, nint baseAddress, int size)
    {
        FilePath = filePath;
        Name = name;
        Regions_Internal.Add(new(baseAddress, size));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Module"/> class on MacOS.
    /// </summary>
    [SupportedOSPlatform(OS.MacOS)]
    internal Module(nint baseAddress, int size, string filePath)
    {
        Name = Path.GetFileName(filePath);
        FilePath = filePath;
        Regions_Internal.Add(new(baseAddress, size));
    }

    /// <summary>
    ///     Gets the file name of the <see cref="Module"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the fully qualified file path of the <see cref="Module"/>.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    ///     Gets the memory address where the <see cref="Module"/> was loaded.
    /// </summary>
    public nint Base
    {
        get => Regions_Internal[0].Base;
    }

    /// <summary>
    ///     Gets the amount of memory required to load the <see cref="Module"/>, in bytes.
    /// </summary>
    public int MemorySize
    {
        get => Regions_Internal.Sum(region => region.Size);
    }

    /// <summary>
    ///     Acts as an <see langword="internal"/> backing field for <see cref="Regions"/>.
    /// </summary>
    internal List<MemoryRegion> Regions_Internal { get; } = new();

    /// <summary>
    ///     Gets all regions occupied by the <see cref="Module"/>.
    /// </summary>
    public ReadOnlyCollection<MemoryRegion> Regions
    {
        get => Regions_Internal.AsReadOnly();
    }

    /// <summary>
    ///     Gets version information about the <see cref="Module"/>.
    /// </summary>
    public FileVersionInfo VersionInfo
    {
        get => FileVersionInfo.GetVersionInfo(FilePath);
    }

    /// <summary>
    ///     Formats the values of the <see cref="Module"/> into a coherent <see cref="string"/>.
    /// </summary>
    public override string ToString()
    {
        return string.Join(Environment.NewLine, Regions.Select(r => $"{r.Base:X16}-{r.Base + r.Size:X16} {Name}"));
    }
}
