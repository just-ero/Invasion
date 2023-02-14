namespace Invasion.Core.Linux.Internals.Structs;

/// <summary>
///     Contains information about a data buffer for reading and writing
///     memory in another process.
/// </summary>
#pragma warning disable CS8981, IDE1006
[SupportedOSPlatform(OS.Linux)]
internal unsafe struct iovec
{
    /// <summary>
    ///     The starting address of the buffer.
    /// </summary>
    public void* iov_base;

    /// <summary>
    ///     The number of bytes to transfer.
    /// </summary>
    public nuint iov_len;
}
#pragma warning restore CS8981, IDE1006
