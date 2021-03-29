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
            // /C

            Process process = new Process();
            process.StartInfo.FileName = path;
            //process.StartInfo.WorkingDirectory = @"C:\Users\User Asus\source\repos\Narrative Generator\Narrative Generator\bin\Debug";
            process.StartInfo.Arguments = "/k " + commands;

            if (process.Start()) { return true; }
            else { return false; }
        }

        public bool RunGraphviz(string outputPngFileName, string inputDotFileName)
        {
            if (Run("cmd", "dot -Tpng –o" + outputPngFileName + " –" + inputDotFileName))
            {
                return true;
            }
            else { return false; }
        }
    }
}
