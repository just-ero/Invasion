using System;
using Invasion.Core.Exceptions;
using Invasion.Core.Linux.Internals.Structs;
using Invasion.Core.MacOS.Internals.Structs;
using Invasion.Core.Models;
using Microsoft.Win32.SafeHandles;
using static Invasion.Core.MacOS.Internals.Native;

namespace Invasion.Core.MacOS;

/// <summary>
///     The <see cref="ProcessMemory"/> class
///     provides an interface for interacting with a remote process' memory space on Linux.
/// </summary>
[SupportedOSPlatform(OS.MacOS)]
public sealed class ProcessMemory : ProcessMemoryBase
{
    private readonly uint _machTask;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProcessMemory"/> class
    ///     with the specified target process.
    /// </summary>
    /// <param name="process">The target process whose memory space is to be interacted with.</param>
    public ProcessMemory(Process process)
        : base(process)
    {
        int mach_task_self_ = mach_task_self();
        if (mach_task_self_ <= 0)
        {
            ThrowHelper.Throw.Win32();
        }

        int kern_return = task_for_pid((uint)mach_task_self_, process.Id, out uint t);
        if (kern_return != KERN_SUCCESS)
        {
            ThrowHelper.Throw.Win32();
        }

        _machTask = t;
    }

    /// <inheritdoc/>
    protected override bool GetArchitecture()
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedArchitecture);

        return true;
    }

    /// <inheritdoc/>
    public override unsafe List<Module> Modules
    {
        get
        {
            ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedModules);

            int kern_return = task_info(_machTask, TASK_DYLD_INFO, out task_dyld_info info, TASK_DYLD_INFO_COUNT);
            if (kern_return != KERN_SUCCESS)
            {
                ThrowHelper.Throw.Win32();
            }

            List<Module> modules = new();

            dyld_all_image_infos* all_image_infos = (dyld_all_image_infos*)(nuint)info.all_image_info_addr;
            dyld_image_info* infos = all_image_infos->infoArray;
            uint infosCount = all_image_infos->infoArrayCount;

            for (uint i = 0; i < infosCount; i++)
            {
                dyld_image_info* image = infos + i;
                if (Marshal.PtrToStringUTF8((nint)image->imageFilePath) is not string path)
                {
                    continue;
                }

                int size = (int)((mach_header*)image->imageLoadAddress)->sizeofcmds;
                size += *(uint*)image->imageLoadAddress == MH_MAGIC_64 ? sizeof(mach_header_64) : sizeof(mach_header);

                modules.Add(new((nint)image->imageLoadAddress, size, path));
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

            uint count = vm_region_basic_info.SelfSize;
            ulong address = 1;

            if (mach_vm_region(_machTask, ref address, out ulong size, VM_REGION_BASIC_INFO_64, out vm_region_basic_info info, ref count, out _) != KERN_SUCCESS)
            {
                ThrowHelper.Throw.Win32();
            }

            do
            {
                yield return new((nint)address, size, info);
                address += size;
            } while (mach_vm_region((uint)Process.Handle, ref address, out size, VM_REGION_BASIC_INFO_64, out info, ref count, out _) != KERN_SUCCESS);
        }
    }

    /// <inheritdoc/>
    public override unsafe bool Read(nint address, void* buffer, nuint bufferLen)
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedRead);

        if (mach_vm_read_overwrite(_machTask, address, (uint)bufferLen, buffer, out uint bytesRead) != KERN_SUCCESS
            || bytesRead != (uint)bufferLen)
        {
            ThrowHelper.Throw.Win32();
        }

        return true;
    }

    /// <inheritdoc/>
    public override unsafe bool Write(nint address, void* data, nuint dataLen)
    {
        ThrowHelper.ThrowIfProcessExited(Process, IR.ProcessExitedWrite);

        if (mach_vm_write(_machTask, address, data, (uint)dataLen) != KERN_SUCCESS)
        {
            ThrowHelper.Throw.Win32();
        }

        return true;
    }
}
