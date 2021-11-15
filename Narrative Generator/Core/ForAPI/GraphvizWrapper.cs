using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class GraphvizWrapper
    {
        public bool Run(string path, string commands)
        {
            // @"D:\Graphviz\bin\dot.exe"

            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = "/k " + commands;

            if (process.Start())
            {
                process.WaitForExit();
                return true;
            }
            else { return false; }
        }

        public bool RunGraphviz(string fileName)
        {
            if (Run("cmd", "dot.exe -Tpng –O " + fileName))
            {
                return true;
            }
            else { return false; }
        }
    }
}
