using Invasion.Core.Exceptions;
using Invasion.Core.Linux.Internals.Structs;
using Invasion.Core.Models;
using Microsoft.Win32.SafeHandles;
using static Invasion.Core.Linux.Internals.Native;

namespace Invasion.Core.Linux;

/// <summary>
///     The <see cref="ProcessMemory"/> class
///     provides an interface for interacting with a remote process' memory space on Linux.
/// </summary>
[SupportedOSPlatform(OS.Linux)]
public sealed class ProcessMemory : ProcessMemoryBase
{
    private const string Proc = "/proc";
    private const string Maps = "maps";
    private const string Exe = "exe";

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProcessMemory"/> class
    ///     with the specified target process.
    /// </summary>
    /// <param name="process">The target process whose memory space is to be interacted with.</param>
    public ProcessMemory(Process process)
        : base(process) { }

    /// <inheritdoc/>
    protected override bool GetArchitecture()
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedArchitecture);

        Span<byte> buffer = stackalloc byte[5];

        using SafeFileHandle handle = File.OpenHandle($"{Proc}/{Process.Id}/{Exe}", FileMode.Open, FileAccess.Read, FileShare.Read);
        long bytesRead = RandomAccess.Read(handle, buffer, 0);

        if (bytesRead != buffer.Length)
        {
            string msg = "Unable to read the required amount of bytes.";
            ThrowHelper.Throw.EndOfStream(msg);
        }

        if (!buffer.StartsWith("\x007FELF"u8))
        {
            string msg = "Format of the file is invalid.";
            ThrowHelper.Throw.BadImageFormat(msg);
        }

        return buffer[4] switch
        {
            1 => false,
            2 => true,
            _ => throw new InvalidOperationException("Attempted to access a process with an unsupported architecture.")
        };
    }

    /// <inheritdoc/>
    public override List<Module> Modules
    {
        get
        {
            ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedModules);

            Dictionary<string, (Module Module, bool HasFlags)> modules = new();

            foreach (string line in File.ReadLines($"{Proc}/{Process.Id}/{Maps}"))
            {
                if (!ModuleMapsFileEntry.TryParse(line, out ModuleMapsFileEntry? map, out bool hasFlags))
                {
                    continue;
                }

                if (modules.TryGetValue(map.Name, out (Module Module, bool HasFlags) entry))
                {
                    entry.Module.Regions_Internal.Add(new(map.Start, map.Size));
                }
                else
                {
                    entry.Module = new(map.FilePath, map.Name, map.Start, map.Size);
                }

                modules[map.Name] = (entry.Module, entry.HasFlags |= hasFlags);
            }

            return modules.Values.Where(v => v.HasFlags).Select(v => v.Module).ToList();
        }
    }

    /// <inheritdoc/>
    public override IEnumerable<MemoryPage> MemoryPages
    {
        get
        {
            ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedPages);

            foreach (string line in File.ReadLines($"{Proc}/{Process.Id}/{Maps}"))
            {
                if (MapsFileEntry.TryParse(line, out MapsFileEntry? map))
                {
                    yield return new(map.Start, map.Size, map.Protect, map.Access);
                }
            }
        }
    }

    /// <inheritdoc/>
    public override unsafe bool Read(nint address, void* buffer, nuint bufferLen)
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedRead);

        iovec local_iov = new()
        {
            iov_base = buffer,
            iov_len = bufferLen
        };

        iovec remote_iov = new()
        {
            iov_base = (void*)address,
            iov_len = bufferLen
        };

        nint bytesRead = process_vm_readv(Process.Id, local_iov, 1, remote_iov, 1, 0);
        if (bytesRead == -1
            || (nuint)bytesRead != bufferLen)
        {
            ThrowHelper.Throw.Win32();
        }

        return true;
    }

    /// <inheritdoc/>
    public override unsafe bool Write(nint address, void* data, nuint dataLen)
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedWrite);

        iovec local_iov = new()
        {
            iov_base = data,
            iov_len = dataLen
        };

        iovec remote_iov = new()
        {
            iov_base = (void*)address,
            iov_len = dataLen
        };

        nint bytesWritten = process_vm_writev(Process.Id, local_iov, 1, remote_iov, 1, 0);
        if (bytesWritten == -1
            || (nuint)bytesWritten != dataLen)
        {
            ThrowHelper.Throw.Win32();
        }

        return (nuint)bytesWritten == dataLen;
    }
}
