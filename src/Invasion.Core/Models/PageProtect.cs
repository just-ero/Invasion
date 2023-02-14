namespace Invasion.Core.Models;

/// <summary>
///     Provides protection flags for the <see cref="MemoryPage"/> type.
/// </summary>
[Flags]
public enum PageProtect
{
    /// <summary>
    ///     The page disallows all access.
    /// </summary>
    NoAccess,

    /// <summary>
    ///     The page allows read access.
    /// </summary>
    Read,

    /// <summary>
    ///     The page allows write access.
    /// </summary>
    Write,

    /// <summary>
    ///     The page allows execute access.
    /// </summary>
    Execute
}
