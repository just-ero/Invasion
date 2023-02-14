using System.Diagnostics.CodeAnalysis;
using Invasion.Core.Models;

namespace Invasion.Core.Linux;

/// <summary>
///     The <see cref="ModuleMapsFileEntry"/> class
///     represents a module entry within the <c>proc/PID/maps</c> file of a Linux process.
/// </summary>
internal class ModuleMapsFileEntry : MapsFileEntry
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ModuleMapsFileEntry"/> class.
    /// </summary>
    /// <param name="start">The start address of the <see cref="ModuleMapsFileEntry"/>.</param>
    /// <param name="end">The end address of the <see cref="ModuleMapsFileEntry"/>.</param>
    /// <param name="protect">The <see cref="PageProtect"/> flags of the <see cref="ModuleMapsFileEntry"/>.</param>
    /// <param name="access">The <see cref="PageAccess"/> type of the <see cref="ModuleMapsFileEntry"/>.</param>
    /// <param name="path">The file path of the <see cref="ModuleMapsFileEntry"/>.</param>
    /// <param name="name">The file name of the <see cref="ModuleMapsFileEntry"/> excluding its trailing version number.</param>
    public ModuleMapsFileEntry(nint start, nint end, PageProtect protect, PageAccess access, string path, string name)
        : base(start, end, protect, access)
    {
        FilePath = path;
        Name = name;
    }

    /// <summary>
    ///     Gets the absolute file path of the <see cref="ModuleMapsFileEntry"/>.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    ///     Gets the file name of the <see cref="ModuleMapsFileEntry"/> excluding any trailing version numbers.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Tries to convert a line of the <c>proc/PID/maps</c> file into its <see cref="ModuleMapsFileEntry"/> equivalent.
    /// </summary>
    /// <param name="source">The line of the <c>proc/PID/maps</c> file to parse.</param>
    /// <param name="entry">The parsed <see cref="ModuleMapsFileEntry"/> if the method succeeds; otherwise, <see langword="null"/>.</param>
    /// <param name="hasFlags">
    ///     Spefies whether this <see cref="ModuleMapsFileEntry"/> contained the
    ///     <see cref="PageProtect.Read"/> flag and
    ///     either of the <see cref="PageProtect.Execute"/> or <see cref="PageAccess.Shared"/> flags.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse(string source, [NotNullWhen(true)] out ModuleMapsFileEntry? entry, out bool hasFlags)
    {
        string[] split = source.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        entry = default;
        hasFlags = default;

        if (split.Length < 6 || split[5][0] == '[')
        {
            return false;
        }

        if (!MapsFileEntry.TryParse(split, out MapsFileEntry? map))
        {
            return false;
        }

        string path = string.Concat(split[5..]);
        if (!TrySanitizeFileName(path, out string? name))
        {
            return false;
        }

        hasFlags = map.Protect.HasFlag(PageProtect.Read) && (map.Protect.HasFlag(PageProtect.Execute) || map.Access == PageAccess.Shared);
        entry = new(map.Start, map.End, map.Protect, map.Access, path, name);

        return true;
    }

    /// <summary>
    ///     Tries to convert the absolute file path of a process into its file name excluding any potential trailing version identifiers.
    /// </summary>
    /// <param name="path">The file path to convert.</param>
    /// <param name="name">The file name excluding any potential trailing version identifiers.</param>
    /// <returns>
    ///     <see langword="true"/> if the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    private static bool TrySanitizeFileName(string path, [NotNullWhen(true)] out string? name)
    {
        if (path.Contains("(deleted)"))
        {
            name = default;
            return false;
        }

        name = Path.GetFileName(path);

        int so = name.IndexOf(".so");
        if (so != -1)
        {
            name = name[..(so + 3)];
            return true;
        }

        int dll = name.IndexOf(".dll");
        if (dll != -1)
        {
            name = name[..(dll + 4)];
            return true;
        }

        int dylib = name.IndexOf(".dylib");
        if (dylib != -1)
        {
            name = name[..(dylib + 6)];
            return true;
        }

        return true;
    }
}
