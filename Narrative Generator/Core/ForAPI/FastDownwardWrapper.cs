using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// Class serving for low-level interaction with the planner.
    /// </summary>
    class FastDownwardWrapper
    {
        /// <summary>
        /// The class in which the process settings (planner launch) are set and it is launched.
        /// </summary>
        /// <param name="path">Where the process will be launched from.</param>
        /// <param name="commands">The command to pass to the process.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>An indicator of the success or failure of the process launch.</returns>
        public bool Run (string path, string commands, ref TextBox note)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = "/c " + commands;            // с - close, k - not close
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // Hides GUI
            process.StartInfo.CreateNoWindow = true;                   // Hides console

            if (process.Start())
            {
                note.Text = "PROCESS START - CMD";
                process.WaitForExit(/*30000*/);
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// A class that passes some settings and a calling class that directly starts the planner, itself called from the outside.
        /// </summary>
        /// <param name="domainPDDLFileName">The name of the file that describes the planning domain.</param>
        /// <param name="problemPDDLFileName">The name of the file describing the planning problem.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>An indicator of the success or failure of the process launch.</returns>
        public bool RunFastDownward (string domainPDDLFileName, string problemPDDLFileName, ref TextBox note)
        {
            // Options:
            // --alias lama
            // --alias lama-first !!!work!!!
            // --alias seq-opt-bjopl
            // --alias seq-opt-lmcut !!!work!!!
            // --alias seq-sat-fd-autotune-1
            // --alias seq-sat-fd-autotune-2
            // --alias seq-sat-lama-2011

            if (Run("cmd", "python downward\\fast-downward.py --alias seq-opt-lmcut" + " " + domainPDDLFileName + " " + problemPDDLFileName, ref note))
            {
                return true;
            }
            else { return false; }
        }
    }
}
