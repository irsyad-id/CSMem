using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CSMem.CMMemory
{
    internal class CMRead : IDisposable
    {
        //Singleton
        private static CMRead instance = null;
        private static readonly object padlock = new object();

        public static CMRead Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CMRead();
                        }
                    }
                }
                return instance;
            }
        }

        public byte[] ReadVirtualMemory(SafeProcessHandle processHandle, IntPtr baseAddress, int bytesToRead)
        {
            var bytesBuffer = Marshal.AllocHGlobal(bytesToRead);

            if (!Native.PInvoke.ReadProcessMemory(processHandle, baseAddress, bytesBuffer, bytesToRead, IntPtr.Zero))
            {
                throw new Win32Exception($"Failed to read memory region from another process. with last error {Marshal.GetLastWin32Error()}");
            }
            var bytesRead = new byte[bytesToRead];
            Marshal.Copy(bytesBuffer, bytesRead, 0, bytesToRead);
            Marshal.FreeHGlobal(bytesBuffer);
            return bytesRead;
        }

        public TStructure ReadVirtualMemory<TStructure>(SafeProcessHandle processHandle, IntPtr baseAddress) where TStructure : struct
        {
            var structureSize = Marshal.SizeOf<TStructure>();
            var structureBuffer = Marshal.AllocHGlobal(structureSize);
            if (!Native.PInvoke.ReadProcessMemory(processHandle, baseAddress, structureBuffer, structureSize, IntPtr.Zero))
            {
                throw new Win32Exception($"Failed to read memory region from another process. with last error {Marshal.GetLastWin32Error()}");
            }
            try
            {
                return Marshal.PtrToStructure<TStructure>(structureBuffer);
            }
            finally
            {
                Marshal.FreeHGlobal(structureBuffer);
            }
        }
              
        public void Dispose()
        {

        }
    }
}
