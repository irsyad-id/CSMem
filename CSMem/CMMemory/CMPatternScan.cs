using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMem.CMMemory
{
    internal class CMPatternScan
    {
        //Singleton
        private static CMPatternScan instance = null;
        private static readonly object padlock = new object();

        public static CMPatternScan Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CMPatternScan();
                        }
                    }
                }
                return instance;
            }
        }

        private bool CheckPattern(string pattern, byte[] array2check)
        {
            int len = array2check.Length;
            string[] strBytes = pattern.Split(' ');
            int x = 0;
            foreach (byte b in array2check)
            {
                if (strBytes[x] == "?" || strBytes[x] == "??")
                {
                    x++;
                }
                else if (byte.Parse(strBytes[x], NumberStyles.HexNumber) == b)
                {
                    x++;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public IntPtr PatternScanMod(SafeProcessHandle processHandle,IntPtr baseAddy, int dwSize, string pattern)
        {
            byte[] memDump = CMRead.Instance.ReadVirtualMemory(processHandle, baseAddy, dwSize);
            string[] pBytes = pattern.Split(' ');
            try
            {
                for (int y = 0; y < memDump.Length; y++)
                {
                    if (memDump[y] == byte.Parse(pBytes[0], NumberStyles.HexNumber))
                    {
                        byte[] checkArray = new byte[pBytes.Length];
                        for (int x = 0; x < pBytes.Length; x++)
                        {
                            checkArray[x] = memDump[y + x];
                        }
                        if (CheckPattern(pattern, checkArray))
                        {
                            return baseAddy + y;
                        }
                        else
                        {
                            y += pBytes.Length - (pBytes.Length / 2);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
            return IntPtr.Zero;
        }
    }
}
//InfBreath = Mem.PatternScanMod(Mem.ReadProcess.MainModule, "f3 0f 11 86 ? ? ? ? 76 1d f3 0f 5e c1 f3 0f 11 45 08");