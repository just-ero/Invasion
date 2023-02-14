using Invasion.Core.Exceptions;
using Invasion.Core.Models;

namespace Invasion.Core;

/// <summary>
///     The <see cref="ProcessMemoryBase"/> class
///     provides an interface for interacting with a remote process' memory space.
/// </summary>
[SupportedOSPlatform(OS.Linux)]
[SupportedOSPlatform(OS.MacOS)]
[SupportedOSPlatform(OS.Windows)]
public abstract class ProcessMemoryBase : IDisposable
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProcessMemoryBase"/> class
    ///     with the specified target process.
    /// </summary>
    /// <param name="process">The target process whose memory space is to be interacted with.</param>
    protected ProcessMemoryBase(Process process)
    {
        Process = process;
        Is64Bit = GetArchitecture();
        PointerSize = (byte)(Is64Bit ? 0x8 : 0x4);
    }

    /// <summary>
    ///     Initializes a new <see cref="ProcessMemoryBase"/> depending on the current operating system.
    /// </summary>
    /// <param name="process">The target process whose memory space is to be interacted with.</param>
    /// <returns>
    ///     <see cref="Windows.ProcessMemory"/> when the current operating system is Windows;<br/>
    ///     <see cref="Linux.ProcessMemory"/> when the current operating system is Linux;<br/>
    ///     <see cref="MacOS.ProcessMemory"/> when the current operating system is MacOS.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the provided <see cref="System.Diagnostics.Process"/>'
    ///     <see cref="Process.HasExited"/> property is <see langword="true"/>.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    ///     Thrown when the current operating system is not Windows, Linux, or MacOSX.
    /// </exception>
    public static ProcessMemoryBase Init(Process process)
    {
        if (process.HasExited)
        {
            ThrowHelper.Throw.Argument(nameof(process), IR.ProcessMustNotHaveExited);
        }

        if (OperatingSystem.IsWindows())
        {
            return new Windows.ProcessMemory(process);
        }
        else if (OperatingSystem.IsLinux())
        {
            return new Linux.ProcessMemory(process);
        }
        else if (OperatingSystem.IsMacOS())
        {
            return new MacOS.ProcessMemory(process);
        }

        throw new PlatformNotSupportedException(IR.SupportedOperatingSystems);
    }

    /// <summary>
    ///     Gets the target <see cref="System.Diagnostics.Process"/> object the <see cref="ProcessMemoryBase"/> class
    ///     is hooked to.
    /// </summary>
    public Process Process { get; }

    /// <summary>
    ///     Gets the architecture of <see cref="Process"/>.
    /// </summary>
    public bool Is64Bit { get; }

    /// <summary>
    ///     Gets the size a pointer has in <see cref="Process"/>.
    /// </summary>
    /// <value>
    ///     Returns <c>0x8</c> when <see cref="Is64Bit"/> is <see langword="true"/>;
    ///     otherwise, <c>0x4</c>.
    /// </value>
    public byte PointerSize { get; }

    /// <summary>
    ///     Infers the architecture of <see cref="Process"/>
    ///     and sets its return value to <see cref="Is64Bit"/>.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> when <see cref="Process"/> is a 64-bit process;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    protected abstract bool GetArchitecture();

    /// <summary>
    ///     Gets the list of modules currently loaded by <see cref="Process"/>.
    /// </summary>
    public abstract List<Module> Modules { get; }

    /// <summary>
    ///     Enumerates the virtual memory maps within <see cref="Process"/>.
    /// </summary>
    public abstract IEnumerable<MemoryPage> MemoryPages { get; }

    /// <summary>
    ///     Copies data from <see cref="Process"/>'s memory into a specified buffer.
    /// </summary>
    /// <param name="address">The base address in <see cref="Process"/>'s memory from which to read the data.</param>
    /// <param name="buffer">A pointer to the buffer that receives the data.</param>
    /// <param name="bufferLen">The number of bytes to be read from <see cref="Process"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public abstract unsafe bool Read(nint address, void* buffer, nuint bufferLen);

    /// <summary>
    ///     Copies data from a specified buffer to <see cref="Process"/>'s memory.
    /// </summary>
    /// <param name="address">The base address in <see cref="Process"/>'s memory to which to write the data.</param>
    /// <param name="data">A pointer to the buffer that contains data to be written</param>
    /// <param name="dataLen">The number of bytes to be written to <see cref="Process"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if the method succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public abstract unsafe bool Write(nint address, void* data, nuint dataLen);

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Process.Dispose();
    }
}
