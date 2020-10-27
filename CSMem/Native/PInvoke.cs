using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using static CSMem.CMProtect.CMMemoryProtect;

namespace CSMem.Native
{
    internal class PInvoke
    {
        //Kernel32 Windows API Import

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualProtectEx(SafeProcessHandle processHandle, IntPtr baseAddress, int protectionSize, MemoryProtection protectionType, out MemoryProtection oldProtectionType);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, IntPtr bytesReadBuffer, int bytesToRead, IntPtr numberOfBytesReadBuffer);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(SafeProcessHandle processHandle, IntPtr baseAddress, IntPtr bufferToWrite, int bytesToWriteSize, IntPtr numberOfBytesWrittenBuffer);

        [DllImport("kernel32.dll")]
        internal static extern bool IsWow64Process(SafeProcessHandle processHandle, out bool isWow64Process);
    }
}
