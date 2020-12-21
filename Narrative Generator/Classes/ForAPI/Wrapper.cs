using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Narrative_Generator
{
    class Wrapper
    {
        public void Run(string path, string commands)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = "/k " + commands;
            process.Start();
        }

        public void RunFastDownward(string domainPDDLFileName, string problemPDDLFileName)
        {
            Run("cmd", "python downward\\fast-downward.py --alias seq-opt-bjolp" + " " + domainPDDLFileName + " " + problemPDDLFileName);
        }
    }
}
