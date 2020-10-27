using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMem.CMMemory
{
    public class CMMultiPointer
    {
        //Singleton
        private static CMMultiPointer instance = null;
        private static readonly object padlock = new object();

        public static CMMultiPointer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CMMultiPointer();
                        }
                    }
                }
                return instance;
            }
        }

        public IntPtr FindMultiPointerAddress(SafeProcessHandle safeProcessHandle, IntPtr baseAddress, List<int> offsets)
        {
            var pointer = baseAddress;
            // Determine if the process is running under Wow64 (x86)
            Native.PInvoke.IsWow64Process(safeProcessHandle, out var isWow64);
            foreach (var offset in offsets)
            {
                // If the process is x86
                if (isWow64)
                {
                    // Read the next address (next multi level pointer) from the current address

                    pointer = (IntPtr)CMRead.Instance.ReadVirtualMemory<int>(safeProcessHandle, pointer);
                }
                // If the process is x64
                else
                {
                    // Read the next address (next multi level pointer) from the current address
                    pointer = (IntPtr)CMRead.Instance.ReadVirtualMemory<long>(safeProcessHandle, pointer);
                }
                // Add the next offset onto the address
                pointer += offset;
            }
            return pointer;
        }
    }
}
