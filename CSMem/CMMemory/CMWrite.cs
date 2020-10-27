using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CSMem.CMProtect;
using Microsoft.Win32.SafeHandles;

namespace CSMem.CMMemory
{
    internal class CMWrite : IDisposable
    {
        //Singleton
        private static CMWrite instance = null;
        private static readonly object padlock = new object();

        public static CMWrite Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CMWrite();
                        }
                    }
                }
                return instance;
            }
        }


        internal void WriteVirtualMemory(SafeProcessHandle processHandle,IntPtr baseAddress, byte[] bytesToWrite)
        {
            // Adjust the protection of the virtual memory region to ensure it has write privileges
            var originalProtectionType = CMMemoryProtect.Instance.ProtectVirtualMemory(processHandle, baseAddress, bytesToWrite.Length, CMMemoryProtect.MemoryProtection.ReadWrite);
            var bytesToWriteBufferHandle = GCHandle.Alloc(bytesToWrite, GCHandleType.Pinned);
            if (!Native.PInvoke.WriteProcessMemory(processHandle, baseAddress, bytesToWriteBufferHandle.AddrOfPinnedObject(), bytesToWrite.Length, IntPtr.Zero))
            {
                throw new Win32Exception($"Failed to write into another process memory region. with last error {Marshal.GetLastWin32Error()}");
            }
            // Restore the original protection of the virtual memory region
            CMMemoryProtect.Instance.ProtectVirtualMemory(processHandle, baseAddress, bytesToWrite.Length, originalProtectionType);
            //Free GC Allocated Handle
            bytesToWriteBufferHandle.Free();
        }

        internal void WriteVirtualMemory<TStructure>(SafeProcessHandle processHandle, IntPtr baseAddress, TStructure structureToWrite) where TStructure : struct
        {
            var structureSize = Marshal.SizeOf<TStructure>();
            // Adjust the protection of the virtual memory region to ensure it has write privileges
            var originalProtectionType = CMMemoryProtect.Instance.ProtectVirtualMemory(processHandle,baseAddress, structureSize, CMMemoryProtect.MemoryProtection.ReadWrite);
            var structureToWriteBufferHandle = GCHandle.Alloc(structureToWrite, GCHandleType.Pinned);
            if (!Native.PInvoke.WriteProcessMemory(processHandle, baseAddress, structureToWriteBufferHandle.AddrOfPinnedObject(), structureSize, IntPtr.Zero))
            {
                throw new Win32Exception($"Failed to write into another process memory region. with last error {Marshal.GetLastWin32Error()}");
            }
            // Restore the original protection of the virtual memory region
            CMMemoryProtect.Instance.ProtectVirtualMemory(processHandle, baseAddress, structureSize, originalProtectionType);
            structureToWriteBufferHandle.Free();
        }

        public void Dispose()
        {

        }
    }
}
