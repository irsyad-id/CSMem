using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading;

namespace CSMem.CMProcess
{
    internal class Handle : IDisposable
    {
        
        private SafeProcessHandle _processHandle;

        /// <summary>
        /// ProcessHandle from another process
        /// </summary>
        public SafeProcessHandle ProcessHandle
        {
            private set { _processHandle = value; }
            get { return _processHandle; }
        }


        /// <summary>
        /// Get another process handler using process id.
        /// </summary>
        public void GetSafeProcessHandleById(int iProcessId)
        {
            try
            {
                ProcessHandle = Process.GetProcessById(iProcessId).SafeHandle;
                Module.Instance.ProcessModuleCollection = Process.GetProcessById(iProcessId).Modules;
            }
            catch (InstanceNotFoundException e)
            {
                throw new InstanceNotFoundException($"Process with pid #{iProcessId} not found.");
            }
        }

        /// <summary>
        /// Get another process handler using process name.
        /// </summary>
        public void GetSafeProcessHandleByName(string sProcessName)
        {
            try
            {
                ProcessHandle = Process.GetProcessesByName(sProcessName).First().SafeHandle;
                Module.Instance.ProcessModuleCollection = Process.GetProcessesByName(sProcessName).First().Modules;
            }
            catch (InstanceNotFoundException e)
            {
                throw new InstanceNotFoundException($"Process with process name #{sProcessName} not found.");
            }
        }

        /// <summary>
        /// Wait until another process handler using process name found.
        /// </summary>
        public void WaitForSafeProcessHandleByName(string sProcessName)
        {
            try { 
                while(Process.GetProcessesByName(sProcessName).Length <= 0)
                {
                    Thread.Sleep(100);
                }
                ProcessHandle = Process.GetProcessesByName(sProcessName).First().SafeHandle;
                Module.Instance.ProcessModuleCollection = Process.GetProcessesByName(sProcessName).First().Modules;
            } catch (InstanceNotFoundException e){
                throw new InstanceNotFoundException($"Process with process name #{sProcessName} not found.");
            }
        }


        public void Dispose()
        {
            _processHandle.Dispose();
        }
    }
}
