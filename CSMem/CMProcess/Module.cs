using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMem.CMProcess
{
    internal class Module
    {
        //Singleton
        private static Module instance = null;
        private static readonly object padlock = new object();

        public ProcessModuleCollection ProcessModuleCollection { set; get; }

        public static Module Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Module();
                        }
                    }
                }
                return instance;
            }
        }

        public Module()
        {
            
        }

        public ProcessModule GetModuleByName(string moduleName)
        {
            return (from ProcessModule module in ProcessModuleCollection where module.ModuleName.Equals(moduleName) select module).FirstOrDefault();
        }
    }
}
