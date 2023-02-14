using System.Buffers;
using Invasion.Core.Exceptions;
using Invasion.Core.Models;
using Invasion.Core.Windows.Internals.Enums;
using Invasion.Core.Windows.Internals.Structs;
using static Invasion.Core.Windows.Internals.Native;

namespace Invasion.Core.Windows;

/// <summary>
///     The <see cref="ProcessMemory"/> class
///     provides an interface for interacting with a remote process' memory space on Windows.
/// </summary>
[SupportedOSPlatform(OS.Windows)]
public sealed class ProcessMemory : ProcessMemoryBase
{
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

        if (!IsWow64Process(Process.Handle, out bool wow64Process))
        {
            ThrowHelper.Throw.Win32();
        }

        return Environment.Is64BitOperatingSystem && !wow64Process;
    }

    /// <inheritdoc/>
    public override unsafe List<Module> Modules
    {
        get
        {
            ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedModules);

            const int MAX_PATH = 260;

            nint hProcess = Process.Handle;

            if (!K32EnumProcessModulesEx(hProcess, null, 0, out uint cbNeeded, LIST_MODULES_ALL))
            {
                ThrowHelper.Throw.Win32();
            }

            int numModules = (int)(cbNeeded / sizeof(nint));
            nint[]? rented = null;
            Span<nint> hModule =
                cbNeeded < 1024
                ? stackalloc nint[128]
                : (rented = ArrayPool<nint>.Shared.Rent(numModules));

            fixed (nint* lphModule = hModule)
            {
                if (!K32EnumProcessModulesEx(hProcess, lphModule, cbNeeded, out _, LIST_MODULES_ALL))
                {
                    ThrowHelper.Throw.Win32();
                }
            }

            ushort* buffer = stackalloc ushort[MAX_PATH];
            List<Module> modules = new(numModules);

            for (int i = 0; i < numModules; i++)
            {
                if (K32GetModuleBaseNameW(hProcess, hModule[i], buffer, MAX_PATH) == 0)
                {
                    ThrowHelper.Throw.Win32();
                }

                string baseName = new((char*)buffer);

                if (K32GetModuleFileNameExW(hProcess, hModule[i], buffer, MAX_PATH) == 0)
                {
                    ThrowHelper.Throw.Win32();
                }

                string fileName = new((char*)buffer);

                if (!K32GetModuleInformation(hProcess, hModule[i], out MODULEINFO moduleInfo, MODULEINFO.SelfSize))
                {
                    ThrowHelper.Throw.Win32();
                }

                modules.Add(new(baseName, fileName, moduleInfo));
            }

            if (rented is not null)
            {
                ArrayPool<nint>.Shared.Return(rented);
            }

            return modules;
        }
    }

    /// <inheritdoc/>
    public override IEnumerable<MemoryPage> MemoryPages
    {
        get
        {
            ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedPages);

            nint addr = 0x10000, max = (nint)(Is64Bit ? 0x7FFFFFFEFFFF : 0x7FFEFFFF);

            while (VirtualQueryEx(Process.Handle, addr, out MEMORY_BASIC_INFORMATION mbi, MEMORY_BASIC_INFORMATION.SelfSize) != 0)
            {
                addr += (nint)mbi.RegionSize;

                if (mbi.State == MemState.MEM_COMMIT
                    && (mbi.Protect & MemProtect.PAGE_GUARD) != 0
                    && mbi.Type != MemType.MEM_PRIVATE)
                {
                    yield return new(mbi);
                }

                if (addr >= max)
                {
                    break;
                }
            }
        }
    }

    /// <inheritdoc/>
    public override unsafe bool Read(nint address, void* buffer, nuint bufferLen)
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedRead);

        if (!ReadProcessMemory(Process.Handle, address, buffer, bufferLen, out nuint bytesRead)
            || bytesRead != bufferLen)
        {
            ThrowHelper.Throw.Win32();
        }

        return true;
    }

    /// <inheritdoc/>
    public override unsafe bool Write(nint address, void* data, nuint dataLen)
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedRead);

        if (!WriteProcessMemory(Process.Handle, address, data, dataLen, out nuint bytesWritten)
            || bytesWritten != dataLen)
        {
            ThrowHelper.Throw.Win32();
        }

        return true;
    }
}
