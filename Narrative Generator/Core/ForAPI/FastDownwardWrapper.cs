﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Narrative_Generator
{
    class FastDownwardWrapper
    {
        public bool Run(string path, string commands)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = "/k " + commands;

            if (process.Start()) { return true; }
            else { return false; }
        }

        public bool RunFastDownward(string domainPDDLFileName, string problemPDDLFileName)
        {
            // Options:
            // --alias lama
            // --alias lama-first !!!work!!!
            // --alias seq-opt-bjopl
            // --alias seq-opt-lmcut !!!work!!!
            // --alias seq-sat-fd-autotune-1
            // --alias seq-sat-fd-autotune-2
            // --alias seq-sat-lama-2011

            if (Run("cmd", "python downward\\fast-downward.py --alias lama-first" + " " + domainPDDLFileName + " " + problemPDDLFileName))
            {
                return true;
            }
            else { return false; }
        }
    }
}