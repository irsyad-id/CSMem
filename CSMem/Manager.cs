using CSMem.CMMemory;
using CSMem.CMProcess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSMem
{
    /// <summary>
    /// Library Manager for managing memory inside another process.
    /// </summary>
    public class Manager : IDisposable
    {
        CMProcess.Handle handle = new CMProcess.Handle();
        CMRead readMemory = new CMRead();
        CMWrite writeMemory = new CMWrite();

        /// <summary>
        /// Check Platform Information.
        /// </summary>
        public static bool IsWinPlatform = CheckPlatform("This library is intended for Windows use only.");
        public static bool CheckPlatform(string s)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException(s);
            }
            return true;
        }

        
        /// <summary>
        /// Create a instance of Manager class with another process pid as parameter.
        /// </summary>
        public Manager(int iProcessId)
        {
            handle.GetSafeProcessHandleById(iProcessId);
        }

        /// <summary>
        /// Create a instance of Manager class with another process name as parameter.
        /// </summary>
        public Manager(string sProcessName, bool wait=false)
        {
            if (!wait)
                handle.GetSafeProcessHandleByName(sProcessName);
            else
                handle.WaitForSafeProcessHandleByName(sProcessName);
        }


        /// <summary>
        /// Reads an array of bytes from a region of virtual memory in the remote process
        /// </summary>
        public byte[] ReadVirtualMemory(IntPtr baseAddress, int bytesToRead)
        {
            return readMemory.ReadVirtualMemory(handle.ProcessHandle, baseAddress, bytesToRead);
        }
        /// <summary>
        /// Reads a structure from a region of virtual memory in the remote process
        /// </summary>
        public TStructure ReadVirtualMemory<TStructure>(IntPtr baseAddress) where TStructure : struct
        {
            return readMemory.ReadVirtualMemory<TStructure>(handle.ProcessHandle, baseAddress);
        }
        /// <summary>
        /// Reads a structure from a region of virtual memory in the remote process
        /// </summary>
        public TStructure ReadVirtualMemory<TStructure>(string moduleName, int relativeOffset) where TStructure : struct
        {
            ProcessModule mod =Module.Instance.GetModuleByName(moduleName);
            return readMemory.ReadVirtualMemory<TStructure>(handle.ProcessHandle, mod.BaseAddress + relativeOffset);
        }
        /// <summary>
        /// Reads a structure from a region of virtual memory in the remote process (Multi Pointer)
        /// </summary>
        public TStructure ReadVirtualMemory<TStructure>(string moduleName, int relativeOffset, List<int> offsets) where TStructure : struct
        {
            ProcessModule mod = Module.Instance.GetModuleByName(moduleName);
            IntPtr multiPointerAddress = CMMultiPointer.Instance.FindMultiPointerAddress(handle.ProcessHandle, mod.BaseAddress + relativeOffset, offsets);
            return readMemory.ReadVirtualMemory<TStructure>(handle.ProcessHandle, multiPointerAddress);
        }
        /// <summary>
        /// Reads a structure from a region of virtual memory in the remote process (Multi Pointer)
        /// </summary>
        public TStructure ReadVirtualMemory<TStructure>(IntPtr baseAddress, List<int> offsets) where TStructure : struct
        {
            IntPtr multiPointerAddress = CMMultiPointer.Instance.FindMultiPointerAddress(handle.ProcessHandle, baseAddress, offsets);
            return readMemory.ReadVirtualMemory<TStructure>(handle.ProcessHandle, multiPointerAddress);
        }
        /// <summary>
        /// Reads an array of bytes from a region of virtual memory in the remote process
        /// </summary>
        public byte[] ReadVirtualMemory(IntPtr baseAddress, List<int> offsets, int bytesToRead)
        {
            IntPtr multiPointerAddress = CMMultiPointer.Instance.FindMultiPointerAddress(handle.ProcessHandle, baseAddress, offsets);
            return readMemory.ReadVirtualMemory(handle.ProcessHandle, multiPointerAddress, bytesToRead);
        }
        //
        //
        //====================================================================================================================================
        //
        //
        /// <summary>
        /// Writes an array of bytes into a region of virtual memory in the remote process
        /// </summary>
        public void WriteVirtualMemory(IntPtr baseAddress, byte[] bytesToWrite)
        {
            writeMemory.WriteVirtualMemory(handle.ProcessHandle, baseAddress, bytesToWrite);
        }
        /// <summary>
        /// Writes a structure into a region of virtual memory in the remote process ( Module + Relative Offset )
        /// </summary>
        public void WriteVirtualMemory<TStructure>(IntPtr baseAddress, TStructure structureToWrite) where TStructure : struct
        {
            writeMemory.WriteVirtualMemory(handle.ProcessHandle, baseAddress, structureToWrite);
        }
        /// <summary>
        /// Writes a structure into a region of virtual memory in the remote process ( Module + Relative Offset )
        /// </summary>
        public void WriteVirtualMemory<TStructure>(string moduleName, int relativeOffset, TStructure structureToWrite) where TStructure : struct
        {
            ProcessModule mod = Module.Instance.GetModuleByName(moduleName);
            writeMemory.WriteVirtualMemory(handle.ProcessHandle, mod.BaseAddress + relativeOffset, structureToWrite);
        }
        /// <summary>
        /// Writes a structure into a region of virtual memory in the remote process  (Multi Pointer)
        /// </summary>
        public void WriteVirtualMemory<TStructure>(string moduleName, int relativeOffset, List<int> offsets, TStructure structureToWrite) where TStructure : struct
        {
            ProcessModule mod = Module.Instance.GetModuleByName(moduleName);
            IntPtr multiPointerAddress = CMMultiPointer.Instance.FindMultiPointerAddress(handle.ProcessHandle, mod.BaseAddress + relativeOffset, offsets);
            writeMemory.WriteVirtualMemory(handle.ProcessHandle, multiPointerAddress, structureToWrite);
        }
        /// <summary>
        /// Writes a structure into a region of virtual memory in the remote process  (Multi Pointer)
        /// </summary>
        public void WriteVirtualMemory<TStructure>(IntPtr baseAddress, List<int> offsets, TStructure structureToWrite) where TStructure : struct
        {
            IntPtr multiPointerAddress = CMMultiPointer.Instance.FindMultiPointerAddress(handle.ProcessHandle, baseAddress, offsets);
            writeMemory.WriteVirtualMemory(handle.ProcessHandle, multiPointerAddress, structureToWrite);
        }
        /// <summary>
        /// Reads an array of bytes from a region of virtual memory in the remote process
        /// </summary>
        public void WriteVirtualMemory(IntPtr baseAddress, List<int> offsets, byte[] bytesToWrite)
        {
            IntPtr multiPointerAddress = CMMultiPointer.Instance.FindMultiPointerAddress(handle.ProcessHandle, baseAddress, offsets);
            writeMemory.WriteVirtualMemory(handle.ProcessHandle, multiPointerAddress, bytesToWrite);
        }

        /// <summary>
        /// Get Address from AOB / Pattern Signature.
        /// </summary>
        public IntPtr GetPatternAddress(string moduleName, string pattern)
        {
            ProcessModule mod = Module.Instance.GetModuleByName(moduleName);
            IntPtr aobAddress = CMPatternScan.Instance.PatternScanMod(handle.ProcessHandle, mod.BaseAddress, mod.ModuleMemorySize, pattern);
            return aobAddress;
        }

        /// <summary>
        /// Frees the unused resources
        /// </summary>
        public void Dispose()
        {
            handle.Dispose();
        }

    }
}
