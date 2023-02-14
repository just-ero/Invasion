/* 
 * This class is largly inspired by Benjamin Moir's DetourSharp.
 * DetourSharp is licensed under the MIT License.
 * 
 * https://github.com/DaZombieKiller
 * https://github.com/DetourSharp/DetourSharp
 * https://github.com/DetourSharp/DetourSharp/blob/master/DetourSharp/Unix/MapsFileEntry.cs
 * https://github.com/DetourSharp/DetourSharp/blob/master/LICENSE.md
 */

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Invasion.Core.Models;

namespace Invasion.Core.Linux;

/// <summary>
///     The <see cref="MapsFileEntry"/> class
///     represents an entry within the <c>proc/PID/maps</c> file of a Linux process.
/// </summary>
internal class MapsFileEntry
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MapsFileEntry"/> class.
    /// </summary>
    /// <param name="start">The start address of the <see cref="MapsFileEntry"/>.</param>
    /// <param name="end">The end address of the <see cref="MapsFileEntry"/>.</param>
    /// <param name="protect">The <see cref="PageProtect"/> flags of the <see cref="MapsFileEntry"/>.</param>
    /// <param name="access">The <see cref="PageAccess"/> type of the <see cref="MapsFileEntry"/>.</param>
    public MapsFileEntry(nint start, nint end, PageProtect protect, PageAccess access)
    {
        Start = start;
        End = end;
        Protect = protect;
        Access = access;
    }

    /// <summary>
    ///     Gets the start address of the <see cref="MapsFileEntry"/>.
    /// </summary>
    public nint Start { get; }

    /// <summary>
    ///     Gets the end address of the <see cref="MapsFileEntry"/>.
    /// </summary>
    public nint End { get; }

    /// <summary>
    ///     Gets the total amount of bytes the <see cref="MapsFileEntry"/> occupies.
    /// </summary>
    public int Size
    {
        get => (int)(End - Start);
    }

    /// <summary>
    ///     Gets the <see cref="PageProtect"/> flags of the <see cref="MapsFileEntry"/>.
    /// </summary>
    public PageProtect Protect { get; }

    /// <summary>
    ///     Gets the <see cref="PageAccess"/> type of the <see cref="MapsFileEntry"/>.
    /// </summary>
    public PageAccess Access { get; }

    /// <summary>
    ///     Tries to convert a line of the <c>proc/PID/maps</c> file into its <see cref="MapsFileEntry"/> equivalent.
    /// </summary>
    /// <param name="source">The line of the <c>proc/PID/maps</c> file to parse.</param>
    /// <param name="entry">The parsed <see cref="MapsFileEntry"/> if the method succeeds; otherwise, <see langword="null"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse(string source, [NotNullWhen(true)] out MapsFileEntry? entry)
    {
        string[] split = source.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return TryParse(split, out entry);
    }

    /// <summary>
    ///     Tries to convert a line of the <c>proc/PID/maps</c> file into its <see cref="MapsFileEntry"/> equivalent.
    /// </summary>
    /// <param name="source">The line of the <c>proc/PID/maps</c> file to parse, split on whitespace characters.</param>
    /// <param name="entry">The parsed <see cref="MapsFileEntry"/> if the method succeeds; otherwise, <see langword="null"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    protected static bool TryParse(string[] source, [NotNullWhen(true)] out MapsFileEntry? entry)
    {
        entry = default;

        if (source.Length < 5)
        {
            return false;
        }

        PageProtect protect = default;
        PageAccess access = default;

        int index = source[0].IndexOf('-');
        if (index == -1
            || !ulong.TryParse(source[0][..index++], NumberStyles.HexNumber, null, out ulong start)
            || !ulong.TryParse(source[0][index..], NumberStyles.HexNumber, null, out ulong end))
        {
            return false;
        }

        if (source[1].Length != 4)
        {
            return false;
        }

        for (int i = 0; i < 4; i++)
        {
            switch (source[1][i])
            {
                case 'r':
                    protect |= PageProtect.Read;
                    break;
                case 'w':
                    protect |= PageProtect.Write;
                    break;
                case 'x':
                    protect |= PageProtect.Execute;
                    break;
                case 'p':
                    access = PageAccess.Private;
                    break;
                case 's':
                    access = PageAccess.Shared;
                    break;
            }
        }

        entry = new((nint)start, (nint)end, protect, access);
        return true;
    }
}
